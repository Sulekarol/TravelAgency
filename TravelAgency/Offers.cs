using System;
using MySql.Data.MySqlClient;


namespace TravelAgency

{
	class Offers
	{
		
			public void GetOffer()
			{
				// MySql conection
				string conStr = "server=localhost;user=root;database=biuropodrozy;port=3306;password=Nolypiok208";

				MySqlConnection conn = new MySqlConnection(conStr);
				
				//Console.WriteLine("Connection with database");
				conn.Open();

				string sql = "select * from Oferty_podrozy where Id_oferty_podrozy = 1;";
				MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					Console.WriteLine(rdr[1] + "--" + rdr[2] + "--" + rdr[3] + "--" + rdr[4] );
				}
				rdr.Close();
				

			}
		
	}
}

