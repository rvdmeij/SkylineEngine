using System;
using System.Collections.Generic;

namespace SkylineEngine
{
    public static class Resources
    {
        public static void LoadAll<T>(List<string> resources)
        {
            if(typeof(T) == typeof(Texture))
            {
                for(int i = 0; i < resources.Count; i++)
                {
                    TextureManager.Load(resources[i]);
                }
            }
            else if(typeof(T) == typeof(Shader))
            {
                for(int i = 0; i < resources.Count; i++)
                {
                    ShaderManager.Load(resources[i]);
                }
            }
        }

        public static Shader LoadShader(string vertexSource, string fragmentSource, string name)
        {
            int id = ShaderManager.Load(vertexSource, fragmentSource, name);

            if( id > 0)
            {
                return ShaderManager.GetShader(id);
            }

            return null;
        }

        public static Texture LoadTexture(int width, int height, byte[] data, string name)
        {
            int id = TextureManager.Load(width, height, data, name);

            if(id > 0)
            {
                return TextureManager.GetTexture(id);
            }
            
            return null;
        }

        public static T Load<T>(string resourcePath) where T : Resource
        {
            Type type = typeof(T);

            if (type == typeof(Texture))
            {
                int id = TextureManager.Load(resourcePath, resourcePath);

                if (id > 0)
                {
                    var resource = TextureManager.GetTexture(id);
                    return resource as T;
                }
            }
            else if(type == typeof(Shader))
            {
                int id = ShaderManager.Load(resourcePath, resourcePath);

                if( id > 0)
                {
                    var resource = ShaderManager.GetShader(id);
                    return resource as T;
                }
            }

            return null;
        }
    }
}
