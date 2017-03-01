using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace JellyFish
{
    public class Mouse
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        private const int MOUSEEVENTF_WHEEL = 0x0800;
        private const int WHEEL_DELTA = -120;
        public static void moveAndClick(int x, int y)
        {
            System.Drawing.Point currPosition = System.Windows.Forms.Cursor.Position;
            int curX = currPosition.X;
            int curY = currPosition.Y;

            int xdiff = x - curX;
            int ydiff = y - curY;
            
            int maxStep = 50;

            for (int i = 1; i <= maxStep; i++)
            {
                double xtemp = (i) * xdiff / maxStep;
                double ytemp = (i) * ydiff / maxStep;
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(curX+ (int)xtemp, curY+(int)ytemp);
                Thread.Sleep(15);
            }
            Thread.Sleep(500);
            LeftClick();
        }

        public static void moveAndScroll(int x, int y, int lines)
        {
            System.Drawing.Point currPosition = System.Windows.Forms.Cursor.Position;
            int curX = currPosition.X;
            int curY = currPosition.Y;

            int xdiff = x - curX;
            int ydiff = y - curY;

            int maxStep = 50;

            for (int i = 1; i <= maxStep; i++)
            {
                double xtemp = (i) * xdiff / maxStep;
                double ytemp = (i) * ydiff / maxStep;
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(curX + (int)xtemp, curY + (int)ytemp);
                Thread.Sleep(15);
            }
            Thread.Sleep(500);
            scroll(lines);
        }

        public static void Move(int x, int y)
        {
            MoveTo(0,0);
            mouse_event(MOUSEEVENTF_MOVE, x, y, 0, 0);
        }
        public static void MoveTo(int x, int y)
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, x, y, 0, 0);
        }
        public static void LeftClick()
        {            
            mouse_event(MOUSEEVENTF_LEFTDOWN, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
        }

        public static void LeftDown()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
        }

        public static void LeftUp()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
        }

        public static void RightClick()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
        }

        public static void RightDown()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
        }

        public static void RightUp()
        {
            mouse_event(MOUSEEVENTF_RIGHTUP, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
        }
        private static void scroll(int count)
        {
            mouse_event(MOUSEEVENTF_WHEEL, 0, 0, WHEEL_DELTA * count, 0);
        }
    }
}
