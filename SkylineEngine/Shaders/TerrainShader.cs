namespace SkylineEngine.Shaders
{
    public class TerrainShader
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
uniform vec4 u_clippingPlane;

uniform float u_FogDensity;
uniform float u_FogGradient;

void main()
{
    vec4 worldPosition = u_Projection * u_View * u_Model * vec4(position, 1.0);
    vec4 positionRelativeToCam = u_View * worldPosition;

    //gl_ClipDistance[0] = dot(worldPosition, u_clippingPlane);
    //gl_ClipDistance[0] = 1;

    LightDirection  = normalize(u_LightDirection);
    LightPosition = u_LightPosition;
    FragPosition    = vec3(position);
    Normal          = a_normal;
    gl_Position     = u_Projection * u_View * u_Model * vec4(position, 1.0);
    TexCoord0 = uv;

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
uniform sampler2D u_Texture0;//splatmap
uniform sampler2D u_Texture1;
uniform sampler2D u_Texture2;
uniform sampler2D u_Texture3;
uniform sampler2D u_Texture4;
uniform int u_ShowGrid;
uniform int u_ShowCircle;
uniform vec2 u_GridUV;
uniform vec3 u_MouseWorldSpace;
uniform float u_CircleRadius;
out vec4 outColor;

vec4 light_color = vec4( 1.0,  1.0,  1.0, 1.0);

float grid(vec2 st, float res)
{
  vec2 grid = fract(st*res);
  return (step(res,grid.x) * step(res,grid.y));
}

float Circle(vec2 uv, vec2 p, float r, float blur)
{
	float d = length(uv - p);
	float c = smoothstep(r, r - blur, d);
	return c;
}

vec4 CreateColor()
{
    vec3  norm = normalize(Normal);

    // ambient
    float ambient_strength = 0.5;
    vec4  ambient = ambient_strength * light_color * u_AmbientColor;  // ambient strength = 0.1

    // diffuse
    float diffuse_strength = 0.8;
    vec4  diffuse = diffuse_strength * max(dot(norm, LightDirection), 0.0) * light_color * u_DiffuseColor;

    //specular
    float specular_strength = 0.2;
    vec3  view_dir = normalize(-FragPosition + LightPosition);
    vec3  reflect_dir = reflect(-LightDirection, norm);
    float spec = pow(max(dot(view_dir, reflect_dir), 0.0), u_SpecularStrength);
    vec4  specular = specular_strength * light_color * (spec * u_SpecularColor);

    vec4  final_color = ambient + diffuse + specular;
    return final_color;    
}

vec4 CreateCircle()
{
    vec4 color = vec4(0);
    float dist = length(FragPosition.xz - u_MouseWorldSpace.xz);

    if(dist <= u_CircleRadius)
    {
        if(dist >= (u_CircleRadius * 0.99))
        {
            color = vec4(0, 1, 0, 1);
        }
        else
        {
            color = vec4(0.1, 0.1, 0.0, 1.0);
        }
    }

    return color;
}

void main()
{
    vec4 blendMapColor = texture(u_Texture0, TexCoord0);
    float backgroundTextureAmount = 1.0 - (blendMapColor.r + blendMapColor.g + blendMapColor.b);
    vec2 tiledCoords = TexCoord0 * 100.0;
    vec4 backgroundTextureColor = texture(u_Texture1, tiledCoords * 4) * backgroundTextureAmount;
    vec4 rTextureColor = texture(u_Texture2, tiledCoords * 10) * blendMapColor.r;
    vec4 gTextureColor = texture(u_Texture3, tiledCoords * 2) * blendMapColor.g;
    vec4 bTextureColor = texture(u_Texture4, tiledCoords * 4) * blendMapColor.b;

    outColor = backgroundTextureColor + rTextureColor + gTextureColor + bTextureColor;
    outColor = outColor * CreateColor();
    outColor = mix(u_SkyColor, outColor, Visibility);

    if(u_ShowCircle == 1)
    {
        vec4 circleColor = CreateCircle();
        outColor += circleColor;
    }

    if(u_ShowGrid == 1)
    {
        vec2 uv = TexCoord0;
        vec2 gridUV = uv * u_GridUV;
        vec2 grid = abs(fract(gridUV - 0.5) - 0.5) / fwidth(gridUV);
        float line = min(grid.x, grid.y);

        float x = 1.0 - min(line, 1.0);
        vec4 gridColor = vec4(vec3(1.0 - min(line, 1.0)) * 30, 1.0);
        gridColor.a = 0.0;
        outColor = mix(outColor, gridColor, 0.005);
    }
}";
    }
}