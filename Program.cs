using System;

namespace GamepadCmd
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Service service = new Service(string.Empty);

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
