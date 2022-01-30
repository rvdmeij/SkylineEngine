using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SkylineEngine
{
    public enum UniformType
    {
        BOOL,
        FLOAT,
        INT,
        MAT3,
        MAT4,
        SAMPLER2D,
        SAMPLERCUBE,
        VEC2,
        VEC3,
        VEC4
    }

    public class UniformInfo
    {
        public string name;
        public int location;
        public UniformType type;
    }

    public struct ShaderProgramSource
    {
        public string vertexSource;
        public string fragmentSource;
    }

    public enum ShaderType
    {
        NONE     = -1, 
        VERTEX   =  0, 
        FRAGMENT =  1
    }

    public class Shader : Resource
    {
        private int m_program;
        private int[] m_shaders = new int[2];
        private Dictionary<string, UniformInfo> uniforms = new Dictionary<string, UniformInfo>();

        public Dictionary<string,UniformInfo> Uniforms
        {
            get{ return uniforms; }
        }

        public int program { get { return m_program; } }

        public Shader(string vertexSource, string fragmentSource)
        {
            m_program = GL.CreateProgram();            

            m_shaders[0] = CreateShader(vertexSource, OpenTK.Graphics.OpenGL.ShaderType.VertexShader);
            m_shaders[1] = CreateShader(fragmentSource, OpenTK.Graphics.OpenGL.ShaderType.FragmentShader);

            for (int i = 0; i < m_shaders.Length; i++)
            {
                GL.AttachShader(m_program, m_shaders[i]);                
            }

            GL.LinkProgram(m_program);
            if(!CheckShaderError((uint)m_program, GetProgramParameterName.LinkStatus, true))
                Debug.Log("Shader linking error");

            GL.ValidateProgram(m_program);
            if(!CheckShaderError((uint)m_program, GetProgramParameterName.ValidateStatus, true))
                Debug.Log("Shader validation error");

            for (int i = 0; i < m_shaders.Length; i++)
            {
                GL.DetachShader(m_program, m_shaders[i]);                
                GL.DeleteShader(m_shaders[i]);                
            }

            List<UniformInfo> uniformsV = GetUniformsFromShader(vertexSource, m_program);
            List<UniformInfo> uniformsF = GetUniformsFromShader(fragmentSource, m_program);

            for (int i = 0; i < uniformsV.Count; i++)
            {
                uniforms[uniformsV[i].name] = uniformsV[i];
            }

            for (int i = 0; i < uniformsF.Count; i++)
            {
                uniforms[uniformsF[i].name] = uniformsF[i];
            }
        }

        public Shader(string filename)
        {
            m_program = GL.CreateProgram();            

            ShaderProgramSource source = LoadShader(filename);

            m_shaders[0] = CreateShader(source.vertexSource, OpenTK.Graphics.OpenGL.ShaderType.VertexShader);
            m_shaders[1] = CreateShader(source.fragmentSource, OpenTK.Graphics.OpenGL.ShaderType.FragmentShader);

            for (int i = 0; i < m_shaders.Length; i++)
            {
                GL.AttachShader(m_program, m_shaders[i]);                
            }

            GL.LinkProgram(m_program);
            if(!CheckShaderError((uint)m_program, GetProgramParameterName.LinkStatus, true))
                Debug.Log("Shader linking error");

            GL.ValidateProgram(m_program);
            if(!CheckShaderError((uint)m_program, GetProgramParameterName.ValidateStatus, true))
                Debug.Log("Shader validation error");


            for (int i = 0; i < m_shaders.Length; i++)
            {
                GL.DetachShader(m_program, m_shaders[i]);                
                GL.DeleteShader(m_shaders[i]);
            }

            List<UniformInfo> uniformsV = GetUniformsFromShader(source.vertexSource, m_program);
            List<UniformInfo> uniformsF = GetUniformsFromShader(source.fragmentSource, m_program);

            for (int i = 0; i < uniformsV.Count; i++)
            {
                uniforms[uniformsV[i].name] = uniformsV[i];
            }

            for (int i = 0; i < uniformsF.Count; i++)
            {
                uniforms[uniformsF[i].name] = uniformsF[i];
            }
        }

        public void Dispose()
        {
            GL.DeleteProgram(m_program);            
        }

        public static Shader CreateShader(string filename)
        {
            Shader shader = new Shader(filename);
            return shader;
        }

        public static int CreateShader(string sourceCode, OpenTK.Graphics.OpenGL.ShaderType shaderType)
        {
            int shader = GL.CreateShader(shaderType);            

            if (shader == 0)
                Debug.Log("Error: shader creation failed");

            string[] shaderSourceStrings = new string[1];
            int[] shaderSourceStringsLengths = new int[1];
            int numberOfSources = 1;

            shaderSourceStrings[0] = sourceCode;
            shaderSourceStringsLengths[0] = sourceCode.Length;

            GL.ShaderSource(shader, numberOfSources, shaderSourceStrings, shaderSourceStringsLengths);
            GL.CompileShader(shader);            

            string infoLogVert = GL.GetShaderInfoLog(shader);
            if (infoLogVert != System.String.Empty)
                System.Console.WriteLine(infoLogVert);

            return shader;
        }


        public static ShaderProgramSource LoadShader(string filename)
        {
            ShaderType type = ShaderType.NONE;
            string[] lines = System.IO.File.ReadAllLines(filename);
            string fragmentShader = string.Empty;
            string vertexShader = string.Empty;       

            for(int i = 0; i < lines.Length; i++)
            {
                if(lines[i].Contains("#shader"))
                {
                    if(lines[i].Contains("vertex"))
                        type = ShaderType.VERTEX;
                    else if(lines[i].Contains("fragment"))
                        type = ShaderType.FRAGMENT;
                }
                else
                {
                    if(type == ShaderType.FRAGMENT)
                        fragmentShader += lines[i] + "\n";
                    else if(type == ShaderType.VERTEX)
                        vertexShader += lines[i] + "\n";
                }
            }

            ShaderProgramSource sps = new ShaderProgramSource();
            sps.fragmentSource = fragmentShader;
            sps.vertexSource = vertexShader;
            return sps;
        }
        
        public bool CheckShaderError(uint shaderProgram, GetProgramParameterName flag, bool isProgram)
        {
            int success = 0;        
            
            if(isProgram)
                GL.GetProgram(shaderProgram, flag, out success);
            else
                GL.GetShader((int)shaderProgram, ShaderParameter.CompileStatus, out success);

            if(success == 0)
            {
                string errorMessage;

                if(isProgram)
                    GL.GetProgramInfoLog((int)shaderProgram, out errorMessage);
                else
                    GL.GetShaderInfoLog((int)shaderProgram, out errorMessage);
                    
                Debug.Log(errorMessage);
                return false;
            }
            return true;
        }        

        public static List<UniformInfo> GetUniformsFromShader(string shaderSource, int program)
        {
            List<UniformInfo> uniforms = new List<UniformInfo>();

            char[] separators = { '\r', '\n' };

            string[] lines = shaderSource.Split(separators);

            for(int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                line = line.Replace(";", "");

                string[] components = line.Split(' ');

                if (components.Length == 3)
                {
                    if (components[0] == "uniform")
                    {
                        UniformType type = UniformType.INT;

                        if (components[1] == "bool")
                            type = UniformType.BOOL;
                        else if (components[1] == "int")
                            type = UniformType.INT;
                        else if (components[1] == "float")
                            type = UniformType.FLOAT;
                        else if (components[1] == "vec2")
                            type = UniformType.VEC2;
                        else if (components[1] == "vec3")
                            type = UniformType.VEC3;
                        else if (components[1] == "vec4")
                            type = UniformType.VEC4;
                        else if (components[1] == "mat3")
                            type = UniformType.MAT3;
                        else if (components[1] == "mat4")
                            type = UniformType.MAT4;
                        else if (components[1] == "sampler2D")
                            type = UniformType.SAMPLER2D;
                        else if (components[1] == "samplerCube")
                            type = UniformType.SAMPLERCUBE;

                        UniformInfo info = new UniformInfo();
                        info.name = components[2];
                        info.type = type;
                        info.location = GL.GetUniformLocation(program, info.name);
                        
                        uniforms.Add(info);
                    }
                }
            }
            return uniforms;
        }

        public void Bind()
        {
            if(m_program > 0)
                GL.UseProgram(m_program);
        }

        public UniformInfo GetUniform(string name)
        {
            if (uniforms.ContainsKey(name))
            {
                return uniforms[name];
            }
            return null;
        }

        public int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(m_program, name);
        }

        public void SetFloat(string name, float value)
        {
            GL.Uniform1(GL.GetUniformLocation(m_program, name), value);
        }

        public void SetFloat2(string name, float[] value)
        {
            GL.Uniform2(GL.GetUniformLocation(m_program, name), value.Length, value);
        }

        public void SetFloat2(string name, Vector2 value)
        {
            GL.Uniform2(GL.GetUniformLocation(m_program, name), value.x, value.y);
        }

        public void SetFloat2(string name, float x, float y)
        {
            GL.Uniform2(GL.GetUniformLocation(m_program, name), x, y);
        }

        public void SetFloat3(string name, float[] value)
        {
            GL.Uniform3(GL.GetUniformLocation(m_program, name), value.Length, value);
        }

        public void SetFloat3(string name, Vector3 value)
        {
            GL.Uniform3(GL.GetUniformLocation(m_program, name), value.x, value.y, value.z);
        }

        public void SetFloat3(string name, float x, float y, float z)
        {
            GL.Uniform3(GL.GetUniformLocation(m_program, name), x, y, z);
        }

        public void SetFloat4(string name, float[] value)
        {
            GL.Uniform4(GL.GetUniformLocation(m_program, name), value.Length, value);
        }

        public void SetFloat4(string name, Color32 value)
        {
            GL.Uniform4(GL.GetUniformLocation(m_program, name), value.r, value.g, value.b, value.a);
        }

        public void SetFloat4(string name, float x, float y, float z, float w)
        {
            GL.Uniform4(GL.GetUniformLocation(m_program, name), x, y, z, w);
        }

        public void SetFloat4(string name, Vector4 value)
        {
            GL.Uniform4(GL.GetUniformLocation(m_program, name), value.x, value.y, value.z, value.w);
        }

        public void SetInt(string name, int value)
        {
            GL.Uniform1(GL.GetUniformLocation(m_program, name), value);
        }

        public void SetMat4(string name, Matrix4 value)
        {
            GL.UniformMatrix4(GL.GetUniformLocation(m_program, name), false, ref value);
        }

        public void SetMat3(string name, Matrix3 value)
        {
            GL.UniformMatrix3(GL.GetUniformLocation(m_program, name), false, ref value);
        }

        public void SetBool(string name, bool value)
        {
            int val = value == false ? 0 : 1;
            GL.Uniform1(GL.GetUniformLocation(m_program, name), val);
        }

        public void SetFloat(int location, float value)
        {
            GL.Uniform1(location, value);
        }

        public void SetFloat2(int location, float[] value)
        {
            GL.Uniform2(location, value.Length, value);
        }

        public void SetFloat2(int location, Vector2 value)
        {
            GL.Uniform2(location, value.x, value.y);
        }

        public void SetFloat3(int location, float[] value)
        {
            GL.Uniform3(location, value.Length, value);
        }

        public void SetFloat3(int location, Vector3 value)
        {
            GL.Uniform3(location, value.x, value.y, value.z);
        }

        public void SetFloat4(int location, float[] value)
        {
            GL.Uniform4(location, value.Length, value);
        }

        public void SetFloat4(int location, Vector4 value)
        {
            GL.Uniform4(location, value.x, value.y, value.z, value.w);
        }

        public void SetFloat4(int location, Color32 value)
        {
            GL.Uniform4(location, value.r, value.g, value.b, value.a);
        }

        public void SetInt(int location, int value)
        {
            GL.Uniform1(location, value);
        }

        public void SetMat4(int location, Matrix4 value)
        {
            GL.UniformMatrix4(location, false, ref value);
        }

        public void SetMat3(int location, Matrix3 value)
        {
            GL.UniformMatrix3(location, false, ref value);
        }
    }
}
