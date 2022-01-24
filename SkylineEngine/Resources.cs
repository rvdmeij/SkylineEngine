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

        public static T Load<T>(string resourcePath) where T : Resource
        {
            Type type = typeof(T);

            if (type == typeof(Texture))
            {
                uint id = TextureManager.Load(resourcePath, resourcePath);

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
