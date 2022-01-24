using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace SkylineEngine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public float x;
        public float y;
        public float width;
        public float height;

        public Rect(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public bool Contains(Vector2 point)
        {
            if(point.x >= x && point.x < (x+width))
            {
                if(point.y >= y && point.y < (y+height))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Rect[]> GetRect2D(int leftIndent, int topIndent, int width, int height, int rows, int cols, int offsetX = 0, int offsetY = 0)
        {
            List<Rect[]> r = new List<Rect[]>();

            for (int i = 0; i < rows; i++)
            {
                Rect[] rects = new Rect[cols];
                for (int j = 0; j < cols; j++)
                {
                    int x = leftIndent + (j * (width + offsetX));
                    int y = topIndent + (i * (height + offsetY));
                    rects[j] = new Rect(x, y, width, height);
                }
                r.Add(rects);
            }

            return r;
        }

        public override string ToString()
        {
            return x + "," + y + "," + width + "," + height;
        }
    }
}
