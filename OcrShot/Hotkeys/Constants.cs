namespace OcrShot.Hotkeys
{
    public sealed class Constants
    {
        private Constants() { }

        //modifiers
        public static int NOMOD => 0x0000;
        public static int ALT => 0x0001;
        public static int CTRL => 0x0002;
        public static int SHIFT => 0x0004;
        public static int WIN => 0x0008;

        //windows message id for hotkey
        public static int WM_HOTKEY_MSG_ID => 0x0312;
    }
}
