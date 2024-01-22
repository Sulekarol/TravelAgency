using System;
using MySql.Data.MySqlClient;

namespace TravelAgency
{
    interface IDBConnect
    {
        void GetData()
        {
            //your MySQL connection string
            string connStr = "server=localhost;user=root;database=biuropodrozy;port=3306;password=Nolypiok208";

            MySqlConnection conn = new MySqlConnection(connStr);



            conn.Open();

            //SQL Query to execute
            //selecting only first 10 rows for demo
            string sql = "select * from Oferty_podrozy limit 0,3;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            //read the data
            rdr.Read();
            Console.WriteLine(rdr[1] + " -- " + rdr[4] + " Euro" + " -- " + rdr[5]);

            rdr.Close();


            conn.Close();


        }


        void InsertData(string destynation, DateTime dateOfDept, string arrived, string price, string tourDescription, string places)
        {
            string connStr = "server=localhost;user=root;database=buropodrozy;password=Nolypiok208";

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("");
                conn.Open();

                // SQL Query to execute
                // insert Query
                // we are inserting destynation, departure, arrived, price, tourDescription, places columns data

                string sql = "INSERT INTO sakila.actor (destynation, departure, arrived, price, tourDescription, places) VALUES (@destynation, @departure, @arrived, @price, @tourDescription, @places)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@destynation", destynation);
                cmd.Parameters.AddWithValue("@departure", dateOfDept);
                cmd.Parameters.AddWithValue("@arrived", arrived);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@tourDescription", tourDescription);
                cmd.Parameters.AddWithValue("@places", places);
                cmd.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                Console.WriteLine($"Something went wrong: {err.Message}");
            }
            finally
            {
                conn.Close();
            }

            Console.SetCursorPosition(12, 4);
            Console.WriteLine("Great! Your Tour Adding was successful! ");
            Console.SetCursorPosition(12, 4);
            Console.WriteLine("If you want to see your tour, go to the \"Searching Offer\" page and check it out.");

            Console.ReadKey();
        }
    }
    
}

