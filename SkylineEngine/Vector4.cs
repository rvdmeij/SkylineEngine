using System.Runtime.InteropServices;
using SkylineEngine.Utilities;

namespace SkylineEngine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public static readonly Vector4 zero   = new Vector4(0, 0, 0, 0);
        public static readonly Vector4 one    = new Vector4(1, 1, 1, 1);
        public static readonly Vector4 unit_x = new Vector4(1, 0, 0, 0);
        public static readonly Vector4 unit_y = new Vector4(0, 1, 0, 0);
        public static readonly Vector4 unit_z = new Vector4(0, 0, 1, 0);
        public static readonly Vector4 unit_w = new Vector4(0, 0, 0, 1);

        public float magnitude
        {
            get { return Mathf.Sqrt(x * x + y * y + z * z + w * w); }
        }

        public Vector3 xyz
        {
            get {return new Vector3(x, y, z); }
        }

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        

        public override string ToString()
        {
            string text = "(" +x + "," + y + "," + z + ")";
            return text;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Vector4 && Equals((Vector4)obj);
        }

        public bool Equals(Vector4 other)
        {
            return x.NearlyEquals(other.x, 0.0001f) &&
                   y.NearlyEquals(other.y, 0.0001f) &&
                   z.NearlyEquals(other.z, 0.0001f) &&
                   w.NearlyEquals(other.w, 0.0001f);
        }

        public static bool operator ==(Vector4 left, Vector4 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector4 left, Vector4 right)
        {
            return !(left == right);
        }

        public static Vector4 operator + (Vector4 lhs, Vector4 rhs)
        {
	        return new Vector4(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z, lhs.w + rhs.w);
        }

        public static Vector4 operator - (Vector4 lhs, Vector4 rhs)
        {
	        return new Vector4(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z, lhs.w - rhs.w);
        }

        public static Vector4 operator -(Vector4 vec)
        {
            vec.x = -vec.x;
            vec.y = -vec.y;
            vec.z = -vec.z;
            vec.w = -vec.w;
            return vec;
        }

        public static Vector4 operator * (Vector4 lhs, Vector4 rhs)
        {
	        return new Vector4(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z, lhs.w * rhs.w);
        }

        public static Vector4 operator / (Vector4 lhs, Vector4 rhs)
        {
	        return new Vector4(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z, lhs.w / rhs.w);
        }


        public static Vector4 operator * (Vector4 lhs, float rhs)
        {
	        lhs.x *= rhs;
	        lhs.y *= rhs;
	        lhs.z *= rhs;
            lhs.w *= rhs;
	        return lhs;
        }

        public static Vector4 operator *(float lhs, Vector4 rhs)
        {
            rhs.x *= lhs;
            rhs.y *= lhs;
            rhs.z *= lhs;
            rhs.w *= lhs;
            return rhs;
        }

        public static Vector4 operator / (Vector4 lhs, float rhs)
        {
	        lhs.x /= rhs;
	        lhs.y /= rhs;
	        lhs.z /= rhs;
            lhs.w /= rhs;
	        return lhs;
        }        
    }
}
