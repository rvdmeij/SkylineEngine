using System.Collections.Generic;

namespace SkylineEngine
{
    internal static class AudioSourceManager
    {
        private static List<AudioSource> audioSources = new List<AudioSource>();
        private static Queue<int> destroyQueue = new Queue<int>();

        public static void Register(AudioSource source)
        {
            audioSources.Add(source);
        }

        public static void Unregister(int instanceId)
        {
            AddToDestroyQueue(instanceId);
        }

        internal static void AddToDestroyQueue(int instanceId)
        {
            destroyQueue.Enqueue(instanceId);
        }

        internal static void UpdateDestroyQueue()
        {
            if (destroyQueue.Count > 0)
            {
                int count = destroyQueue.Count;

                for (int i = 0; i < count; i++)
                {
                    int instanceId = destroyQueue.Dequeue();
                    Destroy(instanceId);
                }
            }
        }

        internal static void Destroy(int instanceId)
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (audioSources[i].InstanceId == instanceId)
                {
                    audioSources[i].Dispose();
                    audioSources.RemoveAt(i);
                    break;
                }
            }
        }

        internal static void Dispose() 
        {
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (audioSources[i] != null)
                {
                    audioSources[i].Dispose();
                }
            }
        }
    }
}
