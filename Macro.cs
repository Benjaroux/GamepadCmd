using System;
using System.Diagnostics;

namespace GamepadCmd
{
    internal class Macro
    {
		public ButtonFlags pattern;
		public int patternDuration;
		public string command;

        public Macro(string _patternStr, int _patternDuration, string _command)
        {
            pattern = (ButtonFlags)Enum.Parse(typeof(ButtonFlags), _patternStr);
            patternDuration = _patternDuration;
            command = _command;
        }

        public bool IsMatched(ButtonFlags button, int duration)
        {
            return button == pattern && 
                   duration == patternDuration;
        }

        public void DoCommand()
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/C " + command,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                }
            };

            proc.Start();
        }

        public override string ToString() => string.Format("[{0} | {1} ms] Execute command : {2}", pattern, patternDuration, command);

        [Flags]
        public enum ButtonFlags : ushort
        {
            None = 0x0000,
            Up = 0x0001,
            Down = 0x0002,
            Left = 0x0004,
            Right = 0x0008,
            Start = 0x0010,
            Back = 0x0020,
            LS = 0x0040,
            RS = 0x0080,
            LB = 0x0100,
            RB = 0x0200,
            A = 0x1000,
            B = 0x2000,
            X = 0x4000,
            Y = 0x8000,
        };
    }
}
