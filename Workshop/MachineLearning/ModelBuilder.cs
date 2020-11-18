using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.TimeSeries;


using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

using Workshop.Models;

namespace Workshop.MachineLearning
{
    public static class ModelBuilder
    {
        private static MLContext _mlContext = new MLContext();


        public static void CreateModel()
        {
            string rootDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../"));
            string modelPath = Path.Combine(rootDir, "MLModel.zip");

            DatabaseHelper helper = new DatabaseHelper();
            string query = "SELECT e.jour, f.matricule, SUM(e.nbPassant) as nombre FROM etat e INNER JOIN feu f ON e.idFeu = f.idFeu GROUP BY f.matricule, e.jour";

            DatabaseLoader loader = _mlContext.Data.CreateDatabaseLoader<ModelInput>();
            DatabaseSource databaseSource = new DatabaseSource(SqlClientFactory.Instance, helper.Builder.ConnectionString, query);

            IDataView dataView = loader.Load(databaseSource);
            IDataView firstWeekData = _mlContext.Data.FilterRowsByColumn(dataView, "Jour", upperBound: 1);
            IDataView nextWeekData = _mlContext.Data.FilterRowsByColumn(dataView, "Jour", lowerBound: 1);

            var forecastingPipeline = _mlContext.Forecasting.ForecastBySsa(
                outputColumnName: "ForecastedPassants",
                inputColumnName: "NbPassants",
                windowSize: 7,
                seriesLength: 4,
                trainSize: 28,
                horizon: 7,
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: "LowerBoundPassants",
                confidenceUpperBoundColumn: "UpperBoundPassants");

            SsaForecastingTransformer forecaster = forecastingPipeline.Fit(firstWeekData);

            Evaluate(nextWeekData, forecaster, _mlContext);

            var forecastEngine = forecaster.CreateTimeSeriesEngine<ModelInput, ModelOutput>(_mlContext);
            forecastEngine.CheckPoint(_mlContext, modelPath);

            Forecast(nextWeekData, 7, forecastEngine, _mlContext);
        }

        static void Evaluate(IDataView testData, ITransformer model, MLContext mlContext)
        {
            IDataView predictions = model.Transform(testData);

            IEnumerable<int> actual = mlContext.Data.CreateEnumerable<ModelInput>(testData, true).Select(observed => observed.NbPassants);
            IEnumerable<int> forecast = mlContext.Data.CreateEnumerable<ModelOutput>(predictions, true).Select(prediction => prediction.ForecastedPassants[0]);
            var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);

            var MAE = metrics.Average(error => Math.Abs(error)); // Mean Absolute Error
            var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); // Root Mean Squared Error

            //Console.WriteLine("Evaluation Metrics");
            //Console.WriteLine("---------------------");
            //Console.WriteLine($"Mean Absolute Error: {MAE:F3}");
            //Console.WriteLine($"Root Mean Squared Error: {RMSE:F3}\n");
        }

        static void Forecast(IDataView testData, int horizon, TimeSeriesPredictionEngine<ModelInput, ModelOutput> forecaster, MLContext mlContext)
        {
            ModelOutput forecast = forecaster.Predict();

            IEnumerable<string> forecastOutput = mlContext.Data.CreateEnumerable<ModelInput>(testData, reuseRowObject: false)
                .Take(horizon)
                .Select((ModelInput passants, int index) =>
                {
                    string jour = Enum.GetName(typeof(DayOfWeek), passants.Jour);
                    int actualPassants = passants.NbPassants;
                    float lowerEstimate = Math.Max(0, forecast.LowerBoundPassants[index]);
                    float estimate = forecast.ForecastedPassants[index];
                    float upperEstimate = forecast.UpperBoundPassants[index];
                    return $"Date: {jour}\n" +
                    $"Actual Rentals: {actualPassants}\n" +
                    $"Lower Estimate: {lowerEstimate}\n" +
                    $"Forecast: {estimate}\n" +
                    $"Upper Estimate: {upperEstimate}\n";
                });

            //Console.WriteLine("Rental Forecast");
            //Console.WriteLine("---------------------");
            //foreach (var prediction in forecastOutput)
            //{
            //    Console.WriteLine(prediction);
            //}
        }
    }
}