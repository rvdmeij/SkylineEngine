using System;
using System.Runtime.InteropServices;
using SDL2;
using SkylineEngine.Audio;

namespace SkylineEngine
{
    public delegate void AudioReadEvent(float[] buffer, int channels);
    public delegate void PlaybackEndedEvent();

    public class AudioSource : Component
    {
        private SDL.SDL_AudioSpec specDesired;
        private SDL.SDL_AudioSpec specObtained;
        private int device = 0;
        private bool isPlaying;
        private GCHandle hInstance;
        private int status = 1;
        private int channels = 2;
        private int sampleRate;
        private short[] audioBuffer;
        private float[] audioBufferCopy;
        private ushort bufferSize;
        private long playbackTime;
        private bool loop;
        private WaveFileReader wavefileReader;
        private float volume;

        public event AudioReadEvent onAudioRead;
        public event PlaybackEndedEvent onPlaybackEnded;

        public bool IsPlaying
        {
            get { return isPlaying; }
        }

        public long PlaybackTime 
        { 
            get { return playbackTime; } 
        }

        public bool IsLooping
        {
            get { return loop; }
            set { loop = value; }
        }

        public float Volume
        {
            get { return volume; }
            set 
            { 
                if(value < 0)
                    volume = 0;
                else
                    volume = value; 
            }
        }

        public int Channels
        {
            get
            {
                return channels;
            }
            // set
            // {
            //     value = Mathf.Clamp(value, 1, 2);
            //     channels = value;
            //     Reinitialize();
            // }
        }

        private static int audioSourceCount = 0;

        public AudioSource()
        {
            this.channels = 2;
            this.sampleRate = AudioSettings.outputSampleRate;
            this.bufferSize = (ushort)AudioSettings.outputBufferSize;
            this.audioBuffer = new short[bufferSize];
            this.audioBufferCopy = new float[bufferSize];
            this.volume = 1.0f;
            this.wavefileReader = new WaveFileReader();
            this.wavefileReader.onRead += WavefileReader_OnRead;
            this.wavefileReader.onReadFinished += WavefileReader_OnReadFinished;
            Initialize();
        }

        ~AudioSource()
        {
            Dispose();
        }

        private void Initialize()
        {
            hInstance = GCHandle.Alloc(this);
            IntPtr userdata = GCHandle.ToIntPtr(hInstance);

            SDL.SDL_AudioCallback callback = new SDL.SDL_AudioCallback(OnAudioRead);
            specDesired.freq = sampleRate;
            specDesired.channels = Convert.ToByte(channels); //channels is stored as bytes
            specDesired.samples = bufferSize;
            specDesired.userdata = userdata;
            specDesired.format = SDL.AUDIO_S16;
            specDesired.callback = callback;

            device = SDL.SDL_InitSubSystem(SDL.SDL_INIT_AUDIO);
            status = SDL.SDL_OpenAudio(ref specDesired, out specObtained);
            Console.WriteLine("Audio status: " + status);

            audioSourceCount++;
        }

        private void Reinitialize()
        {
            Stop();
            SDL.SDL_CloseAudio();
            Debug.Log("Audio status: " + status);

            IntPtr userdata = GCHandle.ToIntPtr(hInstance);

            SDL.SDL_AudioCallback callback = new SDL.SDL_AudioCallback(OnAudioRead);
            specDesired.freq = sampleRate;
            specDesired.channels = Convert.ToByte(channels); //channels is stored as bytes
            specDesired.samples = bufferSize;
            specDesired.userdata = userdata;
            specDesired.format = SDL.AUDIO_S16;
            specDesired.callback = callback;

            device = SDL.SDL_InitSubSystem(SDL.SDL_INIT_AUDIO);
            status = SDL.SDL_OpenAudio(ref specDesired, out specObtained);
            Console.WriteLine("Audio status: " + status);
        }

        public void Play()
        {
            if (!isPlaying)
            {
                SDL.SDL_PauseAudio(0);
                isPlaying = true;
            }
        }

        public void PlayOneShot(AudioClip clip)
        {
            if (!isPlaying)
            {
                if (wavefileReader.Open(clip.FilePath))
                {
                    SDL.SDL_PauseAudio(0);
                    isPlaying = true;
                }
                else
                {
                    Debug.Log("Couldn't open file");
                }
            }

        }

        public void Stop()
        {
            if (isPlaying)
            {
                SDL.SDL_PauseAudio(1);
                isPlaying = false;
                playbackTime = 0;
                wavefileReader.Close();
            }
        }

        public void GetOutputData(float[] data)
        {
            if(!IsPlaying)
                return;

            for(int i = 0; i < audioBufferCopy.Length; i++)
            {
                data[i] = audioBufferCopy[i];
            }
        }

        private void OnAudioRead(IntPtr userdata, IntPtr stream, int len)
        {
            if (onAudioRead != null)
            {
                //onAudioRead(audioBuffer, channels);
            }
            else
            {
                long bytesRead = wavefileReader.Read();

                if(bytesRead > 0)
                {
                    playbackTime += (bytesRead / sizeof(Int16));
                }
                else
                {
                    for (int x = 0; x < audioBuffer.Length; x += channels)
                    {
                        audioBuffer[x] = 0;

                        if (channels == 2)
                            audioBuffer[x + 1] = 0;
                    }
                }
            }

            float t = 1.0f / ushort.MaxValue;

            for(int i = 0; i < audioBuffer.Length; i++)
            {
                audioBuffer[i] = (short)(volume * audioBuffer[i]);
                
                audioBufferCopy[i] = t * audioBuffer[i];
            }

            Marshal.Copy(audioBuffer, 0, stream, audioBuffer.Length);
        }

        void WavefileReader_OnRead(byte[] bytes, int length)
        {
            Buffer.BlockCopy(bytes, 0, audioBuffer, 0, length);
        }

        void WavefileReader_OnReadFinished()
        {
            playbackTime = 0;

            if (loop)
            {
                wavefileReader.ResetPosition();
            }
            else
            {
                Stop();
                wavefileReader.Close();
                onPlaybackEnded?.Invoke();
            }
        }

        public void Dispose()
        {
            Stop();
            SDL.SDL_CloseAudio();
            Debug.Log("Audio status: " + status);

            if (hInstance.IsAllocated)
            {
                hInstance.Free();
                GC.SuppressFinalize(this);
            }

            audioSourceCount--;

            wavefileReader.Dispose();
        }
    }
}
