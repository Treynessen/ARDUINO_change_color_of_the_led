using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO.Ports;

static class Prog
{
    [DllImport("user32.dll")]
    static extern bool GetCursorPos(ref Point lpPoint);

    [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
    public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

    static Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

    public static Color GetColorAt(Point location)
    {
        using (Graphics screen = Graphics.FromImage(screenPixel))
        {
            using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
            {
                IntPtr hSrcDC = gsrc.GetHdc();
                IntPtr hDC = screen.GetHdc();
                int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                screen.ReleaseHdc();
                gsrc.ReleaseHdc();
            }
        }

        return screenPixel.GetPixel(0, 0);
    }

    static void Main()
    {
        SerialPort port = new SerialPort("COM3", 9600);
        port.Open();
        Color color;
        Point last_point = new Point();
        Point cursor_point = new Point();
        GetCursorPos(ref last_point);
        while (true)
        {
            GetCursorPos(ref cursor_point);
            if (last_point != cursor_point)
            {
                last_point = cursor_point;
                color = GetColorAt(last_point);
                port.Write(new byte[] { color.R, color.G, color.B }, 0, 3);
            }
            Thread.Sleep(100);
        }
    }
}
