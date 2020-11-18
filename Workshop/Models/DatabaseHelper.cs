using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Workshop.Models
{
    public class DatabaseHelper
    {
        private MySqlConnectionStringBuilder _builder { get; set; }

        private string _connectionString { get; set; }


        public DatabaseHelper()
        {
            _builder = new MySqlConnectionStringBuilder
            {
                Server = "vps-f0d101aa.vps.ovh.net",
                Database = "workshop",
                UserID = "BLegendre",
                Password = "EPSIworkshop2020*",
                SslMode = MySqlSslMode.None
            };

            _connectionString = "server=vps-f0d101aa.vps.ovh.net;user=BLegendre;database=workshop;password=EPSIworkshop2020*";
        }


        public void Insert()
        {
            using (MySqlConnection connection = new MySqlConnection(_builder.ConnectionString))
            {
                connection.Open();

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "LA REQUETE SQL";
                    command.Parameters.AddWithValue("@Parametre", 100);
                    command.ExecuteNonQuery();
                }
            }
        }

        // CECI EST UN EXEMPLE
        //public List<Personne> SelectPersonne()
        //{
        //    List<Personne> personnes = new List<Personne>();

        //    using (MySqlConnection connection = new MySqlConnection(_builder.ConnectionString))
        //    {
        //        connection.Open();

        //        using (MySqlCommand command = connection.CreateCommand())
        //        {
        //            command.CommandText = "SELECT * FROM Personne";

        //            using (MySqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    Personne personne = new Personne
        //                    {
        //                        Id = reader.GetInt32("Id"),
        //                        Nom = reader.GetString("Nom"),
        //                        Prenom = reader.GetString("Prenom"),
        //                    };

        //                    personnes.Add(personne);
        //                }
        //            }
        //        }
        //    }

        //    return personnes;
        //}
    }
}