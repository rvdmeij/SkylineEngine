using System;
using System.IO;
using SkylineEngine.IO;
namespace SkylineEngine.Audio
{
    public struct WaveFileSpecification
    {
        public int chunkId;
        public int chunkSize;
        public int format;
        public int subChunk1Id;
        public int subChunk1Size;
        public Int16 audioFormat;
        public Int16 numChannels;
        public int samplingRate;
        public int byteRate;
        public Int16 blockAlign;
        public Int16 bitsPerSample;
        public int subChunk2Id;
        public int subChunk2Size;
    }

    public class WaveFileReader : IDisposable
    {
        public delegate void ReadEvent(byte[] bytes, int length);
        public delegate void ReadFinishedEvent();

        public event ReadEvent onRead;
        public event ReadFinishedEvent onReadFinished;

        private FileStream stream;
        private StreamInfo streamInfo;
        private int currentChunk;
        private long currentChunkSize;

        private WaveFileSpecification wavefileSpecification;
        private byte[] m_buffer;
        private bool m_canRead;

        public int sampleRate { get { return wavefileSpecification.samplingRate; } }
        public long dataLength { get { return wavefileSpecification.subChunk2Size; } }
        public int channels { get { return wavefileSpecification.numChannels; } }
        public Int16 bitsPerSample { get { return wavefileSpecification.bitsPerSample; } }
        public bool canRead { get { return m_canRead; } }

        public WaveFileReader()
        {
            m_buffer = new byte[4096];
            wavefileSpecification = new WaveFileSpecification();
        }

        public bool Open(string filename) 
        {
            FileInfo info = new FileInfo(filename);

            if (!info.Exists)
                return false;

            streamInfo = new StreamInfo(filename, info.Length, m_buffer.Length);
            currentChunk = 0;

            stream = new FileStream(streamInfo.filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            stream.Read(m_buffer, 0, 44);

            wavefileSpecification = new WaveFileSpecification();

            wavefileSpecification.chunkId = BitConverter.ToInt32(m_buffer, 0);
            wavefileSpecification.chunkSize = BitConverter.ToInt32(m_buffer, 4);
            wavefileSpecification.format = BitConverter.ToInt32(m_buffer, 8);
            wavefileSpecification.subChunk1Id = BitConverter.ToInt32(m_buffer, 12);
            wavefileSpecification.subChunk1Size = BitConverter.ToInt32(m_buffer, 16);
            wavefileSpecification.audioFormat = BitConverter.ToInt16(m_buffer, 20);
            wavefileSpecification.numChannels = BitConverter.ToInt16(m_buffer, 22);
            wavefileSpecification.samplingRate = BitConverter.ToInt32(m_buffer, 24);
            wavefileSpecification.byteRate = BitConverter.ToInt32(m_buffer, 28);
            wavefileSpecification.blockAlign = BitConverter.ToInt16(m_buffer, 32);
            wavefileSpecification.bitsPerSample = BitConverter.ToInt16(m_buffer, 34);
            wavefileSpecification.subChunk2Id = BitConverter.ToInt32(m_buffer, 36);
            wavefileSpecification.subChunk2Size = BitConverter.ToInt32(m_buffer, 40);

            m_canRead = true;

            return true;
        }

        public void Close()
        {
            m_canRead = false;
            stream.Close();
        }

        public void Dispose()
        {
            m_canRead = false;
            stream.Close();
            stream.Dispose();
        }

        public void ResetPosition()
        {
            currentChunk = 0;
            stream.Seek(44, SeekOrigin.Begin);
        }

        public long Read()
        {
            if (!m_canRead)
                return 0;

            if (currentChunk > streamInfo.totalChunks)
                return 0;

            if (currentChunk == streamInfo.totalChunks - 1)
                currentChunkSize = streamInfo.lastChunkSize;
            else
                currentChunkSize = streamInfo.chunkSize;

            if (stream.CanRead)
            {
                int bytesRead = stream.Read(m_buffer, 0, (int)currentChunkSize);                

                if (onRead != null)
                    onRead(m_buffer, (int)currentChunkSize);

                currentChunk++;

                if (currentChunk > streamInfo.totalChunks)
                {
                    if (onReadFinished != null)
                        onReadFinished();
                }

                return bytesRead;
            }

            return 0;
        }
    }
}
