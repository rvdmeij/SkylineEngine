using System.Collections.Generic;

namespace SkylineEngine
{
    public static class TextureManager
    {
        private static Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public static uint Load(string filepath)
        {
            return Load(filepath, filepath);
        }

        public static uint Load(string filepath, string name)
        {
            if(!textures.ContainsKey(name))
            {
                textures[name] = new Texture(filepath);
                uint textureID = textures[name].textureId;
                Debug.Log("Loaded " + filepath + " with ID " +textureID);
                return textureID;
            }
            else
            {
                return GetTexture(name).textureId;
            }
        }

        public static uint Load(int width, int height, string name)
        {
            if(!textures.ContainsKey(name))
            {
                textures[name] = new Texture(width, height);
                uint textureID = textures[name].textureId;
                Debug.Log("Loaded " + name + " with ID " + textureID);
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

        public static Texture GetTexture(uint textureId)
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
