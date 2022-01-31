namespace SkylineEngine.Shaders
{
    public class ProceduralSkyboxShader
    {
        public const string vertex =
@"#version 330 core
layout (location = 0) in vec3 aPos;

uniform mat4 u_Projection;
uniform mat4 u_View;
uniform mat4 u_Model;

out vec3 worldPosition;

void main()
{
    worldPosition = aPos;
    gl_Position = u_Projection * u_View * vec4(aPos, 1.0);
}";

    public const string fragment = 
@"#version 330 core

out vec4 FragColor;

in vec3 worldPosition;

uniform samplerCube u_DiffuseTexture;
uniform vec3 u_DiffuseColor;
uniform vec3 u_SkyColor;

void main()
{    
    vec3 pointOnSphere = normalize(worldPosition);
    float a = pointOnSphere.y;
    FragColor = vec4(mix(u_DiffuseColor, u_SkyColor, a), 1.0);
}";
    }
}
