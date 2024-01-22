using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TravelAgency
{
    class Menu : DBConnect, ILogo
    {
        static string[] positionMenu ={"[] Searching offer","[] Add New Offer","[] Edit Offer",
        "[] Delete Offer","[] Ticket reservation","[] Payments"
        ,"End"};

        static int activeMenuPosition = 0;


        public static void StartMenu()
        {

            Console.CursorVisible = false;
            while (true)
            {


                ShowMenu();
                ChooseOption();
                GetOption();
            }
        }

        static void ShowMenu()
        {


            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            ILogo.ShowLogo();
            Console.WriteLine();
            Console.WriteLine("Special Offer !");
            Console.WriteLine();
            DBConnect dBConnect = new DBConnect();
            dBConnect.getData();
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
        static void ChooseOption()
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

        static void GetOption()
        {
            switch (activeMenuPosition)
            {
                case 0: Console.Clear(); OptionUnderConstruction(); break;
                case 1: Console.Clear(); AddNewData(); break;
                case 2: Console.Clear(); OptionUnderConstruction(); break;
                case 3: Console.Clear(); OptionUnderConstruction(); break;
                case 4: Console.Clear(); OptionUnderConstruction(); break;
                case 5: Environment.Exit(0); break;

            }
        }

        static void OptionUnderConstruction()
        {
            Console.SetCursorPosition(12, 4);
            Console.Write("We Work on this, Sorry :(");
            Console.ReadKey();
        }
        string destynation;
        string departure;
        string arived;
        string price;
        string tourDescription;
        string places;

        static void AddNewData()
        {



            Console.SetCursorPosition(12, 4);
            Console.Write("Add new offer !");
            Console.WriteLine();
            Console.Write("Where ? (City/Country) :");
            string destynation = Console.ReadLine();

            if (destynation.Length <= 3)
            {
                Console.WriteLine("Something went wrong, name of your City and contry is to short try again :) ");
                Console.Clear();
                AddNewData();

            }

            else
            {

            }
            Console.WriteLine();
            Console.Write("When? (departure - Y-M-D): ");
            string departure = Console.ReadLine();
            DateTime dateOfDept;

            dateOfDept = DateTime.Parse(departure);


            if (DateTime.Now < dateOfDept)
            {
                Console.WriteLine("Looks great, keep moving");

            }
            else if (DateTime.Now >= dateOfDept)
            {
                Console.WriteLine("Something went wrong, your date of departure is from the past  try again :) ");

            }


            Console.Write("When? (arived - Y-M-D): ");
            string arived = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Price:");
            string price = Console.ReadLine();
            Console.WriteLine();

            Console.Write("Write few words about this tour: ");
            string tourDescription = Console.ReadLine();
            Console.WriteLine();

            Console.Write("How many places are avilable? :");
            string places = Console.ReadLine();






            //string connStr = "server=localhost;user=root;database=biuropodrozy;port=3306;password=Nolypiok208";

            //MySqlConnection conn = new MySqlConnection(connStr);
            //try
            //{

            //    conn.Open();

            //    //SQL Query to execute
            //    //insert Query
            //    // we are inserting actor_id, first_name, last_name, last_updated columns data

            //    string sql = $"INSERT INTO Oferty_podrozy VALUES (7,'{destynation}',{departure},{arived},{price},'{tourDescription}',{places}";
            //    MySqlCommand cmd = new MySqlCommand(sql, conn);
            //    cmd.ExecuteNonQuery();


            //}
            //catch (Exception err)
            //{
            //    Console.WriteLine(err.ToString());
            //}

            //conn.Close();
            //Console.WriteLine("Great, Your tour is added to our data Base ! ");
            //Console.ReadKey();

            Console.SetCursorPosition(12, 4);
            Console.WriteLine(" Grea! Youur Tour Adding are succesfull! ");
            Console.SetCursorPosition(12, 4);
            Console.WriteLine("If You want to saw your tour, go to \"Searching Offer\" page and check out. ");


        }


    }

}




