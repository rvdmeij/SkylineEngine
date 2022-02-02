using System;
using System.Runtime.InteropServices;
using SkylineEngine.Utilities;

namespace SkylineEngine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2i : IComparable
    {
        public int x;
        public int y;

        public static readonly Vector2i down = new Vector2i(0, -1);
        public static readonly Vector2i left = new Vector2i(-1, 0);
        public static readonly Vector2i one = new Vector2i(1, 1);
        public static readonly Vector2i right = new Vector2i(1, 0);
        public static readonly Vector2i up = new Vector2i(0, 1);
        public static readonly Vector2i zero = new Vector2i(0, 0);

        public float magnitude
        {
            get
            {
                return Mathf.Sqrt(x * x + y * y);
            }
        }

        public Vector2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public OpenTK.Mathematics.Vector2i ToOpenTKVector()
        {
            return new OpenTK.Mathematics.Vector2i(x, y);
        }

        public float[] ToArray()
        {
            return new float[] { x, y };
        }

        public byte[] ToBytes()
        {
            byte[] bytes = new byte[6];
            BinaryConverter.GetBytes(x, bytes, 0);
            BinaryConverter.GetBytes(y, bytes, 4);
            return bytes;
        }

        public override string ToString()
        {
            string text = "(" + x + "," + y + ")";
            return text;
        }

        public static float Distance(Vector2i a, Vector2i b)
        {
            return (float)(Mathf.Sqrt(Mathf.Pow(b.x - a.x, 2) + Mathf.Pow(b.y - a.y, 2)));
        }

        public static Vector2i Lerp(Vector2i v1, Vector2i v2, float t)
        {
            float x = Mathf.Lerp(v1.x, v2.x, t);
            float y = Mathf.Lerp(v1.y, v2.y, t);
            return new Vector2i((int)x, (int)y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2i && Equals((Vector2i)obj);
        }

        public bool Equals(Vector2i other)
        {
            return x == other.x && y == other.y;
        }

        public int CompareTo(object obj)
        {
            Vector2i other = (Vector2i)obj;

            if(other.x == x && other.y == y)
                return 0;
            return 1;
        }

        public static implicit operator Vector2i(Vector3 rhs)
        {
            return new Vector2i((int)rhs.x, (int)rhs.y);
        }

        public static bool operator ==(Vector2i left, Vector2i right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2i left, Vector2i right)
        {
            return !(left == right);
        }

        public static Vector2i operator +(Vector2i lhs, Vector2i rhs)
        {
            return new Vector2i(lhs.x + rhs.x, lhs.y + rhs.y);
        }

        public static Vector2i operator -(Vector2i lhs, Vector2i rhs)
        {
            return new Vector2i(lhs.x - rhs.x, lhs.y - rhs.y);
        }

        public static Vector2i operator -(Vector2i vec)
        {
            vec.x = -vec.x;
            vec.y = -vec.y;
            return vec;
        }

        public static Vector2i operator -(Vector2i lhs, int rhs)
        {
            lhs.x -= rhs;
            lhs.y -= rhs;
            return lhs;
        }

        public static Vector2i operator *(Vector2i lhs, Vector2i rhs)
        {
            return new Vector2i(lhs.x * rhs.x, lhs.y * rhs.y);
        }

        public static Vector2i operator /(Vector2i lhs, Vector2i rhs)
        {
            return new Vector2i(lhs.x / rhs.x, lhs.y / rhs.y);
        }


        public static Vector2i operator *(Vector2i lhs, int rhs)
        {
            lhs.x *= rhs;
            lhs.y *= rhs;
            return lhs;
        }

        public static Vector2i operator /(Vector2i lhs, int rhs)
        {
            lhs.x /= rhs;
            lhs.y /= rhs;
            return lhs;
        }
    }
}
