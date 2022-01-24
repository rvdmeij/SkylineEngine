using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SDL2;

namespace SkylineEngine
{
    public class AudioClip
    {
        private short[] data;
        private int channels;
        private int sampleRate;
        private string name;
        private static List<AudioClip> clips = new List<AudioClip>();
        private string filePath;

        public static List<AudioClip> Clips
        {
            get { return clips; }
            set { clips = value; }
        }

        public short[] Data
        {
            get
            {
                return this.data;
            }
        }

        public int Channels
        {
            get { return channels; }
        }

        public int SampleRate
        {
            get { return sampleRate; }
        }

        public long Samples
        {
            get { return data.Length; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public string FilePath
        {
            get { return filePath; }
        }

        public AudioClip()
        {

        }

        public AudioClip(string filepath, bool streamFromDisk = true)
        {
            this.filePath = filepath;

            if(!streamFromDisk)
                Load(filepath);
        }

        public AudioClip(short[] data)
        {
            this.SetData(data);
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetClip(string filepath)
        {
            if (!filepath.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                return;

            Load(filepath);
        }

        public static AudioClip LoadFromPath(string filepath)
        {
            for (int i = 0; i < clips.Count; i++)
            {
                if (clips[i].Name == filepath)
                    return clips[i];
            }

            AudioClip clip = new AudioClip(filepath);
            if (clip.Data.Length > 0)
            {
                clips.Add(clip);
                int index = clips.Count - 1;
                return clips[index];
            }
            return null;
        }

        public void Load(string filepath)
        {
            SDL.SDL_AudioSpec spec = new SDL.SDL_AudioSpec();
            IntPtr ptr;
            uint length;
            SDL.SDL_LoadWAV(filepath, out spec, out ptr, out length);

            if (ptr != IntPtr.Zero)
            {
                byte[] bytes = new byte[length];
                Marshal.Copy(ptr, bytes, 0, (int)length);
                this.data = new short[(int)(length / 2)];
                Buffer.BlockCopy(bytes, 0, data, 0, (int)length);
                SetName(filepath);
                this.channels = (int)spec.channels;
                this.sampleRate = spec.freq;
                SDL.SDL_FreeWAV(ptr);
            }
        }

        public void SetData(short[] data)
        {
            this.data = data;
        }

        public float[] GetDataAsFloat()
        {
            float[] d = new float[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 0)
                    d[i] = 0;
                else
                    d[i] = (float)(data[i] / 32767f);
            }
            return d;
        }

        public double[] GetDataAsDouble()
        {
            double[] d = new double[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 0)
                    d[i] = 0;
                else
                    d[i] = (double)(data[i] / 32767f);
            }
            return d;
        }
    }
}
