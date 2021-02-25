using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace godhavemercyuponme
{
    static class Bitmap
    {


        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }


        [DllImport(@"KMPDLL.dll", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]

        public static extern int ShowBitMap(IntPtr hWnd,
           IntPtr hBit,
            int x, int y);



        [DllImport(@"KMPDLL.dll", EntryPoint ="ShowBitMapFromFile", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]

        public static extern int ShowBitMap(IntPtr hWnd,
           string Name,
            int x, int y);










        [DllImport("KMPDLL.dll", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ClientToBmp(IntPtr hWnd, string Name);



        [DllImport(@"KMPDLL.dll", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ClientRectToBmp(IntPtr hWnd, string name, RECT r);



        public static RECT ToRECT(this Rect r)
        {
            var top = ConversionHelper.TransformToYPixel(r.Top);
            var bottom = ConversionHelper.TransformToYPixel(r.Bottom);
            var left = ConversionHelper.TransformToXPixel(r.Left);
            var right = ConversionHelper.TransformToXPixel(r.Right);

            return new RECT() { Top = top, Bottom = bottom, Left = left, Right = right };

        }

        public static int ClientRectToBmp(IntPtr hWnd, string name, Rect r) => ClientRectToBmp(hWnd, name, r.ToRECT());




    }

    static class ConversionHelper
    {
        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hDc, int nIndex);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);

        private const int LOGPIXELSX = 88;
        private const int LOGPIXELSY = 90;

        public static int TransformToXPixel(double unit)
        {
            IntPtr hDc = GetDC(IntPtr.Zero);
            if (hDc != IntPtr.Zero)
            {
                int dpiX = GetDeviceCaps(hDc, LOGPIXELSX);


                ReleaseDC(IntPtr.Zero, hDc);

                return (int)(((double)dpiX / 96) * unit);

            }
            else
                throw new ArgumentNullException("DC не будет");

        }

        public static int TransformToYPixel(double unit)
        {
            IntPtr hDc = GetDC(IntPtr.Zero);
            if (hDc != IntPtr.Zero)
            {

                int dpiY = GetDeviceCaps(hDc, LOGPIXELSY);

                ReleaseDC(IntPtr.Zero, hDc);


                return (int)(((double)dpiY / 96) * unit);
            }
            else
                throw new ArgumentNullException("DC не будет");

        }

    }
}
