using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;
using ConditionalCompilationElseIf = Microsoft.Ajax.Utilities.ConditionalCompilationElseIf;

namespace Workshop.Models
{
    public class DatabaseHelper
    {
        public MySqlConnectionStringBuilder Builder { get; set; }


        public DatabaseHelper()
        {
            Builder = new MySqlConnectionStringBuilder
            {
                Server = "vps-f0d101aa.vps.ovh.net",
                Database = "workshop",
                UserID = "BLegendre",
                Password = "EPSIworkshop2020*",
                SslMode = MySqlSslMode.None
            };
        }

        //Script d'insertion

        private static int genererAlea(Random random, int h, int m)
        {
            int time = h * m;
            if (time < 360)
            {
                return random.Next(0, 3);
            } 
            else if (time < 420)
            {
                return random.Next(2, 6);
            } 
            else if (time < 540)
            {
                return random.Next(4, 9);
            } 
            else if (time < 780)
            {
                return random.Next(2, 6);
            } 
            else if (time < 840)
            {
                return random.Next(4, 9);
            } 
            else if (time < 960)
            {
                return random.Next(2, 6);
            } 
            else if (time < 1140)
            {
                return random.Next(4, 9);
            } 
            else if (time < 1260)
            {
                return random.Next(2, 5);
            }
            else if (time < 1440)
            {
                return random.Next(0, 3);
            }

            return 0;
        }

        public static void ScriptInsertion()
        {
            DatabaseHelper helper = new DatabaseHelper();

            helper.insertOneFeu(new Feu() { matricule = "C1-HR1" });
            helper.insertOneFeu(new Feu() { matricule = "C1-VR1" });
            helper.insertOneFeu(new Feu() { matricule = "C1-HR2" });
            helper.insertOneFeu(new Feu() { matricule = "C1-VR2" });

            Dictionary<string, Feu> feux = helper.selectFeux();
            

            Random random = new Random();

            for (int jour = 0; jour < 7; jour++)
            {
                for (int h = 0; h < 24; h++)
                {
                    for (int m = 0; m < 60; m++)
                    {
                        bool etatFeu = random.Next(0, 2) == 0;

                        List <Feu> feuxHR = feux.Values.Where(f => f.matricule.StartsWith("C1-HR")).ToList();
                        feuxHR.ForEach(f =>
                        {
                            Etat etat = new Etat()
                            {
                                etat = etatFeu,
                                feu = f,
                                jour = (JourEnum) jour,
                                horaire = new DateTime(1, 1, 1, h, m, 0),
                                nbPassant = (genererAlea(random, h, m))
                            };
                            helper.insertOneEtat(etat);
                        });
                        
                        List<Feu> feuxVR = feux.Values.Where(f => f.matricule.StartsWith("C1-VR")).ToList();
                        feuxVR.ForEach(f =>
                        {
                            Etat etat = new Etat()
                            {
                                etat = !etatFeu,
                                feu = f,
                                jour = (JourEnum)jour,
                                horaire = new DateTime(1, 1, 1, h, m, 0),
                                nbPassant = (genererAlea(random, h, m))
                            };
                            helper.insertOneEtat(etat);
                        });
                    }
                }
            }
        }

        // Jour

        public void insertOneEtat(Etat etat)
        {
            using (MySqlConnection connection = new MySqlConnection(Builder.ConnectionString))
            {
                connection.Open();

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO etat(idFeu, jour, horaire, nbPassant, etat) VALUES (@feu, @jour, @horaire, @passants, @etat);";
                    command.Parameters.AddWithValue("@feu", etat.feu.idFeu);
                    command.Parameters.AddWithValue("@jour", etat.jour);
                    command.Parameters.AddWithValue("@horaire", etat.horaire);
                    command.Parameters.AddWithValue("@passants", etat.nbPassant);
                    command.Parameters.AddWithValue("@etat", etat.etat);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void insertOneFeu(Feu feu)
        {
            using (MySqlConnection connection = new MySqlConnection(Builder.ConnectionString))
            {
                connection.Open();

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO feu(matricule) VALUES (@matricule)";
                    command.Parameters.AddWithValue("@matricule", feu.matricule);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Dictionary<string, Feu> selectFeux()
        {
            Dictionary<string, Feu> feux = new Dictionary<string, Feu>();

            using (MySqlConnection connection = new MySqlConnection(Builder.ConnectionString))
            {
                connection.Open();

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM feu";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        { 
                            Feu feu = new Feu
                            {
                                idFeu = reader.GetInt32("idFeu"),
                                matricule = reader.GetString("matricule")
                            };

                            feux.Add(feu.matricule, feu);
                        }
                    }
                }
            }

            return feux;
        }

        public List<Etat> SelectNombreVoitureParVoie(int jour)
        {
            List<Etat> etat = new List<Etat>();

            using (MySqlConnection connection = new MySqlConnection(Builder.ConnectionString))
            {
                connection.Open();

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT e.jour, f.idFeu, f.matricule, SUM(e.nbPassant) as nombre FROM etat e INNER JOIN feu f ON e.idFeu = f.idFeu WHERE e.jour = @jour GROUP BY f.matricule, e.jour";
                    command.Parameters.AddWithValue("@jour", jour);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Feu feu = new Feu
                            {
                                idFeu = reader.GetInt32("idFeu"),
                                matricule = reader.GetString("matricule")
                            };

                            Etat unEtat = new Etat
                            {
                                jour = (JourEnum)reader.GetInt32("jour"),
                                feu = feu,
                                nbPassant = reader.GetInt32("nombre"),
                                etat = false
                            };

                            etat.Add(unEtat);
                        }
                    }
                }
            }

            return etat;
        }
    }
}