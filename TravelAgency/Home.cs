using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TravelAgency
{
    interface ILogo 
    {


        public static void ShowLogo()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(@"
                 _____                    _       _____ _           _   _             
                |_   _| __ __ ___   _____| | ___ |  ___(_)_  ____ _| |_(_) ___  _ __  
                  | || '__/ _` \ \ / / _ \ |/ _ \| |_  | \ \/ / _` | __| |/ _ \| '_ \ 
                  | || | | (_| |\ V /  __/ | (_) |  _| | |>  < (_| | |_| | (_) | | | |
                  |_||_|  \__,_| \_/ \___|_|\___/|_|   |_/_/\_\__,_|\__|_|\___/|_| |_|

                    ");
            Console.WriteLine("Welcome to Travelofixation office menagement program !");
            
        }
        
      
    }      
}

