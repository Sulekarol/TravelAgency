using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TravelAgency
{
    class Menu : Offers
    {
        static string[] positionMenu ={"[] Last minute","[] Special ForU",
        "[] Regular","[] Ours Guids",
        "[] Log In","End"};

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
                Console.WriteLine(@"
                 _____                    _       _____ _           _   _             
                |_   _| __ __ ___   _____| | ___ |  ___(_)_  ____ _| |_(_) ___  _ __  
                  | || '__/ _` \ \ / / _ \ |/ _ \| |_  | \ \/ / _` | __| |/ _ \| '_ \ 
                  | || | | (_| |\ V /  __/ | (_) |  _| | |>  < (_| | |_| | (_) | | | |
                  |_||_|  \__,_| \_/ \___|_|\___/|_|   |_/_/\_\__,_|\__|_|\___/|_| |_|

                    ");
                Console.WriteLine("Welcome to Travelofixation!");
                Console.WriteLine("The best page where you can find the tours to the places which can you even imagine!");


            

            Console.WriteLine();
            Console.WriteLine("Special Offer For U!");
            Offers offers = new Offers();
            offers.GetOffer();
          
          
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
                case 1: Console.Clear(); OptionUnderConstruction(); break;
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



    }
}




