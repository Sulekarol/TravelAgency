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
    public class Home : IDBConnect, ILogo, ISecretP
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

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;           
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.DarkGreen;           
            Console.WriteLine();
            ILogo.ShowLogo();
            IDBConnect.GetData();



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
            
            ConsoleKeyInfo key;

            Console.Write("If you want to add a new trip, enter the password to the company database.\n" +
                "Enter Password:");

            string password = ISecretP.GetMaskedInput();
           

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
            InsertData(password, destynation, dateOfDept, arrived, price, tourDescription, places);
            Console.ReadKey();

        }

        

 

        static void InsertData(string password ,string destynation, DateTime dateOfDept, string arrived, string price, string tourDescription, string places)
        {
            string connStr = $"server=localhost;user=root;database=biuropodrozy;port=3306;password={password}";

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                
                conn.Open();

               

                string sql = "INSERT INTO Oferty_podrozy (id,destynation, dateOfDept, arrived, price, tourDescription, places) VALUES (@id,@Cel_podrozy,@Data_rozpoczecia,@Data_zakonczenia,@Cena,@Opis,@Dostepne_miejsca)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id_oferty_podrozy", 7);
                cmd.Parameters.AddWithValue("@Cel_podrozy", destynation);
                cmd.Parameters.AddWithValue("@Data_rozpoczecia", dateOfDept);
                cmd.Parameters.AddWithValue("@Data_zakonczenia", arrived);
                cmd.Parameters.AddWithValue("@Cena", price);
                cmd.Parameters.AddWithValue("@Opis", tourDescription);
                cmd.Parameters.AddWithValue("@Dostepne_miejsca", places);
                cmd.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                if (true)
                {
                    Console.Clear();
                    Console.WriteLine($"Something went wrong: {err.Message}");
                }
                else
                {
                    Console.Clear();
                    Console.SetCursorPosition(12, 4);
                    Console.WriteLine("Great! Your Tour Adding was successful! ");
                    Console.WriteLine("If you want to see your tour, go to the \"Searching Offer\" page and check it out.");
                }
            }
            finally
            {
                conn.Close();
               
            }
            

       
        }


        static string[] menuOption = { "[] When", "[] Where", "[] Price", "Back" };
        static int activeMenuOption = 0;


        static void SerchingOffer(Home home)
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
                case 0: Console.Clear(); OptionUnderConstruction(); break;
                case 1: Console.Clear(); OptionUnderConstruction(); break;
                case 3: Console.Clear(); home.StartMenu(home); break;

            }
        }





    }
}




