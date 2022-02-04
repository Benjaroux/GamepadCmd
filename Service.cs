using System;
using System.Linq;
using System.Threading;

namespace GamepadCmd
{
    internal class Service
    {
        private Macro[] macros;

        public Service(string filePath)
        {
            // TODO use filepath parameter and deserialize it

            macros = new Macro[] {
                new Macro("Start,Back", 3000, "taskkill /F /IM Cemu.exe")
            };
        }

        public void Run()
        {
            const int SLEEP_MS = 100;

            Gamepad gamepad = new Gamepad();
            bool wasConnected = true;
            Macro.ButtonFlags pressedButton = 0;
            Macro.ButtonFlags pressedButtonPrev = 0;
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
                        if (pressedButton != Macro.ButtonFlags.None)
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
                        foreach (Macro macro in macros.Where(x => x.IsMatched(pressedButton, pressedButtonDuration)))
                        {
                            Console.WriteLine(macro.ToString());
                            macro.DoCommand();
                        }
                    }
                }
            }
        }
    }
}
