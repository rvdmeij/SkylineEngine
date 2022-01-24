using System;
using System.Runtime.InteropServices;
using SkylineEngine.Utilities;

namespace SkylineEngine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;

        public static readonly Vector3 back = new Vector3(0, 0, -1);
        public static readonly Vector3 down = new Vector3(0, -1, 0);
        public static readonly Vector3 forward = new Vector3(0, 0, 1);
        public static readonly Vector3 left = new Vector3(-1, 0, 0);
        public static readonly Vector3 one = new Vector3(1, 1, 1);
        public static readonly Vector3 right = new Vector3(1, 0, 0);
        public static readonly Vector3 up = new Vector3(0, 1, 0);
        public static readonly Vector3 zero = new Vector3(0, 0, 0);

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public OpenTK.Mathematics.Vector3 ToOpenTKVector()
        {
            return new OpenTK.Mathematics.Vector3(x, y, z);
        }

        public float[] ToArray()
        {
            return new float[] { x, y, z };
        }

        public byte[] ToBytes()
        {
            byte[] bytes = new byte[12];
            BinaryConverter.GetBytes(x, bytes, 0);
            BinaryConverter.GetBytes(y, bytes, 4);
            BinaryConverter.GetBytes(z, bytes, 8);
            return bytes;
        }

        public float magnitude
        {
            get
            {
                return (float)Math.Sqrt(x * x + y * y + z * z);
            }
        }

        public float sqrMagnitude
        {
            get
            {
                return x * x + y * y + z * z;
            }
        }

        public Vector3 normalized
        {
            get
            {
                float r = 1.0f / magnitude;
                return new Vector3(x * r, y * r, z * r);
            }
        }

        public float lengthSquared 
        {
            get 
            { 
                return (x * x) + (y * y) + (z * z); 
            }
        }

        public static float Angle(Vector3 v1, Vector3 v2)
        {
            return OpenTK.Mathematics.Vector3.CalculateAngle(v1.ToOpenTKVector(), v2.ToOpenTKVector());
        }

        public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(
                vector1.y * vector2.z - vector1.z * vector2.y,
                vector1.z * vector2.x - vector1.x * vector2.z,
                vector1.x * vector2.y - vector1.y * vector2.x);
        }

        public static Vector3 Cross2(Vector3 left, Vector3 right)
        {
            return new Vector3( left.y * right.z - left.z * right.y,
                                left.z * right.x - left.x * right.z,
                                left.x * right.y - left.y * right.x);            
        }

        public static float Distance(Vector3 a, Vector3 b)
        {
	        return (float)(Math.Sqrt(Math.Pow(b.x - a.x, 2) + Math.Pow(b.y - a.y, 2) + Math.Pow(b.z - a.z, 2)));
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
	        return (float)(Math.Sqrt(Math.Pow(b.x - a.x, 2) + Math.Pow(b.y - a.y, 2)));
        }        

        public static float Dot(Vector3 lhs, Vector3 rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            a.x = t * (b.x - a.x) + a.x;
            a.y = t * (b.y - a.y) + a.y;
            a.z = t * (b.z - a.z) + a.z;
            return a;
        }

        public static Vector3 Slerp(Vector3 a, Vector3 b, float t)
        {
	        // Dot product - the cosine of the angle between 2 vectors.
	        float dot = Vector3.Dot(a, b);
	        // Clamp it to be in the range of Acos()
	        // This may be unnecessary, but floating point
	        // precision can be a fickle mistress.
	        dot = Mathf.Clamp(dot, -1.0f, 1.0f);
	        // Acos(dot) returns the angle between start and end,
	        // And multiplying that by percent returns the angle between
	        // start and the final result.
	        float theta = Mathf.Acos(dot) * t;
	        Vector3 RelativeVec = b - a * dot;
	        RelativeVec = Normalize(RelativeVec);
	        // Orthonormal basis
	        // The final result.

	         Vector3 a1 = a * Mathf.Cos(theta);
	         Vector3 b1 = RelativeVec * Mathf.Sin(theta);

	         return a1 + b1;

             //return ((start*cos(theta)) + (RelativeVec*sin(theta)));
        }

        public static Vector3 Normalize(Vector3 v)
        {
	        float r = 1.0f / v.magnitude; 
	        return new Vector3(v.x * r, v.y * r, v.z * r);
        }

        public static Vector3 Transform(Vector3 vec, Quaternion quat)
        {
            Vector3 result;
            Transform(ref vec, ref quat, out result);
            return result;
        }

        public static void Transform(ref Vector3 vec, ref Quaternion quat, out Vector3 result)
        {
            // Since vec.W == 0, we can optimize quat * vec * quat^-1 as follows:
            // vec + 2.0 * cross(quat.xyz, cross(quat.xyz, vec) + quat.w * vec)
            Vector3 xyz = quat.xyz, temp, temp2;
            temp = Vector3.Cross(xyz, vec);
            temp2 = vec * quat.w;
            temp = temp + temp2;
            temp = Vector3.Cross(xyz, temp);
            temp = temp * 2;
            result = vec + temp;
        }

        public static void Min(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result.x = left.x < right.x ? left.x : right.x;
            result.y = left.y < right.y ? left.y : right.y;
            result.z = left.z < right.z ? left.z : right.z;
        }

        public static void Max(ref Vector3 left, ref Vector3 right, out Vector3 result)
        {
            result.x = left.x > right.x ? left.x : right.x;
            result.y = left.y > right.y ? left.y : right.y;
            result.z = left.z > right.z ? left.z : right.z;
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
            return obj is Vector3 && Equals((Vector3)obj);
        }

        public bool Equals(Vector3 other)
        {
            return x.NearlyEquals(other.x, 0.0001f) &&
                   y.NearlyEquals(other.y, 0.0001f) &&
                   z.NearlyEquals(other.z, 0.0001f);
        }

        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return !(left == right);
        }

        public static Vector3 operator + (Vector3 lhs, Vector3 rhs)
        {
	        return new Vector3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        public static Vector3 operator - (Vector3 lhs, Vector3 rhs)
        {
	        return new Vector3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        public static Vector3 operator -(Vector3 vec)
        {
            vec.x = -vec.x;
            vec.y = -vec.y;
            vec.z = -vec.z;
            return vec;
        }

        public static Vector3 operator * (Vector3 lhs, Vector3 rhs)
        {
	        return new Vector3(lhs.x * rhs.x, lhs.y * rhs.y, lhs.z * rhs.z);
        }

        public static Vector3 operator / (Vector3 lhs, Vector3 rhs)
        {
	        return new Vector3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
        }


        public static Vector3 operator * (Vector3 lhs, float rhs)
        {
	        lhs.x *= rhs;
	        lhs.y *= rhs;
	        lhs.z *= rhs;
	        return lhs;
        }

        public static Vector3 operator *(float lhs, Vector3 rhs)
        {
            rhs.x *= lhs;
            rhs.y *= lhs;
            rhs.z *= lhs;
            return rhs;
        }

        public static Vector3 operator / (Vector3 lhs, float rhs)
        {
	        lhs.x /= rhs;
	        lhs.y /= rhs;
	        lhs.z /= rhs;
	        return lhs;
        }
    }
}
