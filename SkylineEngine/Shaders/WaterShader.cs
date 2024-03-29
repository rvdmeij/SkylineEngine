namespace SkylineEngine.Shaders
{
    public class WaterShader
    {
        public const string vertex = 
@"#version 330 core

layout (location = 0) in vec3 a_position;
layout (location = 2) in vec2 a_uv;
layout (location = 3) in vec3 a_normal;

#define DRAG_MULT 0.048
#define ITERATIONS_NORMAL 48

uniform mat4 u_Model;
uniform mat4 u_View;
uniform mat4 u_Projection;
uniform float u_Time;
uniform vec4 u_clippingPlane;

out vec2 TexCoord0;

const float amplitude = 0.125;
const float frequency = 4;
const float PI = 3.14159;

void main()
{
	vec4 worldPosition = u_Projection * u_View * u_Model * vec4(a_position, 1.0);
    TexCoord0 = a_uv;
    
    float distance = length(a_position);
    float y = sin(gl_VertexID * u_Time * 0.001f) * 0.25f;
    vec3 pos = a_position;
    pos.y = y;

	//gl_ClipDistance[0] = dot(worldPosition, u_clippingPlane);

    gl_Position = u_Projection * u_View * u_Model * vec4(pos, 1.0);
}";

        public const string fragment = 
@"#version 330 core

uniform sampler2D u_Texture0;
uniform float u_Time;
uniform vec4 u_DiffuseColor;
uniform vec2 u_Resolution;
uniform float u_Opacity;
uniform vec2 u_UVScale;

in vec2 TexCoord0;

vec2 resolution = vec2(2, 2);

#define TAU 6.28318530718
#define MAX_ITER 5

void main() 
{
	resolution = u_Resolution;
	
	float time = u_Time * 0.5 + 23.0;
    //vec2 uv = TexCoord0 * 50.0;    
	vec2 uv = TexCoord0 * u_UVScale;

    vec2 p = mod(uv*TAU, TAU)-250.0;

	vec2 i = vec2(p);
	float c = 1.0;
	float inten = .005;

	for (int n = 0; n < MAX_ITER; n++) 
	{
		float t = time * (1.0 - (3.5 / float(n+1)));
		i = p + vec2(cos(t - i.x) + sin(t + i.y), sin(t - i.y) + cos(t + i.x));
		c += 1.0/length(vec2(p.x / (sin(i.x+t)/inten),p.y / (cos(i.y+t)/inten)));
	}
    
	c /= float(MAX_ITER);
	c = 1.17-pow(c, 1.4);
	vec3 colour = vec3(pow(abs(c), 8.0));
    colour = clamp(colour + vec3(0.0, 0.35, 0.5), 0.0, 1.0);
    
	gl_FragColor = vec4(colour, u_Opacity) * u_DiffuseColor;
}";
    }
}