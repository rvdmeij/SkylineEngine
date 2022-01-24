using System;
using System.Collections.Generic;

namespace SkylineEngine
{
    public static class Resources
    {
        public static void LoadAll<T>()
        {
            if(typeof(T) == typeof(Texture))
            {
                TextureManager.Load("res/Textures/Beach.jpg");
                TextureManager.Load("res/Textures/Grass.jpg");
                TextureManager.Load("res/Textures/Grass3.jpg");
                TextureManager.Load("res/Textures/GrassFlowers.png");
                TextureManager.Load("res/Textures/Default.png");
                TextureManager.Load("res/Textures/Mud.png");
                TextureManager.Load("res/Textures/SplatMap.jpg");
                //TextureManager.Load("res/Textures/Road.jpg");
                //TextureManager.Load("res/Textures/Road2.jpg");
                TextureManager.Load("res/Textures/Box.jpg");
                //TextureManager.Load("res/Textures/Moon.bmp");
                //TextureManager.Load("res/Textures/Water6.png");
                TextureManager.Load("res/Textures/Sun.jpg");
                //TextureManager.Load("res/Textures/Trees/PalmTree1.png");
                //TextureManager.Load("res/Textures/Trees/PalmTree2.png");
                //TextureManager.Load("res/Textures/Trees/PalmTree3.png");
                //TextureManager.Load("res/Textures/Trees/PalmTree4.png");
                //TextureManager.Load("res/Textures/Trees/Tree1.png");
                //TextureManager.Load("res/Textures/Trees/Tree2.png");
                //TextureManager.Load("res/Textures/Trees/Bush1.png");
                //TextureManager.Load("res/Textures/Trees/RedFlower.png");
                //TextureManager.Load("res/Textures/Trees/YellowFlower.png");
            }
            else if(typeof(T) == typeof(Shader))
            {
                ShaderManager.Load("res/Shaders/Skybox.shader");
                ShaderManager.Load("res/Shaders/Terrain.shader");
                ShaderManager.Load("res/Shaders/Default.shader");
                ShaderManager.Load("res/Shaders/Particle.shader");
                return;
            }
        }

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
