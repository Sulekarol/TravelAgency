using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency
{
    static class Menu
    {
        static string[] positionMenu ={"Option 1","Option 2",
        "Option 3","Option 4",
        "Option 5","End"};

        static int activeMenuPosition = 0;

        public static void StartMenu()
        {
            Console.Title = "Travel Agency";
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
            Console.WriteLine(">>> Travel Agency<<<");
            Console.WriteLine();
            for (int i = 0; i < positionMenu.Length; i++)
            {
                if (i == activeMenuPosition)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("{0,-35}", positionMenu[i]);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;

                }
                else
                {
                    Console.WriteLine(positionMenu[i]);
                }
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
            Console.Write("We Work on this, Sory :(");
            Console.ReadKey();
        }



    }
}




