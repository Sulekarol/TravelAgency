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
        static string[] positionMenu ={"[] Searching offer","[] Add New Offer","[] Edit Offer",
        "[] Delete Offer","[] Ticket reservation"
        ,"End"};


        static int activeMenuPosition = 0;



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
                case 2: Console.Clear(); EditOffer(); break;
                case 3: Console.Clear(); OptionUnderConstruction(); break;
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
            

            else if(date> DateTime.Now)
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

        static void EditOffer()
        {
            Console.WriteLine("Edit offer:");
            Console.WriteLine();
            Console.ReadKey();
            Console.WriteLine("If you wont ");


        }
    }
}






