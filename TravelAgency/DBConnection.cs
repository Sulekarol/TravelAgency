using System;
using MySql.Data.MySqlClient;

namespace TravelAgency
{
    class DBConnect
    {
        public virtual void getData()
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
        public virtual void InsertData()
        {
            string connStr = "server=localhost;user=root;database=buropodrozy=3306;password=Nolypiok208";

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                //SQL Query to execute
                //insert Query
                // we are inserting actor_id, first_name, last_name, last_updated columns data

                string sql = "INSERT INTO sakila.actor VALUES ('202','First Name Actor test','Last Name Actor test', '2020-11-05 04:34:33')";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();


            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }

            conn.Close();
            Console.WriteLine("Great, Your tour is added to our data Base ! ");
            Console.ReadKey();
        }
    }
    
}

