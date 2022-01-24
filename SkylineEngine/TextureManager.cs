using System.Collections.Generic;

namespace SkylineEngine
{
    public static class TextureManager
    {
        private static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public static int Load(string filepath)
        {
            return Load(filepath, filepath);
        }

        public static int Load(string filepath, string name)
        {
            if(!textures.ContainsKey(name))
            {
                textures[name] = new Texture(filepath);
                int textureID = textures[name].textureId;
                Debug.Log("Loaded texture " + filepath + " with ID " +textureID);
                return textureID;
            }
            else
            {
                return GetTexture(name).textureId;
            }
        }

        public static int Load(int width, int height, string name)
        {
            if(!textures.ContainsKey(name))
            {
                textures[name] = new Texture(width, height);
                int textureID = textures[name].textureId;
                Debug.Log("Loaded texture " + name + " with ID " + textureID);
                return textureID;
            }
            else
            {
                return GetTexture(name).textureId;
            }
        }

        public static int Load(int width, int height, byte[] data, string name)
        {
            if(!textures.ContainsKey(name))
            {
                textures[name] = new Texture(width, height, data, name);
                int textureID = textures[name].textureId;
                Debug.Log("Loaded texture " + name + " with ID " + textureID);
                return textureID;
            }
            else
            {
                return GetTexture(name).textureId;
            }
        }

        public static void Add(Texture texture, string name)
        {
            if(!textures.ContainsKey(name))
            {
                textures.Add(name, texture);
            }
        }

        public static Texture GetTexture(string name)
        {
            if(textures.ContainsKey(name))
            {
                return textures[name];
            }
            return null;
        }

        public static Texture GetTexture(int textureId)
        {
            Texture texture = null;
            foreach (var item in textures)
            {
                if (item.Value.textureId == textureId)
                {
                    texture = item.Value;
                    break;
                }
            }
            return texture;
        }
    }
}
