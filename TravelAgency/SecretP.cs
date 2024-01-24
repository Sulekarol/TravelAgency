using System;
using static Mysqlx.Expect.Open.Types.Condition.Types;

namespace TravelAgency
{
    interface ISecretP
    {
        static string GetMaskedInput()
        {
            string input = "";
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);


                if (key.Key != ConsoleKey.Enter)
                {
                    if (key.Key == ConsoleKey.Backspace)
                    {

                        if (input.Length > 0)
                        {
                            input = input.Substring(0, input.Length - 1);

                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            Console.Write(" ");
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        }
                    }
                    else
                    {

                        input += key.KeyChar;


                        Console.Write("*");
                    }
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();

            return input;
        }
    }
}