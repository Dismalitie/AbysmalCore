using AbysmalCore.Debugging;
using System.Runtime.InteropServices;

namespace AbysmalCore.UI
{
    /// <summary>
    /// Windows-platform specific window helper
    /// </summary>
    [DebugInfo("windows dwm api helper", true)]
    public class DwmPInvokeHelper
    {
        private enum DWMWINDOWATTRIBUTE : uint
        {
            DWMWA_BORDER_COLOR = 34,
            DWMWA_CAPTION_COLOR = 35,
            DWMWA_TEXT_COLOR = 36,
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20, // unused, kept for reference
            DWMWA_SYSTEMBACKDROP_TYPE = 38 // used for backdrop material (mica/acrylic)
        }

        /// pinvoke
        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(
            IntPtr hwnd,
            DWMWINDOWATTRIBUTE dwAttribute,
            ref int pvAttribute,
            uint cbAttribute);

        /// convert to colorref fmt (0x00BBGGRR)
        private static int ColorToCOLORREF(Color color)
        {
            return color.R | (color.G << 8) | (color.B << 16);
        }

        /// <summary>
        /// Type of window material
        /// </summary>
        public enum MaterialType : int
        {
            /// <summary>
            /// No material
            /// </summary>
            None = 1,
            /// <summary>
            /// Slightly translucent material
            /// </summary>
            Mica = 2,
            /// <summary>
            /// Semi-transparent material
            /// </summary>
            Acrylic = 3,
            /// <summary>
            /// Translucent material
            /// </summary>
            Tabbed = 4
        }

        /// <summary>
        /// Sets the material of a window background
        /// </summary>
        /// <param name="hWnd"></param>The window handle
        /// <param name="material"></param>The material to use
        public static void SetMaterial(IntPtr hWnd, MaterialType material)
        {
            int n = (int)material;
            int hresult = DwmSetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref n, (uint)Marshal.SizeOf<int>());

            if (hresult != 0)
            {
                AbysmalDebug.Warn(new DwmPInvokeHelper(), $"Failed to set material type {material} on hwnd {hWnd}, error code: {hresult}");
            }
        }

        /// <summary>
        /// Sets the color of the titlebar
        /// </summary>
        /// <param name="hWnd"></param>The window handle
        /// <param name="r"></param>The red channel
        /// <param name="g"></param>The green channel
        /// <param name="b"></param>The blue channel
        public static void SetNonClientColor(IntPtr hWnd, int r, int g, int b)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                AbysmalDebug.Warn(new DwmPInvokeHelper(), "Called windows specific func (SetCaptionColor), aborted");
                return;
            }

            int colorRef = ColorToCOLORREF(new Color(r,g,b));
            uint sizeOfInt = (uint)Marshal.SizeOf(typeof(int));

            DwmSetWindowAttribute(
                hWnd,
                DWMWINDOWATTRIBUTE.DWMWA_CAPTION_COLOR,
                ref colorRef,
                sizeOfInt);
        }

        /// <summary>
        /// Sets the text of the titlebar caption
        /// </summary>
        /// <param name="hWnd"></param>The window handle
        /// <param name="r"></param>The red channel
        /// <param name="g"></param>The green channel
        /// <param name="b"></param>The blue channel
        public static void SetNonClientTextColor(IntPtr hWnd, int r, int g, int b)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                AbysmalDebug.Warn(new DwmPInvokeHelper(), "Called windows specific func (SetCaptionTextColor), aborted");
                return;
            }

            int colorRef = ColorToCOLORREF(new Color(r, g, b));
            uint sizeOfInt = (uint)Marshal.SizeOf(typeof(int));

            DwmSetWindowAttribute(
                hWnd,
                DWMWINDOWATTRIBUTE.DWMWA_TEXT_COLOR,
                ref colorRef,
                sizeOfInt);
        }

        /// <summary>
        /// Sets the color of the window border
        /// </summary>
        /// <param name="hWnd"></param>The window handle
        /// <param name="r"></param>The red channel
        /// <param name="g"></param>The green channel
        /// <param name="b"></param>The blue channel
        public static void SetBorderColor(IntPtr hWnd, int r, int g, int b)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                AbysmalDebug.Warn(new DwmPInvokeHelper(), "Called windows specific func (SetBorderColor), aborted");
                return;
            }

            int colorRef = ColorToCOLORREF(new Color(r, g, b));
            uint sizeOfInt = (uint)Marshal.SizeOf(typeof(int));

            DwmSetWindowAttribute(
                hWnd,
                DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR,
                ref colorRef,
                sizeOfInt);
        }
    }
}
