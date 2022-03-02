using System;

namespace GamepadCmd
{
    internal class Program
    {
        /// <summary>
        /// Entry point of the program
        /// 
        /// Usage : 
        /// GamepadCmd.exe "[parameter file path]"
        /// 
        /// Parameter file path : 
        /// 1 parameter / line, a parameter is represented as follows :
        /// [Pattern 1];[Duration 1];[Command 1]
        /// [Pattern 2];[Duration 2];[Command 2]
        /// ...
        /// 
        /// With :
        ///  - Pattern : button or combination of buttons pressed at the same time required for matching the pattern
        ///              buttons must be separated by comma
        ///  - Duration : Pressed buttons duration (in milliseconds) required for matching the pattern
        ///  - Command : Command to execute for this pattern
        ///  
        /// Examples :
        /// A;1000;notepad.exe ==> Press "A" for 1000 ms will execute the command "calc.exe"
        /// Start,Back;3000;taskkill /F /IM calc.exe ==> Press "Start" + "Back" for 3000 ms will execute the command "taskkill /F /IM calc.exe"
        /// 
        /// </summary>
        /// <param name="args">Arguments with the parameter file path as first argument</param>
        static void Main(string[] args)
        {
            try
            {
                string filePath = args[0];
                Service service = new Service(filePath);

                Console.WriteLine("Using file : " + filePath);
                Console.WriteLine(service.ToString());
                Console.WriteLine("Run service...");
                service.Run();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            finally
            {
                Console.Write("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
