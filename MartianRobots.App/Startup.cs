using System;
using MartianRobots.App.Extensions;
using MartianRobots.App.Utils;
using Console = System.Console;


namespace MartianRobots
{
    class Startup
    {
        /// <summary>
        /// Main point of application
        /// </summary>
        public static void Run()
        {
            try
            {
                // Read file instructions, parse incomming data to dictionary
                // instructions and create report
                Processing.ReadFileAndConfigure();

                Console.WriteLine($"Planet size:\n{Processing.Height}x{Processing.Width}\n");
                Console.WriteLine("Instructions:");
                // Deploy instructions for Robot moving
                Processing.Deploy();
                // Print report
                Console.WriteLine($"\nOutput:\n{Processing.Control?.GetStatusReport()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadKey(true);
            }
        }
    }
}
