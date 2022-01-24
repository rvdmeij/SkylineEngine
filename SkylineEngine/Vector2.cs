﻿using System.Runtime.InteropServices;
using SkylineEngine.Utilities;

namespace SkylineEngine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2
    {
        public float x;
        public float y;

        public static readonly Vector2 down = new Vector2(0, -1);
        public static readonly Vector2 left = new Vector2(-1, 0);
        public static readonly Vector2 one = new Vector2(1, 1);
        public static readonly Vector2 right = new Vector2(1, 0);
        public static readonly Vector2 up = new Vector2(0, 1);
        public static readonly Vector2 zero = new Vector2(0, 0);

        public float magnitude
        {
            get
            {
                return Mathf.Sqrt(x * x + y * y);
            }
        }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public OpenTK.Mathematics.Vector2 ToOpenTKVector()
        {
            return new OpenTK.Mathematics.Vector2(x, y);
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

        public static float Distance(Vector2 a, Vector2 b)
        {
            return (float)(Mathf.Sqrt(Mathf.Pow(b.x - a.x, 2) + Mathf.Pow(b.y - a.y, 2)));
        }

        public static Vector2 Lerp(Vector2 v1, Vector2 v2, float t)
        {
            float x = Mathf.Lerp(v1.x, v2.x, t);
            float y = Mathf.Lerp(v1.y, v2.y, t);
            return new Vector2(x, y);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2 && Equals((Vector2)obj);
        }

        public bool Equals(Vector2 other)
        {
            return x.NearlyEquals(other.x, 0.0001f) &&
                   y.NearlyEquals(other.y, 0.0001f);
        }

        public static implicit operator Vector2(Vector3 rhs)
        {
            return new Vector2(rhs.x, rhs.y);
        }

        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return !(left == right);
        }

        public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x + rhs.x, lhs.y + rhs.y);
        }

        public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x - rhs.x, lhs.y - rhs.y);
        }

        public static Vector2 operator -(Vector2 vec)
        {
            vec.x = -vec.x;
            vec.y = -vec.y;
            return vec;
        }

        public static Vector2 operator -(Vector2 lhs, float rhs)
        {
            lhs.x -= rhs;
            lhs.y -= rhs;
            return lhs;
        }

        public static Vector2 operator *(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x * rhs.x, lhs.y * rhs.y);
        }

        public static Vector2 operator /(Vector2 lhs, Vector2 rhs)
        {
            return new Vector2(lhs.x / rhs.x, lhs.y / rhs.y);
        }


        public static Vector2 operator *(Vector2 lhs, float rhs)
        {
            lhs.x *= rhs;
            lhs.y *= rhs;
            return lhs;
        }

        public static Vector2 operator /(Vector2 lhs, float rhs)
        {
            lhs.x /= rhs;
            lhs.y /= rhs;
            return lhs;
        }
    }
}
