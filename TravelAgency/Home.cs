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
                case 4: Console.Clear(); TicketReservation(home); break;
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


            Console.Write("Where ? (City/Country) : ");
            string destynation = Console.ReadLine();

            if (destynation.Length < 3)
            {
                Console.WriteLine("Something went wrong, the name of your City or country is too short. Try again :) ");
                Console.Clear();
                return;
            }

            Console.WriteLine();
            Console.Write("When? (departure- Y-M-D): ");
            string departur = Console.ReadLine();

            DateTime date;

            if (!DateTime.TryParse(departur, out date))
            {
                Console.WriteLine("Invalid date format. Please enter the date in the correct format (Y-M-D).");
                Console.WriteLine();
                Console.Clear();
                AddNewData(home);
                return;
            }

            if (date <= DateTime.Now)
            {
                Console.WriteLine("Invalid date. Please enter a date in the future.");
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
                        break; 
                    }
                }
            }


            Console.ReadKey();



        }

        // INSER DATA FOR ADDING PAGE 

        static void InsertData(string password, string destynation, string departure, string arrived, string price, string tourDescription, string places)
        {
            


            string connStr1 = $"server=localhost;user=root;database=biuropodrozy;port=3306;password={password}";

            MySqlConnection conn1 = new MySqlConnection(connStr1);
            try
            {

                conn1.Open();



                string sql = $"INSERT INTO Oferty_podrozy (Destination, Price,Departure, Arrived,  Description, Dostepne_miejsca) VALUES ('{destynation}',{price},'{departure}','{arrived}','{tourDescription}',{places})";
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
            Console.Write("Enter departure data (YYYY - MM- DD):");
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

                   
                    string sql = $"SELECT * FROM Oferty_podrozy WHERE Departure >= '{from}';";
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
            Console.Write("Enter place where you wont to go(City):");

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

                   
                    string sql = $"SELECT *FROM Oferty_podrozy WHERE Destination = '{destynation}';";
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

                    
                    string sql = $"SELECT *FROM Oferty_podrozy WHERE Price >= {amount};";
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


                    Console.WriteLine("ID: " + rdr[0] + " -- " + rdr[1] + " Euro" + " -- " + rdr[2] + " -- \n" + rdr[3] + " Euro" + " -- " + rdr[4] + " -- " + rdr[5] + " -- " + "Avilable places: " + rdr[6]);
                    Console.WriteLine();
                }

                rdr.Close();


                conn.Close();
            }
            Console.Write("Insert ID Ofer wich you want to change:");


            if (int.TryParse(Console.ReadLine(), out int offerID))
            {
                GetData1(offerID);
            }
            else
            {
                Console.WriteLine("Invalid offer ID.");
            }






            static void GetData1(int OfferID)
            {
                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                MySqlConnection conn = new MySqlConnection(connStr);



                conn.Open();

               
                string sql = $"SELECT * FROM Oferty_podrozy WHERE Id_oferty_podrozy = {OfferID}";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();


                Console.WriteLine();
               
                while (rdr.Read())
                {
                    Console.Clear();
                    rdr.Read();
                    Console.WriteLine("ID: " + rdr[0] + " -- " + "\nDestynation: " + rdr[1] + "\nPrice: " + rdr[2] + " Euro" + "\nDeparture: " + rdr[3] + "\nArrived: " + rdr[4] + "\nDescription: " + rdr[5] + "\nAvilable places: " + rdr[6]);
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
                        string ofertaInfo = $"ID: {rdr[0]} \nDestynation:  {rdr[1]} \nPrice:  {rdr[2]}  Euro \nDeparture: {rdr[3]} \nArrived: {rdr[4]} \nDescription: {rdr[5]} \nAvilable places: {rdr[6]}";
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
                    string sql = $"UPDATE Oferty_podrozy SET Destination = '{NewDest}' WHERE ID_oferty_podrozy = {OfferID};";
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
                string sql = $"UPDATE Oferty_podrozy SET Price = {NewPrice} WHERE ID_oferty_podrozy = {OfferID};";
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
                    string sql = $"UPDATE Oferty_podrozy SET Departure = '{NewDept}' WHERE ID_oferty_podrozy = {OfferID};";
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
                    string sql = $"UPDATE Oferty_podrozy SET Arrived = '{NewRet}' WHERE ID_oferty_podrozy = {OfferID};";
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
                    string sql = $"UPDATE Oferty_podrozy SET Description = '{NewDesc}' WHERE ID_oferty_podrozy = {OfferID};";
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
               
                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                MySqlConnection conn = new MySqlConnection(connStr);


                try
                {
                    conn.Open();

                  
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


                string query = $"DELETE FROM Oferty_podrozy WHERE Id_oferty_podrozy ={offerID};";


                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        try
                        {

                            connection.Open();


                            int rowsAffected = command.ExecuteNonQuery();


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









        static int nextId = 0;
        static string Depart;
        static string Email;

        static void TicketReservation(Home home)
        {
            Console.WriteLine("Ticket Reservation:");
            Console.WriteLine();
            Console.WriteLine(" For which tour are you wont to make reservation? ");
            GetData(home);
            static void GetData(Home home)
            {

                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                MySqlConnection conn = new MySqlConnection(connStr);



                conn.Open();


                string sql = "SELECT * FROM Oferty_podrozy ";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();


                Console.WriteLine();

                while (rdr.Read())
                {


                    Console.WriteLine("ID: " + rdr[0] + " -- " + rdr[1] + " Euro" + " -- " + rdr[2] + " -- \n" + rdr[3] + " Euro" + " -- " + rdr[4] + " -- " + rdr[5] + " -- " + "Avilable places: " + rdr[6]);
                    Console.WriteLine();
                }

                rdr.Close();


                conn.Close();
            }

            Console.Write("Insert ID Offer which you want to make a reservation:");
            string offerIDString = Console.ReadLine();


            if (int.TryParse(offerIDString, out int offerID))
            {
                GetData2(offerID, Depart, Email);
            }
            else
            {
                Console.WriteLine("Invalid offer ID entered.");
            }

            static void GetData2(int offerID, string Depart, string email)
            {
                string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";

                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();

                        string sql = "SELECT * FROM Oferty_podrozy WHERE Id_oferty_podrozy = @offerID";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@OfferID", offerID);

                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                Console.Clear();
                                string ofertaInfo = $"ID: {rdr[0]} \nDestination: {rdr[1]} \nPrice: {rdr[2]} Euro \nDeparture: {rdr.GetDateTime(3).ToString("yyyy-MM-dd")} \nArrived: {rdr[4]} \nDescription: {rdr[5]} \nAvailable places: {rdr[6]}";
                                Console.WriteLine(ofertaInfo);

                                Depart = rdr.GetDateTime(3).ToString("yyyy-MM-dd");
                                AddReservation(Depart, offerID, email);

                                Console.WriteLine();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }


                Console.WriteLine("Are you wont make reservation for exist client or for new klient ?");
                Console.WriteLine();
                Console.Write("Insert NEW for new or OLD for exist klient: ");
                string decide = Console.ReadLine();
                string NewOrOld = decide.ToUpper();

                if (NewOrOld == "NEW")
                {
                    AddClientToDatabase(Email, offerID);
                    Console.ReadKey();
                }
                else
                {
                    Console.Write("Email: ");
                    string Email = Console.ReadLine();
                    if (!IsValidEmail(Email))
                    {
                        Console.WriteLine("Invalid email format.");

                    }
                    Console.WriteLine();

                    if (ClientExists(Email))
                    {
                        Console.WriteLine("OK! We create a reservation for you :D");
                        Console.WriteLine();
                        AddReservation(Depart, offerID, Email);
                        Console.ReadKey();
                    }

                }
            }






            Console.ReadKey();
        }


        static bool IsValidEmail(string Email)
        {
            return !string.IsNullOrWhiteSpace(Email) && Email.Contains("@") && (Email.EndsWith(".pl") || Email.EndsWith(".com")); // Dodatkowy warunek sprawdzający końcówkę adresu email
        }

        static bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.Length == 9 && phoneNumber.All(char.IsDigit); // Warunek sprawdzający długość numeru telefonu oraz czy składa się tylko z cyfr
        }

        static bool IsValidDateOfBirth(string dateOfBirth)
        {
            DateTime dob;
            if (DateTime.TryParse(dateOfBirth, out dob))
            {
                return dob < DateTime.Now;
            }
            return false;
        }


        static bool IsAdult(string dateOfBirth)
        {
            DateTime dob;
            if (DateTime.TryParse(dateOfBirth, out dob))
            {
                return DateTime.Now.Subtract(dob).TotalDays / 365 >= 18;
            }
            return false;

        }



        static bool ClientExists(string Email)
        {
            string connStr = "server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";
            string query = "SELECT COUNT(*) FROM Klient WHERE Email = @Email";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", Email);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }




        public static void AddClientToDatabase(string Email, int offerID)
        {
            Console.WriteLine("Insert data of person which are you want make reservation: ");
            Console.WriteLine();

            Console.Write("Email: ");
            Email = Console.ReadLine();
            if (!IsValidEmail(Email))
            {
                Console.WriteLine("Invalid email format.");

            }
            Console.WriteLine();

            Console.Write("Name: ");
            string Name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(Name))
            {
                Console.WriteLine("Name cannot be empty.");

            }
            Console.WriteLine();

            Console.Write("Surname: ");
            string Surname = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(Surname))
            {
                Console.WriteLine("Surname cannot be empty.");

            }
            Console.WriteLine();

            Console.Write("Address: ");
            string Address = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(Address))
            {
                Console.WriteLine("Address cannot be empty.");

            }
            Console.WriteLine();

            Console.Write("Phone number: ");
            string PhoneNumber = Console.ReadLine();
            if (!IsValidPhoneNumber(PhoneNumber))
            {
                Console.WriteLine("Invalid phone number format.");

            }
            Console.WriteLine();

            Console.Write("Date of birth (YYYY-MM-DD): ");
            string DateOfBirth = Console.ReadLine();
            Console.WriteLine();


            if (!IsValidDateOfBirth(DateOfBirth))
            {
                Console.WriteLine("Invalid date of birth format.");

                if (!IsAdult(DateOfBirth))
                {
                    Console.WriteLine("Person must be at least 18 years old.");

                }
            }

            Console.WriteLine("All information entered correctly.");
            Console.WriteLine();


            Console.ReadKey();

            string connStr = "server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";
            string query = "INSERT INTO Klient (Imie, Nazwisko, Email, Adres, Nr_tel, Data_urodzenia) VALUES (@Name, @Surname, @Email, @Address, @PhoneNumber, @DateOfBirth)";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();




                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Surname", Surname);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                    cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);


                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Client added successfully.");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Failed to add client to the database.");
                        Console.WriteLine();
                    }
                }
            }
            AddReservation(Depart, offerID, Email);



        }


        public static void AddReservation(string Depart, int offerID, string Email)
        {
            string connStr1 = "server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";
            string query1 = "SELECT Id_klienta FROM Klient WHERE Email = @Email";

            try
            {
                using (MySqlConnection conn1 = new MySqlConnection(connStr1))
                {
                    conn1.Open();
                    using (MySqlCommand cmd1 = new MySqlCommand(query1, conn1))
                    {
                        cmd1.Parameters.AddWithValue("@Email", Email);

                        object result = cmd1.ExecuteScalar();
                        if (result != null)
                        {
                            int clientId = Convert.ToInt32(result);

                            string connStr = "server=localhost;user=root;database=biuropodrozy;port=3306;password=drzwi";
                            string query = "INSERT INTO Rezerwacja (Data_Rezerwacji, Data_Wyjazdu, Status_Wycieczki, Id_klienta, Id_oferty_podrozy, Id_platnosci, Id_przewodnika) VALUES (@Data_Rezerwacji, @Data_Wyjazdu, @Status_Wycieczki, @Id_klienta, @Id_oferty_podrozy, @Id_platnosci, @Id_przewodnika)";

                            using (MySqlConnection conn = new MySqlConnection(connStr))
                            {
                                conn.Open();
                                DateTime today = DateTime.Today;
                                string formattedDate = today.ToString("yyyy-MM-dd");

                                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                {
                                    cmd.Parameters.AddWithValue("@Data_Rezerwacji", formattedDate);
                                    cmd.Parameters.AddWithValue("@Data_Wyjazdu", Depart);
                                    cmd.Parameters.AddWithValue("@Status_Wycieczki", "Oczekujący");
                                    cmd.Parameters.AddWithValue("@Id_klienta", clientId);
                                    cmd.Parameters.AddWithValue("@Id_oferty_podrozy", offerID);
                                    cmd.Parameters.AddWithValue("@Id_platnosci", DBNull.Value);
                                    cmd.Parameters.AddWithValue("@Id_przewodnika", DBNull.Value);
                                    int rowsAffected = cmd.ExecuteNonQuery();

                                    if (rowsAffected > 0)
                                    {
                                        Console.WriteLine("Reservation added successfully.");
                                        Console.WriteLine();

                                        UpdateAvailablePlaces(conn, offerID);

                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to add reservation. No rows affected.");

                                    }
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding reservation: " + ex.Message);

            }
        }

        static void UpdateAvailablePlaces(MySqlConnection conn, int offerID)
        {
            try
            {
                string updateQuery = "UPDATE Oferty_podrozy SET Dostepne_miejsca = Dostepne_miejsca - 1 WHERE Id_oferty_podrozy = @OfferID";
                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@OfferID", offerID);
                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Updated available places for offer ID: " + offerID);
                    }
                    else
                    {
                        Console.WriteLine("No rows were affected while updating available places for offer ID: " + offerID);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating available places: " + ex.Message);
            }
        }



    }
}



















