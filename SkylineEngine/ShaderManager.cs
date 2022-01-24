using System.Collections.Generic;

namespace SkylineEngine
{
    public static class ShaderManager
    {
        private static Dictionary<string, Shader> shaders = new Dictionary<string, Shader>();

        public static int Load(string filepath)
        {
            return Load(filepath, filepath);
        }

        public static int Load(string filepath, string name)
        {
            if(!shaders.ContainsKey(name))
            {
                shaders[name] = new Shader(filepath);
                int shaderID = shaders[name].program;
                Debug.Log("Loaded " + filepath + " with ID " + shaderID);
                return shaderID;
            }
            else
            {
                return GetShader(name).program;
            }
        }

        public static Shader GetShader(string name)
        {
            if(shaders.ContainsKey(name))
            {
                Shader shader = shaders[name];
                return shader;
            }
            return null;
        }

        public static Shader GetShader(int shaderId)
        {
            Shader shader = null;
            foreach (var item in shaders)
            {
                if (item.Value.program == shaderId)
                {
                    shader = item.Value;
                    break;
                }
            }
            return shader;
        }
    }
}
