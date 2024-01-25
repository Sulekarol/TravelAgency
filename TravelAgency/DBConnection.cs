using System;
using MySql.Data.MySqlClient;

namespace TravelAgency
{
    interface IDBConnect
    {
        static void GetData()
        {
            //your MySQL connection string
            string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

            MySqlConnection conn = new MySqlConnection(connStr);



            conn.Open();

            //SQL Query to execute
            //selecting only first 10 rows for demo
            string sql = "SELECT Cel_podrozy,Data_rozpoczecia,Data_zakonczenia,Cena,Opis,Dostepne_miejsca FROM Oferty_podrozy WHERE Data_rozpoczecia >= '2023-03-03';";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            Console.WriteLine("This is all what you looking for");
            Console.WriteLine();
            //read the data
            while (rdr.Read())
            {

                rdr.Read();
                Console.WriteLine("" + rdr[0] + " -- " + rdr[1] + " Euro" + " -- " + rdr[2] + " -- \n" + rdr[3] + " Euro" + " -- " + rdr[4] + " -- " + rdr[5]);
            }

            rdr.Close();


            conn.Close();


        }


        void InsertData(string password, string destynation, DateTime dateOfDept, string arrived, string price, string tourDescription, string places)
        {
            string connStr = $"server=localhost;user=root;database=buropodrozy;port=3306;password=";

            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                Console.WriteLine("");
                conn.Open();

                // SQL Query to execute
                // insert Query
                // we are inserting destynation, departure, arrived, price, tourDescription, places columns data

                string sql = $"INSERT INTO Oferty_podrozy (Cel_podrozy, Data_rozpoczenca, Data_zakonczenia, Cena, Opis, Dostepne_miejsca) VALUES ({destynation},{dateOfDept},{arrived},{price},{tourDescription},{places})";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                Console.WriteLine();
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

