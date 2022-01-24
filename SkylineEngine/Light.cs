using System;
namespace SkylineEngine
{
    public sealed class Light : Component
    {
        public Color32 color;
        public float strength;

        private static Light m_mainLight;

        public static Light main { get { return m_mainLight; } }

        public Light()
        {
            color = new Color32(1.0f, 1.0f, 1.0f, 1.0f);
            strength = 10.0f;

            if (m_mainLight == null)
                m_mainLight = this;
        }
    }
}
