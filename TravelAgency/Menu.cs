using System;
namespace TravelAgency
{
	public abstract class Menu 
	{
        static string[] positionMenu ={"[] Searching offer","[] Add New Offer","[] Edit Offer",
        "[] Delete Offer","[] Ticket reservation","[] Payments"
        ,"End"};


        static int activeMenuPosition = 0;



        public virtual void StartMenu(Home home )
        {


            Console.CursorVisible = false;
            while (true)
            {


                ShowMenu();
                ChooseOption();
                GetOption(home);
            }

        }

        public virtual void ShowMenu()
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
        public virtual void ChooseOption()
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


        public virtual void GetOption(Home home)
        {


            switch (activeMenuPosition)
            {
                case 0: Console.Clear(); OptionUnderConstruction();  break;
                case 1: Console.Clear(); OptionUnderConstruction();  break;
                case 2: Console.Clear(); OptionUnderConstruction(); break;
                case 3: Console.Clear(); OptionUnderConstruction(); break;
                case 4: Console.Clear(); OptionUnderConstruction(); break;
                case 5: Console.Clear(); OptionUnderConstruction(); break;
                case 6: Environment.Exit(0); break;

            }
        }



        public virtual void OptionUnderConstruction()
        {
            Console.SetCursorPosition(12, 4);
            Console.Write("We Work on this, Sorry :(");
            Console.ReadKey();
        }


    }
}

