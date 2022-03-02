using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace GamepadCmd
{
    /// <summary>
    /// Manage gamepad buttons detection and test if the buttons 
    /// pressed by the user match with one of a parameter defined in the parameters file
    /// </summary>
    internal class Service
    {
        private Parameter[] macros;

        /// <summary>
        /// Constructor, initialize service with parameters 
        /// </summary>
        /// <param name="filePath">Parameters file path</param>
        public Service(string filePath)
        {
            List<Parameter> lstMacros = new List<Parameter>();

            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(';');
                    lstMacros.Add(new Parameter(line[0], int.Parse(line[1]), line[2]));
                }
            }

            macros = lstMacros.ToArray();
        }

        /// <summary>
        /// Run gamepad buttons detection and detect if one of the pattern defined in parameters is matching
        /// In case of matching, run the corresponding command and continue the detection
        /// </summary>
        public void Run()
        {
            const int SLEEP_MS = 100;

            Gamepad gamepad = new Gamepad();
            bool wasConnected = true;
            Parameter.ButtonFlags pressedButton = 0;
            Parameter.ButtonFlags pressedButtonPrev = 0;
            int pressedButtonDuration = 0;

            while (true)
            {
                Thread.Sleep(SLEEP_MS);

                if (!gamepad.Refresh())
                {
                    if (wasConnected)
                    {
                        wasConnected = false;
                        Console.WriteLine("Please connect a controller.");
                    }
                }
                else
                {
                    if (!wasConnected)
                    {
                        wasConnected = true;
                        Console.WriteLine("Controller connected on port " + gamepad.GetPort());
                    }

                    pressedButtonPrev = pressedButton;
                    pressedButton = gamepad.GetPressedButton();

                    // New button or new button combinaison pressed
                    if (pressedButton != pressedButtonPrev)
                    {
                        if (pressedButton != Parameter.ButtonFlags.None)
                        {
                            Console.WriteLine("Button pressed : " + pressedButton);
                        }

                        // Re-init duration counter
                        pressedButtonDuration = SLEEP_MS;
                    }
                    else
                    {
                        // Increment counter
                        pressedButtonDuration += SLEEP_MS;

                        // Check for matching macros
                        foreach (Parameter macro in macros.Where(x => x.IsMatched(pressedButton, pressedButtonDuration)))
                        {
                            Console.WriteLine(macro.ToString());
                            macro.DoCommand();
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Parameter macro in macros)
            {
                sb.AppendLine(macro.ToString());
            }

            return sb.ToString();
        }
    }
}
