using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace TravelAgency
{
    public class Home : IDBConnect, ILogo
    {
        static string[] positionMenu ={"[] Searching offer","[] Add New Offer","[] Edit Offer",
        "[] Delete Offer","[] Ticket reservation","[] Payments"
        ,"End"};

        static int activeMenuPosition = 0;

        

        public void StartMenu(Home home)
        {


            Console.CursorVisible = false;
            while (true)
            {


                ShowMenu();
                ChooseOption();
                GetOption(home);
            }
           
        }

        public static void ShowMenu()
        {


            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
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
        public static void ChooseOption()
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


        public static void GetOption(Home home)
        {
            

            switch (activeMenuPosition)
            {
                case 0: Console.Clear(); SerchingOffer(home); break;
                case 1: Console.Clear(); AddNewData(home); break;
                case 2: Console.Clear(); OptionUnderConstruction(); break;
                case 3: Console.Clear(); OptionUnderConstruction(); break;
                case 4: Console.Clear(); OptionUnderConstruction(); break;
                case 5: Console.Clear(); OptionUnderConstruction(); break;
                case 6: Environment.Exit(0); break;

            }
        }



        public static void OptionUnderConstruction()
        {
            Console.SetCursorPosition(12, 4);
            Console.Write("We Work on this, Sorry :(");
            Console.ReadKey();
        }

        static void AddNewData(Home home)
        {
            Console.WriteLine("Add New Offer:");
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
            Console.Write("When? (departure - Y-M-D): ");
            string departure = Console.ReadLine();
            
            DateTime dateOfDept;
            
            if (!DateTime.TryParse(departure, out dateOfDept))
            {
                Console.WriteLine("Invalid date format. Please enter the date in the correct format (Y-M-D).");
                AddNewData(home);
                Console.Clear();
                return;

            }
            
            if (DateTime.Now < dateOfDept)
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
            InsertData(destynation, dateOfDept, arrived, price, tourDescription, places);
            Console.ReadKey();

        }


        static void InsertData(string destynation, DateTime dateOfDept, string arrived, string price, string tourDescription, string places)
        {
            string connStr = "server=localhost;user=root;database=biuropodrozy;port=3306;password=Nolypiok208";

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("");
                conn.Open();

                // SQL Query to execute
                // insert Query
                // we are inserting destynation, departure, arrived, price, tourDescription, places columns data

                string sql = "INSERT INTO Oferty_podrozy (Id_rez,destynation, departure, arrived, price, tourDescription, places) VALUES (@destynation, @departure, @arrived, @price, @tourDescription, @places)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", 7);
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
            Console.WriteLine("If you want to see your tour, go to the \"Searching Offer\" page and check it out.");

            Console.ReadKey();
        }


        static string[] menuOption = { "[] When", "[] Where", "[] Price", "Back" };
        static int activeMenuOption = 0;


        static void SerchingOffer(Home home)
        {
            Console.WriteLine("Choose an option which will research for you the data");
            Console.SetCursorPosition(12, 4);
            string[] menuOption = { "[] When", "[] Where", "[] Price", "Back" };
            Console.WriteLine("Serching Offer:");
            Console.SetCursorPosition(12, 4);
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
                case 0: Console.Clear(); OptionUnderConstruction(); break;
                case 1: Console.Clear(); OptionUnderConstruction(); break;
                case 3: Console.Clear(); home.StartMenu(home); break;

            }
        }





    }
}




