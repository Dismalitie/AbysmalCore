using AbysmalCore.Debugging;
using System.Runtime.InteropServices;

namespace AbysmalCore.UI
{
    public class DwmPInvokeHelper
    {
        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(
            IntPtr hwnd,
            int attr,
            ref bool attrValue,
            int attrSize
        );

        public static void SetWindowTheme(IntPtr hWnd, bool darkmode = true)
        {
            if (hWnd == IntPtr.Zero)
            {
                return;
            }

            bool attribute = darkmode;
            int sizeOfAttribute = sizeof(bool); // Note: BOOL is often 4 bytes, but for this specific attribute `bool` in C# often works for Win32 BOOL.

            /// 20 = DWMWA_USE_IMMERSIVE_DARK_MODE
            int result = DwmSetWindowAttribute(
                hWnd,
                20,
                ref attribute,
                sizeOfAttribute
            );

            if (result != 0)
            {
                Debug.Error(new DwmPInvokeHelper(), "Failed to set window attribute. HRESULT: " + result, true);
            }
        }
    }
}
