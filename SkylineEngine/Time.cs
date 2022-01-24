using System;

﻿namespace SkylineEngine
{
    public static class Time
    {
        private static float m_time = 0;
        private static float m_deltaTime = 0;
        private static float m_fixedDeltaTime = 0;
        private static DateTime tp1;
        private static DateTime tp2;
        private static DateTime tp1Fixed;
        private static DateTime tp2Fixed;

        public static float time { get { return m_time; } }
        public static float deltaTime { get { return m_deltaTime; } }
        public static float fixedDeltaTime { get { return m_fixedDeltaTime; } }

        static Time()
        {
            tp1 = DateTime.Now;
            tp2 = DateTime.Now;
            tp1Fixed = DateTime.Now;
            tp2Fixed = DateTime.Now;
        }

        public static void SetDeltaTime()
        {
            tp2 = DateTime.Now;
            m_deltaTime = (float) ( (tp2.Ticks - tp1.Ticks) / 10000000.0 );
		    tp1 = tp2;
            m_time += m_deltaTime;
        }

        public static void SetFixedDeltaTime()
        {
            tp2Fixed = DateTime.Now;
            m_fixedDeltaTime = (float) ( (tp2Fixed.Ticks - tp1Fixed.Ticks) / 10000000.0 );
		    tp1Fixed = tp2Fixed;
        }
    }
}
