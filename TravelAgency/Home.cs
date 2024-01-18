using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency
{
    interface ILogo 
    {


        void ShowLogo()
        {
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


        }

        
            
    }      
}

