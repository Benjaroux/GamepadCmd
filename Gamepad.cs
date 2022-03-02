namespace GamepadCmd
{
    /// <summary>
    /// Manage gamepad actions
    /// </summary>
    internal class Gamepad
    {
        private int cId;
        private XInputInterop.XINPUT_STATE state;

        /// <summary>
        /// Get port number for the currently connected gamepad
        /// </summary>
        /// <returns>Port number</returns>
        public int GetPort()
        {
            return cId + 1;
        }

        /// <summary>
        /// Check if a gamepad is connected
        /// </summary>
        /// <returns>true if connected else, return false</returns>
        public bool CheckConnection()
        {
            int controllerId = -1;

            for (int i = 0; i < XInputInterop.XUSER_MAX_COUNT && controllerId == -1; i++)
            {
                XInputInterop.XINPUT_STATE state = new XInputInterop.XINPUT_STATE();

                if (XInputInterop.XInputGetState((uint)i, ref state) == XInputInterop.ERROR_SUCCESS)
                {
                    controllerId = i;
                }
            }

            cId = controllerId;

            return controllerId != -1;
        }

        /// <summary>
        /// Get current gamepad state
        /// </summary>
        /// <returns></returns>
        public bool Refresh()
        {
            if (cId == -1)
            {
                CheckConnection();
            }

            if (cId != -1)
            {
                state = new XInputInterop.XINPUT_STATE();
                if (XInputInterop.XInputGetState((uint)cId, ref state) != XInputInterop.ERROR_SUCCESS)
                {
                    cId = -1;
                    return false;
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Test if the currently pressed button(s) match with the ID in parameter
        /// </summary>
        /// <param name="button">ID of pressed button(s)</param>
        /// <returns>true if match, else return false</returns>
        public bool IsPressed(Parameter.ButtonFlags button)
        {
            return ((Parameter.ButtonFlags)state.Gamepad.wButtons & button) != 0;
        }

        /// <summary>
        /// Get which button or combinaison of buttons are currently pressed
        /// </summary>
        /// <returns>ID of pressed button(s)</returns>
        public Parameter.ButtonFlags GetPressedButton()
        {
            return (Parameter.ButtonFlags)state.Gamepad.wButtons;
        }
    }
}
