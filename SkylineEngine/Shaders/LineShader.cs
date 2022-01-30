namespace SkylineEngine.Shaders
{
    public class LineShader
    {
        public const string vertex = 
@"#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 color;

uniform mat4 u_Model;
uniform mat4 u_View;
uniform mat4 u_Projection;

out vec3 lineColor;

void main()
{
    lineColor = color;
    gl_Position  = u_Projection * u_View * u_Model * vec4(position, 1.0);
}";

        public const string fragment = 
@"#version 330 core

out vec4 color;
in vec3 lineColor;

void main()
{
    color = vec4(lineColor, 1.0);
}";
    }
}