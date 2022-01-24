namespace SkylineEngine
{
    public static class AudioSettings
    {
        private static int m_outputSampleRate = 44100;
        private static int m_outputBufferSize = 2048;

        public static int outputSampleRate
        {
            get { return m_outputSampleRate; }
            set { m_outputSampleRate = value; }
        }

        public static int outputBufferSize
        {
            get { return m_outputBufferSize; }
            set { m_outputBufferSize = value; }
        }
    }
}
