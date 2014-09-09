using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ArtemisMissionEditor.SpaceMap
{
    /// <summary>
    /// Provides extension methods for Graphics in order to simplify drawing of the Space Map
    /// </summary>
    public static class GrahpicsExtention
    {
        /// <summary>
        /// Draw Ellipse defined with doubles, with same width and height, centered around point x, y
        /// </summary>
        public static void DrawCircle(this Graphics g, Pen pen, double x, double y, double size)
        {
            g.DrawEllipse(pen, (int)Math.Round(x - size / 2.0), (int)Math.Round(y - size / 2.0), (int)Math.Round(size), (int)Math.Round(size));
        }

        /// <summary>
        /// Fill Ellipse defined with doubles, with same width and height, centered around point x, y
        /// </summary>
        public static void FillCircle(this Graphics g, Brush brush, double x, double y, double size)
        {
            g.FillEllipse(brush, (int)Math.Round(x - size / 2.0), (int)Math.Round(y - size / 2.0), (int)Math.Round(size), (int)Math.Round(size));
        }

        public static void DrawLineD(this Graphics g, Pen pen, double x1, double y1, double x2, double y2)
        {
            g.DrawLine(pen, (int)Math.Round(x1), (int)Math.Round(y1), (int)Math.Round(x2), (int)Math.Round(y2));
        }

        public static void DrawStringD(this Graphics g, string s, Font font, Brush brush, double x, double y, StringFormat format)
        {
            g.DrawString(s, font, brush, (float)x, (float)y, format);
        }

        public static void DrawRectangleD(this Graphics g, Pen pen, double x, double y, double width, double height)
        {
            g.DrawRectangle(pen, (int)Math.Round(x), (int)Math.Round(y), (int)Math.Round(width), (int)Math.Round(height));
        }

        public static void FillRectangleD(this Graphics g, Brush brush, double x, double y, double width, double height)
        {
            g.FillRectangle(brush, (int)Math.Round(x), (int)Math.Round(y), (int)Math.Round(width), (int)Math.Round(height));
        }
        /// <summary>
        /// Draw Rectangle defined with doubles, with same width and height, centered around point x, y
        /// </summary>
        public static void DrawSquare(this Graphics g, Pen pen, double x, double y, double side)
        {
            g.DrawRectangleD(pen, x - side / 2.0, y - side / 2.0, side, side);
        }

        /// <summary>
        /// Fill Rectangle defined with doubles, with same width and height, centered around point x, y
        /// </summary>
        public static void FillSquare(this Graphics g, Brush brush, double x, double y, double side)
        {
            g.FillRectangleD(brush, x - side / 2.0, y - side / 2.0, side, side);
        }

        /// <summary>
        /// Draw Arc defined with doubles, with same width and height, centered around point x, y
        /// </summary>
        public static void DrawCircleArc(this Graphics g, Pen pen, double x, double y, double size, double startAngle, double sweepAngle)
        {
            g.DrawArc(pen, (int)Math.Round(x - size / 2.0), (int)Math.Round(y - size / 2.0), (int)Math.Round(size), (int)Math.Round(size), (float)startAngle, (float)sweepAngle);
        }
    }
}
