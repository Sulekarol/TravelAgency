using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Security;
using System.ComponentModel;
using TravelAgency;




namespace TravelAgency

{
    // MAIN MENU 

    public class Home : Menu, IDBConnect, ILogo, ISecretP
    {
        public static new string[] positionMenu ={"[] Searching offer","[] Add New Offer","[] Edit Offer",
        "[] Delete Offer","[] Ticket reservation"
        ,"End"};


        static new int activeMenuPosition = 0;



        public override void StartMenu(Home home)
        {


            Console.CursorVisible = false;
            while (true)
            {


                ShowMenu();
                ChooseOption();
                GetOption(home);
            }

        }

        public override void ShowMenu()
        {

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine();
            ILogo.ShowLogo();
            Console.WriteLine();




            for (int j = 0; j < positionMenu.Length; j++)
            {
                if (j == activeMenuPosition)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("{0,-35}", positionMenu[j]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                }
                else
                {
                    Console.WriteLine(positionMenu[j]);
                }
            }
        }
        public override void ChooseOption()
        {
            do
            {
                ConsoleKeyInfo button = Console.ReadKey();
                if (button.Key == ConsoleKey.UpArrow)
                {
                    activeMenuPosition = (activeMenuPosition > 0) ? activeMenuPosition - 1 : positionMenu.Length - 1;
                    ShowMenu();

                }
                else if (button.Key == ConsoleKey.DownArrow)
                {
                    activeMenuPosition = (activeMenuPosition + 1) % positionMenu.Length;
                    ShowMenu();
                }
                else if (button.Key == ConsoleKey.Escape)
                {
                    activeMenuPosition = positionMenu.Length - 1;
                    break;

                }
                else if (button.Key == ConsoleKey.Enter)
                    break;

            } while (true);


        }


        public override void GetOption(Home home)
        {


            switch (activeMenuPosition)
            {
                case 0: Console.Clear(); SerchingOffer(home); break;
                case 1: Console.Clear(); AddNewData(home); break;
                case 2: Console.Clear(); EditOffer(home); break;
                case 3: Console.Clear(); Delate(home); break;
                case 4: Console.Clear(); OptionUnderConstruction(); break;
                case 5: Environment.Exit(0); break;

            }
        }



        public override void OptionUnderConstruction()
        {
            Console.SetCursorPosition(12, 4);
            Console.Write("We Work on this, Sorry :(");
            Console.ReadKey();
        }




        // ADD NEW TOUR PAGE 

        public static void AddNewData(Home home)
        {




            Console.WriteLine("Add New Offer:");


            Console.Write("If you want to add a new trip, enter the password to the company database.\n" +
                            "Enter Password:");

            string password = ISecretP.GetMaskedInput();




            if (password.Length <= 0)
            {
                Console.WriteLine("Are you forget insert a password. Are you wont try again? (select YES - enter , NO- escape)");
                Console.ReadKey();
            }




            Console.WriteLine();


            Console.SetCursorPosition(12, 4);
            Console.Write("Add new offer !");
            Console.WriteLine();


            Console.Write("Where ? (City/Country) :");
            string destynation = Console.ReadLine();

            if (destynation.Length <= 3)
            {
                Console.WriteLine("Something went wrong, name of your City and country is too short. Try again :) ");
                Console.Clear();
                AddNewData(home);
                return; // Add return to eneble software use 
            }


            Console.WriteLine();
            Console.Write("When? (departure- Y-M-D): ");
            string dept = Console.ReadLine();

            DateTime date = DateTime.Parse(dept);

            if (date <= DateTime.Now)
            {
                Console.WriteLine("Invalid date format. Please enter the date in the correct format (Y-M-D).");
                Console.WriteLine();
                Console.Clear();
                AddNewData(home);
                return;

            }


            else if (date > DateTime.Now)
            {
                Console.WriteLine("Looks great, keep moving");
            }
            else
            {
                Console.WriteLine("Something went wrong, your date of departure is in the past. Try again :) ");
                AddNewData(home);
                Console.Clear();
                return;
            }
            string departure = date.ToString();


            Console.Write("When? (arrived - Y-M-D): ");
            string arrived = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Price:");
            string price = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Write few words about this tour: ");
            string tourDescription = Console.ReadLine();
            Console.WriteLine();
            int id = 8;
            Console.Write("How many places are available? :");
            string places = Console.ReadLine();
            InsertData(password, destynation, departure, arrived, price, tourDescription, places);

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine("Adding new offer cancelled.");
                        break; // Break out of the loop to exit the function
                    }
                }
            }


            Console.ReadKey();



        }

        // INSER DATA FOR ADDING PAGE 

        static void InsertData(string password, string destynation, string departure, string arrived, string price, string tourDescription, string places)
        {
            string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

            string sql1 = "SELECT Id_oferty_podrozy FROM Oferty_podrozy ORDER BY Id_oferty_podrozy DESC LIMIT 1;";
            int sql3 = 0;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql1, conn);
                using (MySqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        // Pobranie wartości zapytania SQL i konwersja na int
                        sql3 = rdr.GetInt32(0);
                    }
                }
            }
            sql3++;
            string sql2 = sql3.ToString();


            string connStr1 = $"server=localhost;user=root;database=biuropodrozy;port=3306;password={password}";

            MySqlConnection conn1 = new MySqlConnection(connStr1);
            try
            {

                conn1.Open();



                string sql = $"INSERT INTO Oferty_podrozy (Id_oferty_podrozy,Cel_podrozy, Data_rozpoczecia, Data_zakonczenia, Cena, Opis, Dostepne_miejsca) VALUES ({sql2},'{destynation}','{departure}','{arrived}',{price},'{tourDescription}',{places})";
                MySqlCommand cmd = new MySqlCommand(sql, conn1);
                MySqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                Console.WriteLine();
                rdr.Close();
            }
            catch (Exception err)
            {

                Console.Clear();
                Console.WriteLine($"Something went wrong: {err.Message}");

            }
            finally
            {
                conn1.Close();

                Console.Clear();
                Console.SetCursorPosition(12, 4);
                Console.WriteLine("Great! Your Tour Adding was successful! ");
                Console.WriteLine("If you want to see your tour, go to the \"Searching Offer\" page and check it out.");
                Console.ReadKey();


            }



        }


        static string[] menuOption = { "[] When", "[] Where", "[] Price", "Back" };
        static int activeMenuOption = 0;

        //SEARCHING OFFER PAGE 

        public static void SerchingOffer(Home home)
        {



            string[] menuOption = { "[] When", "[] Where", "[] Price", "Back" };

            Console.CursorVisible = false;
            while (true)
            {


                MenuSerch();
                Option();
                Decide(home);
            }
        }

        static void MenuSerch()
        {


            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine();
            Console.WriteLine("Serching Offer:");
            Console.SetCursorPosition(12, 4);
            Console.WriteLine("Choose an option which will research for you the data");






            for (int j = 0; j < menuOption.Length; j++)

                if (j == activeMenuOption)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("{0,-35}", menuOption[j]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                }
                else
                {
                    Console.WriteLine(menuOption[j]);
                }


        }
        static void Option()
        {
            do
            {
                ConsoleKeyInfo button = Console.ReadKey();
                if (button.Key == ConsoleKey.UpArrow)
                {
                    activeMenuOption = (activeMenuOption > 0) ? activeMenuOption - 1 : menuOption.Length - 1;
                    MenuSerch();

                }
                else if (button.Key == ConsoleKey.DownArrow)
                {
                    activeMenuOption = (activeMenuOption + 1) % menuOption.Length;
                    MenuSerch();
                }
                else if (button.Key == ConsoleKey.Escape)
                {
                    activeMenuOption = menuOption.Length - 1;
                    break;

                }
                else if (button.Key == ConsoleKey.Enter)
                    break;


            } while (true);

        }

        static void Decide(Home home)
        {
            switch (activeMenuOption)
            {
                case 0: Console.Clear(); When(home); break;
                case 1: Console.Clear(); Where(); break;
                case 2: Console.Clear(); Price(); break;
                case 3: Console.Clear(); home.StartMenu(home); break;

            }
        }

        static void When(Home home)
        {
            ConsoleKeyInfo key;
            Console.WriteLine("Searching offer");
            Console.WriteLine();
            Console.Write("Enter departure data:");
            string from = Console.ReadLine();

            Console.WriteLine();
            GetData(from);
            Console.ReadKey();

            static void GetData(string from)
            {
                //your MySQL connection string
                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                MySqlConnection conn = new MySqlConnection(connStr);


                try
                {
                    conn.Open();

                    //SQL Query to execute
                    //selecting only first 10 rows for demo
                    string sql = $"SELECT Cel_podrozy,Data_rozpoczecia,Data_zakonczenia,Cena,Opis,Dostepne_miejsca FROM Oferty_podrozy WHERE Data_rozpoczecia >= '{from}';";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();



                    if (rdr.Read())
                    {
                        do
                        {
                            Console.WriteLine(rdr[0] + "--" + rdr[1] + " Euro" + "--" + rdr[2] + "--" + rdr[3] + " Euro" + " -- " + rdr[4] + " -- " + rdr[5]);
                            Console.WriteLine();
                        }
                        while (rdr.Read());
                    }
                    else
                    {
                        Console.WriteLine("Brak danych.");
                    }



                    rdr.Close();
                }
                catch (Exception exx)
                {
                    Console.WriteLine($"Somthing went wrong: {exx}");
                }
                finally
                {


                    conn.Close();

                }



            }
        }

        static void Where()
        {
            Console.WriteLine("");

            ConsoleKeyInfo key;
            Console.WriteLine("Searching offer");

            Console.WriteLine();
            Console.Write("Enter place where you wont to go:");

            string destynation = Console.ReadLine();
            Console.WriteLine();

            GetData(destynation);
            Console.ReadKey();

            static void GetData(string destynation)
            {
                //your MySQL connection string
                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                MySqlConnection conn = new MySqlConnection(connStr);


                try
                {
                    conn.Open();

                    //SQL Query to execute
                    //selecting only first 10 rows for demo
                    string sql = $"SELECT Cel_podrozy,Data_rozpoczecia,Data_zakonczenia,Cena,Opis,Dostepne_miejsca FROM Oferty_podrozy WHERE Cel_podrozy = '{destynation}';";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();



                    if (rdr.Read())
                    {
                        do
                        {
                            Console.WriteLine(rdr[0] + "--" + rdr[1] + " Euro" + "--" + rdr[2] + "--" + rdr[3] + " Euro" + " -- " + rdr[4] + " -- " + rdr[5]);
                            Console.WriteLine();
                        }
                        while (rdr.Read());
                    }
                    else
                    {
                        Console.WriteLine("Brak danych.");
                    }

                    rdr.Close();

                }
                catch (Exception exx)
                {
                    Console.WriteLine($"Somthing went wrong: {exx}");
                }
                finally
                {


                    conn.Close();

                }
            }
        }

        static void Price()
        {

            ConsoleKeyInfo key;

            Console.WriteLine("Searching offer");
            Console.WriteLine();
            Console.Write("Enter the amount you would like to spend on the trip :");

            string price = Console.ReadLine();

            double amount = double.Parse(price);
            Console.WriteLine();
            GetData(amount);
            Console.ReadKey();

            static void GetData(double amount)
            {
                //your MySQL connection string
                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                MySqlConnection conn = new MySqlConnection(connStr);


                try
                {
                    conn.Open();

                    //SQL Query to execute
                    //selecting only first 10 rows for demo
                    string sql = $"SELECT Cel_podrozy,Data_rozpoczecia,Data_zakonczenia,Cena,Opis,Dostepne_miejsca FROM Oferty_podrozy WHERE Cena <= {amount};";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();



                    if (rdr.Read())
                    {
                        do
                        {
                            Console.WriteLine(rdr[0] + "--" + rdr[1] + " Euro" + "--" + rdr[2] + "--" + rdr[3] + " Euro" + " -- " + rdr[4] + " -- " + rdr[5]);
                            Console.WriteLine();
                        }
                        while (rdr.Read());
                    }
                    else
                    {
                        Console.WriteLine("Brak danych.");
                    }

                    rdr.Close();
                }
                catch (Exception exx)
                {
                    Console.WriteLine($"Somthing went wrong: {exx}");
                }
                finally
                {


                    conn.Close();

                }
            }

        }

        //// EDIT OFFER

        public static string[] position ={"[] Destynation","[] Price","[] Departure",
        "[] Arived","[] Descryption","[] Avilable places"
        ,"Back"};


        public static int activPosition = 0;

        public static int OfferID { get; private set; }

        static void EditOffer(Home home)
        {
            Console.WriteLine("Edit offer:");
            Console.WriteLine();
            Console.WriteLine("If you want change either offer, select suitable offer..");
            GetData(home);
            static void GetData(Home home)
            {
                //your MySQL connection string
                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                MySqlConnection conn = new MySqlConnection(connStr);



                conn.Open();

                //SQL Query to execute
                //selecting only first 10 rows for demo
                string sql = "SELECT * FROM Oferty_podrozy ";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();


                Console.WriteLine();
                //read the data
                while (rdr.Read())
                {


                    Console.WriteLine("ID: " + rdr[0] + " -- " + rdr[1] + " Euro" + " -- " + rdr[2] + " -- \n" + rdr[3] + " Euro" + " -- " + rdr[4] + " -- " + rdr[5]);
                    Console.WriteLine();
                }

                rdr.Close();


                conn.Close();
            }
            Console.Write("Insert ID Ofer wich you want to change:");

            string ID = Console.ReadLine();
            int OfferID = int.Parse(ID);

            GetData1(OfferID);






            static void GetData1(int OfferID)
            {
                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                MySqlConnection conn = new MySqlConnection(connStr);



                conn.Open();

                //SQL Query to execute
                //selecting only first 10 rows for demo
                string sql = $"SELECT * FROM Oferty_podrozy WHERE Id_oferty_podrozy = {OfferID}";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();


                Console.WriteLine();
                //read the data
                while (rdr.Read())
                {
                    Console.Clear();
                    rdr.Read();
                    Console.WriteLine("ID: " + rdr[0] + " -- " + "\nDestynation: " + rdr[1] + "\nPrice: " + rdr[2] + " Euro" + "\nDeparture: " + rdr[3] + "\nArived: " + rdr[4] + "\nDescription: " + rdr[5] + "\nAvilable places: " + rdr[6]);
                    Console.WriteLine();
                }

                rdr.Close();


                conn.Close();
            }

            ChangeOfferMenu(home, OfferID);



            static void ChangeOfferMenu(Home home, int OfferID)
            {


                Console.CursorVisible = false;
                while (true)
                {


                    show(home, OfferID);
                    ChooseOpt(home, OfferID);
                    Decide(home, OfferID);
                }

            }

            static void show(Home home, int OfferID)
            {


                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine();

                GetData1(OfferID);

                static void GetData1(int OfferID)
                {
                    string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                    MySqlConnection conn = new MySqlConnection(connStr);



                    conn.Open();

                    //SQL Query to execute
                    //selecting only first 10 rows for demo
                    string sql = $"SELECT * FROM Oferty_podrozy WHERE Id_oferty_podrozy = {OfferID}";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();


                    Console.WriteLine();
                    //read the data
                    while (rdr.Read())
                    {
                        Console.Clear();
                        rdr.Read();
                        string ofertaInfo = $"ID: {rdr[0]} \nDestynation:  {rdr[1]} \nPrice:  {rdr[2]}  Euro \nDeparture: {rdr[3]} \nArived: {rdr[4]} \nDescription: {rdr[5]} \nAvilable places: {rdr[6]}";
                        Console.WriteLine(ofertaInfo);
                        Console.WriteLine();
                    }

                    rdr.Close();


                    conn.Close();
                }

                Console.WriteLine("What part of this tour are you want to change ? ");


                for (int k = 0; k < position.Length; k++)

                    if (k == activPosition)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("{0,-35}", position[k]);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.DarkGreen;

                    }
                    else
                    {
                        Console.WriteLine(position[k]);
                    }


            }
            static void ChooseOpt(Home home, int OfferID)
            {
                do
                {
                    ConsoleKeyInfo button = Console.ReadKey();
                    if (button.Key == ConsoleKey.UpArrow)
                    {
                        activPosition = (activPosition > 0) ? activPosition - 1 : position.Length - 1;
                        ChangeOfferMenu(home, OfferID);

                    }
                    else if (button.Key == ConsoleKey.DownArrow)
                    {
                        activPosition = (activPosition + 1) % position.Length;
                        ChangeOfferMenu(home, OfferID);
                    }
                    else if (button.Key == ConsoleKey.Escape)
                    {
                        activPosition = position.Length - 1;
                        break;

                    }
                    else if (button.Key == ConsoleKey.Enter)
                        break;

                } while (true);


            }


            static void Decide(Home home, int OfferID)
            {


                switch (activPosition)
                {
                    case 0: Console.Clear(); DestynationEdit(OfferID); break;
                    case 1: Console.Clear(); PriceEdit(OfferID); break;
                    case 2: Console.Clear(); DepartureEdit(OfferID); break;
                    case 3: Console.Clear(); ArivedEdit(OfferID); break;
                    case 4: Console.Clear(); DescryptionEdit(OfferID); break;
                    case 5: Console.Clear(); PlaceEdit(OfferID); break;
                    case 6: Console.Clear(); home.StartMenu(home); break;

                }
            }

        }
        static void DestynationEdit(int OfferID)
        {
            Console.WriteLine("Destynation Editor:");
            Console.WriteLine();

            Console.WriteLine();
            Console.Write("Insert new destynation (City,Country):");
            string NewDest = Console.ReadLine();

            EditData(NewDest, OfferID);

            static void EditData(string NewDest, int OfferID)
            {
                //your MySQL connection string
                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                MySqlConnection conn = new MySqlConnection(connStr);


                try
                {
                    conn.Open();

                    //SQL Query to execute
                    //selecting only first 10 rows for demo
                    string sql = $"UPDATE Oferty_podrozy SET Cel_podrozy = '{NewDest}' WHERE ID_oferty_podrozy = {OfferID};";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    int rowsAffected = cmd.ExecuteNonQuery();


                    if (rowsAffected > 0)
                    {
                        Console.SetCursorPosition(12, 4);
                        Console.WriteLine("Great,Your Changes were successful");
                        Console.WriteLine("If you want to see the corrected offer, click enter");
                    }
                    else
                    {
                        Console.WriteLine("Brak danych.");
                    }


                }
                catch (Exception exx)
                {
                    Console.WriteLine($"Somthing went wrong: {exx}");
                }
                finally
                {


                    conn.Close();

                }

            }




            Console.ReadKey();
        }


        static void PriceEdit(int OfferID)
        {
            Console.WriteLine("Price Editor:");
            Console.WriteLine();

            Console.WriteLine();
            Console.Write("Insert new Price ");
            string NewP = Console.ReadLine();
            int NewPrice = int.Parse(NewP);
            EditData(NewPrice, OfferID);

            static void EditData(int NewPrice, int OfferID)
            {
                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                MySqlConnection conn = new MySqlConnection(connStr);



                conn.Open();
                string sql = $"UPDATE Oferty_podrozy SET Cena = {NewPrice} WHERE ID_oferty_podrozy = {OfferID};";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();



                conn.Close();

                Console.SetCursorPosition(12, 4);
                Console.WriteLine("Great! Your Tour Adding was successful! ");
                Console.WriteLine("If you want to see your tour, go to the \"Searching Offer\" page and check it out.");

                Console.ReadKey();
            }

        }
        static void DepartureEdit(int OfferID)
        {
            Console.WriteLine("Departure Editor:");
            Console.WriteLine();

            Console.WriteLine();
            Console.Write("Insert new deprature date:");
            string NewDept = Console.ReadLine();

            EditData(NewDept, OfferID);

            static void EditData(string NewDept, int OfferID)
            {
                //your MySQL connection string
                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                MySqlConnection conn = new MySqlConnection(connStr);


                try
                {
                    conn.Open();

                    //SQL Query to execute
                    //selecting only first 10 rows for demo
                    string sql = $"UPDATE Oferty_podrozy SET Data_rozpoczenca = '{NewDept}' WHERE ID_oferty_podrozy = {OfferID};";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    int rowsAffected = cmd.ExecuteNonQuery();


                    if (rowsAffected > 0)
                    {
                        Console.SetCursorPosition(12, 4);
                        Console.WriteLine("Great,Your Changes were successful");
                        Console.WriteLine("If you want to see the corrected offer, click enter");
                    }
                    else
                    {
                        Console.WriteLine("Brak danych.");
                    }


                }
                catch (Exception exx)
                {
                    Console.WriteLine($"Somthing went wrong: {exx}");
                }
                finally
                {


                    conn.Close();

                }

            }


        }
        static void ArivedEdit(int OfferID)
        {
            Console.WriteLine("Return Editor:");
            Console.WriteLine();

            Console.WriteLine();
            Console.Write("Insert new return Date (Y-M-D):");
            string NewRet = Console.ReadLine();

            EditData(NewRet, OfferID);

            static void EditData(string NewRet, int OfferID)
            {
                //your MySQL connection string
                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                MySqlConnection conn = new MySqlConnection(connStr);


                try
                {
                    conn.Open();

                    //SQL Query to execute
                    //selecting only first 10 rows for demo
                    string sql = $"UPDATE Oferty_podrozy SET Data_zakonczenia = '{NewRet}' WHERE ID_oferty_podrozy = {OfferID};";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    int rowsAffected = cmd.ExecuteNonQuery();


                    if (rowsAffected > 0)
                    {
                        Console.SetCursorPosition(12, 4);
                        Console.WriteLine("Great,Your Changes were successful");
                        Console.WriteLine("If you want to see the corrected offer, click enter");
                    }
                    else
                    {
                        Console.WriteLine("Brak danych.");
                    }


                }
                catch (Exception exx)
                {
                    Console.WriteLine($"Somthing went wrong: {exx}");
                }
                finally
                {


                    conn.Close();

                }

            }


        }
        static void DescryptionEdit(int OfferID)
        {
            Console.WriteLine("Descryption Editor:");
            Console.WriteLine();

            Console.WriteLine();
            Console.Write("Insert new descryption:");
            string NewDesc = Console.ReadLine();

            EditData(NewDesc, OfferID);

            static void EditData(string NewDesc, int OfferID)
            {
                //your MySQL connection string
                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                MySqlConnection conn = new MySqlConnection(connStr);


                try
                {
                    conn.Open();

                    //SQL Query to execute
                    //selecting only first 10 rows for demo
                    string sql = $"UPDATE Oferty_podrozy SET Opis = '{NewDesc}' WHERE ID_oferty_podrozy = {OfferID};";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    int rowsAffected = cmd.ExecuteNonQuery();


                    if (rowsAffected > 0)
                    {
                        Console.SetCursorPosition(12, 4);
                        Console.WriteLine("Great,Your Changes were successful");
                        Console.WriteLine("If you want to see the corrected offer, click enter");
                    }
                    else
                    {
                        Console.WriteLine("Brak danych.");
                    }


                }
                catch (Exception exx)
                {
                    Console.WriteLine($"Somthing went wrong: {exx}");
                }
                finally
                {


                    conn.Close();

                }

            }


        }
        static void PlaceEdit(int OfferID)
        {
            Console.WriteLine("Accessible place Editor:");
            Console.WriteLine();

            Console.WriteLine();
            Console.Write("Insert new namber of places:");
            string NewP = Console.ReadLine();
            int NewPlace = int.Parse(NewP);
            EditData(NewPlace, OfferID);


            static void EditData(int NewPlace, int OfferID)
            {
                //your MySQL connection string
                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                MySqlConnection conn = new MySqlConnection(connStr);


                try
                {
                    conn.Open();

                    //SQL Query to execute
                    //selecting only first 10 rows for demo
                    string sql = $"UPDATE Oferty_podrozy SET Dostepne_miejsca = {NewPlace} WHERE ID_oferty_podrozy = {OfferID};";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    int rowsAffected = cmd.ExecuteNonQuery();


                    if (rowsAffected > 0)
                    {
                        Console.SetCursorPosition(12, 4);
                        Console.WriteLine("Great,Your Changes were successful");
                        Console.WriteLine("If you want to see the corrected offer, click enter");
                    }
                    else
                    {
                        Console.WriteLine("Brak danych.");
                    }


                }
                catch (Exception exx)
                {
                    Console.WriteLine($"Somthing went wrong: {exx}");
                }
                finally
                {


                    conn.Close();

                }


                Console.ReadKey();
            }
        }

        static void Delate(Home home)
        {
            Console.WriteLine("Delate offer:");
            Console.WriteLine();
            Console.WriteLine("If you want delate either offer, select suitable offer.");
            GetData(home);
            static void GetData(Home home)
            {
                //your MySQL connection string
                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                MySqlConnection conn = new MySqlConnection(connStr);



                conn.Open();

                //SQL Query to execute
                //selecting only first 10 rows for demo
                string sql = "SELECT * FROM Oferty_podrozy ";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();


                Console.WriteLine();
                //read the data
                while (rdr.Read())
                {


                    Console.WriteLine("ID: " + rdr[0] + " -- " + rdr[1] + " Euro" + " -- " + rdr[2] + " -- \n" + rdr[3] + " Euro" + " -- " + rdr[4] + " -- " + rdr[5]);
                    Console.WriteLine();
                }

                rdr.Close();


                conn.Close();
            }
            Console.Write("Insert the ID of the offer you want to delete: ");
            if (int.TryParse(Console.ReadLine(), out int offerID))
            {
                DelateData(offerID);
            }
            else
            {
                Console.WriteLine("Invalid offer ID.");
            }
            Console.ReadKey();

            DelateData(OfferID);

            static void DelateData(int offerID)
            {
                Console.WriteLine();
                string connectionString = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                // SQL query for deleting data
                string query = $"DELETE FROM Oferty_podrozy WHERE Id_oferty_podrozy ={offerID};";

                // Create SqlConnection
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // Create SqlCommand
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        try
                        {
                            // Open connection
                            connection.Open();

                            // ExecuteNonQuery for executing DELETE query
                            int rowsAffected = command.ExecuteNonQuery();

                            // Check if any rows are affected
                            if (rowsAffected > 0)
                            {
                                Console.WriteLine("Data deleted successfully.");
                            }
                            else
                            {
                                Console.WriteLine("No data deleted.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                    }
                }
                Console.ReadKey();

            }

        }


    }

}















