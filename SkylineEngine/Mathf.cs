using System;
using System.ComponentModel;

namespace SkylineEngine
{
    public static class Mathf
    {
        public const float Rad2Deg = (float)(180.0 / Math.PI);
        public const float Deg2Rad = (float)(Math.PI / 180.0);
        public const float PI = (float)Math.PI;
        public const float PiOver2 = PI / 2.0f;
        public const float Infinity = float.PositiveInfinity;
        public const float NegativeInfinity = float.NegativeInfinity;

        public static float Atan2(float x, float y)
        {
            return (float)System.Math.Atan2(x, y);
        }

        public static float Asin(float x)
        {
            return (float)System.Math.Asin(x);
        }

        public static float Abs(float f)
        {
            if(f < 0.0f)
                return f *= -1.0f;
            return f;
        }

        public static float Ceil(float x)
        {
            return (float)System.Math.Ceiling(x);
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value > max)
                value = max;
            if (value < min)
                value = min;
            return value;
        }

        /// <summary>
        ///   <para>Clamps value between 0 and 1 and returns value.</para>
        /// </summary>
        /// <param name="value"></param>
        public static float Clamp01(float value)
        {
            if ((double)value < 0.0)
                return 0.0f;
            if ((double)value > 1.0)
                return 1f;
            return value;
        }

        public static float Floor(float x)
        {
            return (float)System.Math.Floor(x);
        }

        public static int FloorToInt(float x)
        {
            return (int)System.Math.Floor(x);
        }

        public static float Lerp(float a, float b, float t)
        {
            t = Clamp(t, 0.0f, 1.0f);
            return a + ((b-a) * t);
        }

        /// <summary>
        ///   <para>Same as Lerp but makes sure the values interpolate correctly when they wrap around 360 degrees.</para>
        /// </summary>
        public static float LerpAngle(float a, float b, float t)
        {
            float num = Mathf.Repeat(b - a, 360f);
            if ((double)num > 180.0)
                num -= 360f;
            return a + num * Mathf.Clamp01(t);
        }

        public static float InverseLerp(float a, float b, float value)
        {
            return (value - a) / (b - a);
        }

        public static float Min(float a, float b)
        {
            if(a > b)
                return b;
            return a;
        }

        public static float Max(float a, float b)
        {
            if(a > b)
                return a;
            return b;
        }
        
        public static float Slerp(float a, float b, float t)
        {
            Vector3 from = new Vector3(a, a, a);
            Vector3 to = new Vector3(b, b, b);
            Vector3 result = Vector3.Slerp(from, to, t);
            return result.x;
        }

        /// <summary>
        ///   <para>Loops the value t, so that it is never larger than length and never smaller than 0.</para>
        /// </summary>
        public static float Repeat(float t, float length)
        {
            return t - Mathf.Floor(t / length) * length;
        }

        public static float Sign(float f)
        {
            return (double)f >= 0.0 ? 1f : -1f;
        }

        public static float Sin(float x)
        {
            return (float)Math.Sin(x);
        }

        public static float Cos(float x)
        {
            return (float)Math.Cos(x);
        }

        public static float Acos(float x)
        {
            return (float)Math.Acos(x);
        }

        public static float Sqrt(float x)
        {
            return (float)Math.Sqrt(x);
        }

        public static float Pow(float x, float y)
        {
            return (float)Math.Pow(x, y);
        }

        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
        {
            float deltaTime = Time.deltaTime;
            return Mathf.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }

        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime)
        {
            float deltaTime = Time.deltaTime;
            float maxSpeed = float.PositiveInfinity;
            return Mathf.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
        }


        public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, [DefaultValue("Mathf.Infinity")] float maxSpeed, [DefaultValue("Time.deltaTime")] float deltaTime)
        {
            smoothTime = Mathf.Max(0.0001f, smoothTime);
            float num1 = 2f / smoothTime;
            float num2 = num1 * deltaTime;
            float num3 = (float)(1.0 / (1.0 + (double)num2 + 0.479999989271164 * (double)num2 * (double)num2 + 0.234999999403954 * (double)num2 * (double)num2 * (double)num2));
            float num4 = current - target;
            float num5 = target;
            float max = maxSpeed * smoothTime;
            float num6 = Mathf.Clamp(num4, -max, max);
            target = current - num6;
            float num7 = (currentVelocity + num1 * num6) * deltaTime;
            currentVelocity = (currentVelocity - num1 * num7) * num3;
            float num8 = target + (num6 + num7) * num3;
            if ((double)num5 - (double)current > 0.0 == (double)num8 > (double)num5)
            {
                num8 = num5;
                currentVelocity = (num8 - num5) / deltaTime;
            }
            return num8;
        }

    }
}
