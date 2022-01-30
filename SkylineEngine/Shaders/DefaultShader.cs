namespace SkylineEngine.Shaders
{
    public class DefaultShader
    {
        public const string vertex = 
@"#version 330 core

layout (location = 0) in vec3 position;
layout (location = 2) in vec2 uv;
layout (location = 3) in vec3 a_normal;

in vec3 normal;

out vec3 Normal;
out vec3 FragPosition;
out vec3 LightDirection;
out vec3 LightPosition;
out vec2 TexCoord0;
out float Visibility;

uniform mat4 u_Model;
uniform mat4 u_View;
uniform mat4 u_Projection;
uniform vec3 u_LightDirection;
uniform vec3 u_LightPosition;
uniform vec2 u_UVScale;
uniform vec4 u_clippingPlane;

uniform float u_FogDensity;
uniform float u_FogGradient;


void main()
{
    vec4 worldPosition = u_Projection * u_View * u_Model * vec4(position, 1.0);
    vec4 positionRelativeToCam = u_View * worldPosition;

    //gl_ClipDistance[0] = dot(worldPosition, u_clippingPlane);

    LightDirection  = normalize(u_LightDirection);
    LightPosition = u_LightPosition;
    FragPosition    = vec3(position);
    Normal          = a_normal;
       
    gl_Position = u_Projection * u_View * u_Model * vec4(position, 1.0);
    TexCoord0 = uv * u_UVScale;

    float distance = length(positionRelativeToCam.xyz);
    Visibility = exp(-pow((distance*u_FogDensity), u_FogGradient));
    Visibility = clamp(Visibility, 0.0, 1.0);
}";

        public const string fragment = 
@"#version 330 core

in vec3 Normal;
in vec3 FragPosition;
in vec3 LightDirection;
in vec3 LightPosition;
in vec2 TexCoord0;
in float Visibility;

uniform vec4 u_AmbientColor;
uniform vec4 u_DiffuseColor;
uniform vec4 u_SpecularColor;
uniform vec4 u_SkyColor;
uniform float u_SpecularStrength;
uniform sampler2D u_Texture0;

out vec4 outColor;

vec4 light_color = vec4( 1.0,  1.0,  1.0, 1.0);

void main()
{
    vec3  norm = normalize(Normal);

    // ambient
    float ambient_strength = 0.8;
    vec4  ambient = ambient_strength * light_color * u_AmbientColor;  // ambient strength = 0.1

    // diffuse
    float diffuse_strength = 0.8;
    vec4  diffuse = diffuse_strength * max(dot(norm, LightDirection), 0.0) * light_color * u_DiffuseColor;

    //specular
    float specular_strength = 0.5;
    vec3  view_dir = normalize(-FragPosition + LightPosition);
    vec3  reflect_dir = reflect(-LightDirection, norm);
    float spec = pow(max(dot(view_dir, reflect_dir), 0.0), specular_strength);
    vec4  specular = specular_strength * light_color * (spec * u_SpecularColor);

    vec4  final_color = ambient + diffuse + specular;

    outColor = final_color * texture2D(u_Texture0, TexCoord0);

    outColor = mix(u_DiffuseColor, outColor, Visibility);    
}";
    }
}