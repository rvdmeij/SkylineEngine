namespace SkylineEngine
{
    public static class Screen
    {
        private static int m_width;
        private static int m_height;
        private static Vector2 m_size = new Vector2();

        public static int width { get { return m_width; } }
        public static int height { get { return m_height; } }
        public static Vector2 size { get { return m_size; } }

        public static void SetSize(int width, int height)
        {
            m_width = width;
            m_height = height;
            m_size = new Vector2(width, height);
        }
    }
}