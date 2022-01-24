using System;
using SkylineEngine.Utilities;
using System.Runtime.InteropServices;

namespace SkylineEngine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Vector3 xyz
        {
            get { return new Vector3(x, y, z); }
            set 
            {
                x = value.x;
                y = value.y;
                z = value.z;
            }
        }

        const float radToDeg = (float)(180.0 / Math.PI);
	    const float degToRad = (float)(Math.PI / 180.0);

	    public Quaternion(float x, float y, float z, float w)
	    {
		    this.x = x;
		    this.y = y;
		    this.z = z;
		    this.w = w;

            this.xyz = new Vector3(x, y, z);
	    }

        public Quaternion(float rotationX, float rotationY, float rotationZ)
        {
            rotationX *= 0.5f;
            rotationY *= 0.5f;
            rotationZ *= 0.5f;

            var c1 = Mathf.Cos(rotationX);
            var c2 = Mathf.Cos(rotationY);
            var c3 = Mathf.Cos(rotationZ);
            var s1 = Mathf.Sin(rotationX);
            var s2 = Mathf.Sin(rotationY);
            var s3 = Mathf.Sin(rotationZ);

            w = (c1 * c2 * c3) - (s1 * s2 * s3);

            this.x = (s1 * c2 * c3) + (c1 * s2 * s3);
            this.y = (c1 * s2 * c3) - (s1 * c2 * s3);
            this.z = (c1 * c2 * s3) + (s1 * s2 * c3);
        }

        public Quaternion(Vector3 v, float w)
	    {
		    this.x = v.x;
		    this.y = v.y;
		    this.z = v.z;
		    this.w = w;

            this.xyz = new Vector3(x, y, z);
        }

	    public static Quaternion identity
	    {
		    get
		    {
			    return new Quaternion(0f, 0f, 0f, 1f);
		    }
	    }

	    public Vector3 eulerAngles
	    {
		    get
		    {
                return ToEulerAngles();
		    }
		    set
		    {
                this = FromEulerAngles(value);
		    }
	    }

	    public float Length
	    {
		    get
		    {
			    return (float)System.Math.Sqrt(x * x + y * y + z * z + w * w);
		    }
	    }

	    public float LengthSquared
	    {
		    get
		    {
			    return x * x + y * y + z * z + w * w;
		    }
	    }

	    public void Set(float new_x, float new_y, float new_z, float new_w)
	    {
		    this.x = new_x;
		    this.y = new_y;
		    this.z = new_z;
		    this.w = new_w;
	    }

        public static Quaternion AngleAxis(float angle, Vector3 axis)
        {
            if (Mathf.Abs(axis.lengthSquared) < float.Epsilon)
            {
                return Quaternion.identity;
            }

            var result = Quaternion.identity;

            angle *= 0.5f;
            axis = axis.normalized;
            result.xyz = axis * Mathf.Sin(angle);
            result.w = Mathf.Cos(angle);

            return Normalize(result);
        }

        public void Normalize()
	    {
		    float scale = 1.0f / this.Length;
		    xyz *= scale;
		    w *= scale;
	    }

	    public static Quaternion Normalize(Quaternion q)
	    {
		    Quaternion result;
		    Normalize(ref q, out result);
		    return result;
	    }

	    public static void Normalize(ref Quaternion q, out Quaternion result)
	    {
		    float scale = 1.0f / q.Length;
		    result = new Quaternion(q.xyz * scale, q.w * scale);
	    }

	    public static float Dot(Quaternion a, Quaternion b)
	    {
		    return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
	    }

	    public static float Angle(Quaternion a, Quaternion b)
	    {
		    float f = Quaternion.Dot(a, b);
		    return Mathf.Acos(Mathf.Min(Mathf.Abs(f), 1f)) * 2f * radToDeg;
	    }

	    public static Quaternion Euler(float x, float y, float z)
	    {
		    return Quaternion.FromEulerRad(new Vector3((float)x, (float)y, (float)z) * degToRad);
	    }

	    public static Quaternion Euler(Vector3 euler)
	    {
		    return Quaternion.FromEulerRad(euler * degToRad);
	    }

        private static Vector3 ToEulerRad(Quaternion rotation)
        {
	        float sqw = rotation.w * rotation.w;
	        float sqx = rotation.x * rotation.x;
	        float sqy = rotation.y * rotation.y;
	        float sqz = rotation.z * rotation.z;
	        float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
	        float test = rotation.x * rotation.w - rotation.y * rotation.z;
	        Vector3 v = new Vector3();

	        if (test > 0.4995f * unit)
	        { // singularity at north pole
		        v.y = 2f * Mathf.Atan2(rotation.y, rotation.x);
		        v.x = Mathf.PI / 2;
		        v.z = 0;
		        return NormalizeAngles(v * Mathf.Rad2Deg);
	        }
	        if (test < -0.4995f * unit)
	        { // singularity at south pole
		        v.y = -2f * Mathf.Atan2(rotation.y, rotation.x);
		        v.x = -Mathf.PI / 2;
		        v.z = 0;
		        return NormalizeAngles(v * Mathf.Rad2Deg);
	        }
	        Quaternion q = new Quaternion(rotation.w, rotation.z, rotation.x, rotation.y);
	        v.y = (float)System.Math.Atan2(2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w));     // Yaw
	        v.x = (float)System.Math.Asin(2f * (q.x * q.z - q.w * q.y));                             // Pitch
	        v.z = (float)System.Math.Atan2(2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z));      // Roll
	        return NormalizeAngles(v * Mathf.Rad2Deg);
        }

	    private static Quaternion FromEulerRad(Vector3 euler)
	    {
		    var yaw = euler.x;
		    var pitch = euler.y;
		    var roll = euler.z;

		    float rollOver2 = roll * 0.5f;
		    float sinRollOver2 = (float)System.Math.Sin((float)rollOver2);
		    float cosRollOver2 = (float)System.Math.Cos((float)rollOver2);
		    float pitchOver2 = pitch * 0.5f;
		    float sinPitchOver2 = (float)System.Math.Sin((float)pitchOver2);
		    float cosPitchOver2 = (float)System.Math.Cos((float)pitchOver2);
		    float yawOver2 = yaw * 0.5f;
		    float sinYawOver2 = (float)System.Math.Sin((float)yawOver2);
		    float cosYawOver2 = (float)System.Math.Cos((float)yawOver2);
		    Quaternion result = new Quaternion();
		    result.x = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
		    result.y = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;
		    result.z = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
		    result.w = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
		    return result;
	    }

        public void ToEulerAngles(out Vector3 angles)
        {
            angles = ToEulerAngles();
        }

        public Vector3 ToEulerAngles()
        {
            /*
            reference
            http://en.wikipedia.org/wiki/Conversion_between_quaternions_and_Euler_angles
            http://www.euclideanspace.com/maths/geometry/rotations/conversions/quaternionToEuler/
            */

            var q = this;

            Vector3 _eulerAngles;

            // Threshold for the singularities found at the north/south poles.
            const float SINGULARITY_THRESHOLD = 0.4999995f;

            var sqw = q.w * q.w;
            var sqx = q.x * q.x;
            var sqy = q.y * q.y;
            var sqz = q.z * q.z;
            var unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            var singularityTest = (q.x * q.z) + (q.w * q.y);

            if (singularityTest > SINGULARITY_THRESHOLD * unit)
            {
                _eulerAngles.z = (float)(2 * Math.Atan2(q.x, q.w));
                _eulerAngles.y = OpenTK.Mathematics.MathHelper.PiOver2;
                _eulerAngles.x = 0;
            }
            else if (singularityTest < -SINGULARITY_THRESHOLD * unit)
            {
                _eulerAngles.z = (float)(-2 * Math.Atan2(q.x, q.w));
                _eulerAngles.y = -OpenTK.Mathematics.MathHelper.PiOver2;
                _eulerAngles.x = 0;
            }
            else
            {
                _eulerAngles.z = Mathf.Atan2(2 * ((q.w * q.z) - (q.x * q.y)), sqw + sqx - sqy - sqz);
                _eulerAngles.y = Mathf.Asin(2 * singularityTest / unit);
                _eulerAngles.x = Mathf.Atan2(2 * ((q.w * q.x) - (q.y * q.z)), sqw - sqx - sqy + sqz);
            }

            return _eulerAngles;
        }

        public static Quaternion FromEulerAngles(float pitch, float yaw, float roll)
        {
            return new Quaternion(pitch, yaw, roll);
        }

        public static Quaternion FromEulerAngles(Vector3 eulerAngles)
        {
            return new Quaternion(eulerAngles.x, eulerAngles.y, eulerAngles.z);
        }

        private static Vector3 NormalizeAngles(Vector3 angles)
	    {
		    angles.x = NormalizeAngle(angles.x);
		    angles.y = NormalizeAngle(angles.y);
		    angles.z = NormalizeAngle(angles.z);
		    return angles;
	    }

	    private static float NormalizeAngle(float angle)
	    {
		    while (angle > 360)
			    angle -= 360;
		    while (angle < 0)
			    angle += 360;
		    return angle;
	    }

        public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
        {
            return Quaternion.Slerp(ref a, ref b, t);
        }

        private static Quaternion Slerp(ref Quaternion a, ref Quaternion b, float t)
        {
            if (t > 1) t = 1;
            if (t < 0) t = 0;
            return SlerpUnclamped(ref a, ref b, t);
        }

        public static Quaternion SlerpUnclamped(Quaternion a, Quaternion b, float t)
        {
            return Quaternion.SlerpUnclamped(ref a, ref b, t);
        }
        private static Quaternion SlerpUnclamped(ref Quaternion a, ref Quaternion b, float t)
        {
            // if either input is zero, return the other.
            if (a.LengthSquared == 0.0f)
            {
                if (b.LengthSquared == 0.0f)
                {
                    return identity;
                }
                return b;
            }
            else if (b.LengthSquared == 0.0f)
            {
                return a;
            }


            float cosHalfAngle = a.w * b.w + Vector3.Dot(a.xyz, b.xyz);

            if (cosHalfAngle >= 1.0f || cosHalfAngle <= -1.0f)
            {
                // angle = 0.0f, so just return one input.
                return a;
            }
            else if (cosHalfAngle < 0.0f)
            {
                b.xyz = -b.xyz;
                b.w = -b.w;
                cosHalfAngle = -cosHalfAngle;
            }

            float blendA;
            float blendB;
            if (cosHalfAngle < 0.99f)
            {
                // do proper slerp for big angles
                float halfAngle = (float)System.Math.Acos(cosHalfAngle);
                float sinHalfAngle = (float)System.Math.Sin(halfAngle);
                float oneOverSinHalfAngle = 1.0f / sinHalfAngle;
                blendA = (float)System.Math.Sin(halfAngle * (1.0f - t)) * oneOverSinHalfAngle;
                blendB = (float)System.Math.Sin(halfAngle * t) * oneOverSinHalfAngle;
            }
            else
            {
                // do lerp if angle is really small.
                blendA = 1.0f - t;
                blendB = t;
            }

            Quaternion result = new Quaternion(blendA * a.xyz + blendB * b.xyz, blendA * a.w + blendB * b.w);
            if (result.LengthSquared > 0.0f)
                return Normalize(result);
            else
                return identity;
        }        

        public override string ToString()
        {
            string text = "(" + x + "," + y + "," + z + "," + w + ")";
            return text;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Quaternion && Equals((Quaternion)obj);
        }

        public bool Equals(Quaternion other)
        {
            return x.NearlyEquals(other.x, 0.0001f) &&
                   y.NearlyEquals(other.y, 0.0001f) &&
                   z.NearlyEquals(other.z, 0.0001f) &&
                   w.NearlyEquals(other.w, 0.0001f);
        }

        public static bool operator ==(Quaternion left, Quaternion right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Quaternion left, Quaternion right)
        {
            return !(left == right);
        }

        public static Vector3 operator *(Quaternion rotation, Vector3 point)
        {
            float num1 = rotation.x * 2f;
            float num2 = rotation.y * 2f;
            float num3 = rotation.z * 2f;
            float num4 = rotation.x * num1;
            float num5 = rotation.y * num2;
            float num6 = rotation.z * num3;
            float num7 = rotation.x * num2;
            float num8 = rotation.x * num3;
            float num9 = rotation.y * num3;
            float num10 = rotation.w * num1;
            float num11 = rotation.w * num2;
            float num12 = rotation.w * num3;
            Vector3 vector3;
            vector3.x = (float)((1.0 - ((double)num5 + (double)num6)) * (double)point.x + ((double)num7 - (double)num12) * (double)point.y + ((double)num8 + (double)num11) * (double)point.z);
            vector3.y = (float)(((double)num7 + (double)num12) * (double)point.x + (1.0 - ((double)num4 + (double)num6)) * (double)point.y + ((double)num9 - (double)num10) * (double)point.z);
            vector3.z = (float)(((double)num8 - (double)num11) * (double)point.x + ((double)num9 + (double)num10) * (double)point.y + (1.0 - ((double)num4 + (double)num5)) * (double)point.z);
            return vector3;
        }

        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            return Multiply(left, right);

        }

        public static Quaternion operator +(Quaternion left, Quaternion right)
        {
            left.xyz += right.xyz;
            left.w += right.w;
            return left;
        }
        
        public static Quaternion operator -(Quaternion left, Quaternion right)
        {
            left.xyz -= right.xyz;
            left.w -= right.w;
            return left;
        }

        public static Quaternion Multiply(Quaternion left, Quaternion right)
        {
            return new Quaternion(
                (left.xyz * right.w) + (right.xyz * left.w) + Vector3.Cross(left.xyz, right.xyz),
                (left.w * right.w) - Vector3.Dot(left.xyz, right.xyz));
        }

        public static Quaternion Inverse(Quaternion q)
        {
            var lengthSq = q.LengthSquared;
            if (lengthSq != 0.0)
            {
                var i = 1.0f / lengthSq;
                return new Quaternion(q.xyz * -i, q.w * i);
            }
            else
            {
                return q;
            }
        } 

        public OpenTK.Mathematics.Quaternion ToOpenTKQuaternion()
        {
            return new OpenTK.Mathematics.Quaternion(x, y, z, w);
        }
    }
}
