using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MissCanApp
{

    #region Program CLASS
    //Program - Is the main entry point into the CannMissApp application.
    //Essentially all this class does is call the getSolutionStates method 
    //of the SolutionProvider, and then prints the solutions (If there are any)
    static class Program
    {
        //instance fields
        public static bool CanOutnumberMiss = false;
        public static int boatsize = 2;
          
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            RunSearch();
            Console.ReadLine();
        }
        
        //This method print some static text, then prints whatever the user
        //has asked for in terms of command line arguments then proceeds
        //to create a SolutionProvider class.
        //
        //Once a SolutionProvider is created this class asks it to provide all the
        //optimal solutions. If there are any solutions returned these are then printed
        private static void RunSearch() 
        {
            
            //Print details
            Console.WriteLine("=================================================");
            Console.WriteLine("  THE SEARCH HAS BEEN SETUP WITH THE             ");
            Console.WriteLine("  FOLLOWING OPTIONS                              ");
            Console.WriteLine("=================================================");
            //Examine command line arguments to see what has been asked for
            if (CanOutnumberMiss) 
            {
                Console.WriteLine("1. Cannibals must be equal or greater than Missionaries");
            }
            else 
            {
                Console.WriteLine("1. Missionaries must be equal or greater than Cannibals");
            }
            Console.WriteLine("\r\n");
            Console.WriteLine("As this is a breadth 1st search the higher up the");
            Console.WriteLine("search tree the solutions are, the cheaper they will");
            Console.WriteLine("be. So the 1st solutions found will be the optimal");
            Console.WriteLine("ones. The most optimal solutions are shown below\r\n");
            // Create a SolutionProvider, Ask it for the solutions, then print them
            SolutionProvider S = new SolutionProvider();
            ArrayList Solution = S.getSolutionStates(new State("Root", 3, 3, false,1),
                                              new State("Goal", 0, 0, true,999999));
            printSolutions(Solution);
        }

        //This method prints the Solutions returned by the SolutionProvider object.
        //However, there may not actually be any solutions to print.
        //
        //Once a SolutionProvider is created this class asks it to provide all the
        //solutions. If there are any solutions returned these are then printed
        //
        //param : Solution the Solutions returned by the SolutionProvider object.
        private static void printSolutions(ArrayList Solution) 
        {
            //Are there any solutions
            if (Solution.Count == 0)
            {
                Console.WriteLine("\n\nNO SOLUTIONS HAVE BEEN FOUND\r\n");
            }
            else
            {
                int Solfound = 1;
                //show the Solutions
                for (int i = 0; i < Solution.Count; i++)
                {
                  State s = (State)Solution[i];
                  Console.WriteLine("=====FOUND SOLUTION [" + Solfound++ + "]=====\r\n");
                  Console.WriteLine("This solution was found at level [" + s.getStateLevel() + "]\r\n");
                  s.Print();
                  Console.WriteLine("\r\n");
                }
            }
        }

    } // End of Program Class
    #endregion
}