namespace SkylineEngine.Shaders
{
    public class SkyboxShader
    {
        public const string vertex =
@"#version 330 core
layout (location = 0) in vec3 aPos;

out vec3 TexCoords;

uniform mat4 u_Projection;
uniform mat4 u_View;
uniform mat4 u_Model;
uniform vec3 u_CamPosition;

void main()
{
    TexCoords = aPos;
    gl_Position = u_Projection * u_View * vec4(aPos, 1.0);
}";

        public const string fragment = 
@"#version 330 core

out vec4 FragColor;

in vec3 TexCoords;

uniform samplerCube u_DiffuseTexture;
uniform vec3 u_DiffuseColor;
uniform vec3 u_SkyColor;

const float lowerLimit = 0.0;
const float upperLimit = 0.1;

void main()
{    
    vec3 uv = TexCoords;

    vec4 fogColor = vec4(u_SkyColor, 0);

    vec4 finalColor = texture(u_DiffuseTexture, TexCoords) * vec4(u_DiffuseColor, 1.0);

    float factor = (uv.y - lowerLimit) / (upperLimit - lowerLimit);

    factor = clamp(factor, 0.0, 1.0);

    FragColor = mix(fogColor, finalColor, factor);
}";
    }
}