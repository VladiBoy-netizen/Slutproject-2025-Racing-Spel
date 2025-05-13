using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UnderRun
{
    public static class RaylibTextHelper
    {
        public static void DrawTextBoxed(string text, Rectangle rec, int fontSize, int spacing, Color tint)
        {
            string[] words = text.Split(new char[] { ' ' }, System.StringSplitOptions.None);

            float lineSpacing = fontSize * 1.5f;
            float x = rec.X;
            float y = rec.Y;
            string line = "";

            foreach (string word in words)
            {
                string testLine = (line.Length == 0) ? word : line + " " + word;
                int lineWidth = Raylib.MeasureText(testLine, fontSize);

                if (lineWidth > rec.Width)
                {
                    Raylib.DrawText(line, (int)x, (int)y, fontSize, tint);
                    y += lineSpacing;
                    if (y + lineSpacing > rec.Y + rec.Height) break;
                    line = word;
                }
                else
                {
                    line = testLine;
                }
            }

            // Draw last remaining line
            if (y + lineSpacing <= rec.Y + rec.Height)
            {
                Raylib.DrawText(line, (int)x, (int)y, fontSize, tint);
            }
        }
    }
}
