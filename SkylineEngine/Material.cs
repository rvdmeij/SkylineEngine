using System.Collections.Generic;
using OpenTK.Mathematics;

namespace SkylineEngine
{
    public sealed class Material : Component
    {
        public Shader shader;
        public List<Texture> textures;
        public Matrix4 model;
        public Matrix4 view;
        public Matrix4 projection;
        public bool alphaBlend;
        public bool wireframe;
        public bool showGrid;
        public int identifier;
        public OpenTK.Graphics.OpenGL.PrimitiveType mode;
        public Color32 ambientColor;
        public Color32 diffuseColor;
        public Color32 specularColor;
        public Color32 lightColor;
        public Color32 skyColor;
        public Vector3 lightPosition;
        public Vector3 lightDirection;
        public Vector3 viewPosition;
        public Vector2 resolution;
        public Vector2 uvScale;
        public int diffuseTexture;
        public float specularStrength;
        public float ambientStrength;
        public float time;
        public float strength;

        private UniformInfo uniformIdentifier;
        private UniformInfo uniformAmbientColor;
        private UniformInfo uniformAmbientStrength;
        private UniformInfo uniformDiffuseColor;
        private UniformInfo uniformSpecularColor;
        private UniformInfo uniformSpecularStrength;
        private UniformInfo uniformLightColor;
        private UniformInfo uniformSkyColor;
        private UniformInfo uniformLightPosition;
        private UniformInfo uniformLightDirection;
        private UniformInfo uniformViewPosition;
        private UniformInfo uniformDiffuseTexture;
        private UniformInfo uniformModel;
        private UniformInfo uniformView;
        private UniformInfo uniformProjection;
        private UniformInfo uniformResolution;
        private UniformInfo uniformUVScale;
        private UniformInfo uniformTime;
        private UniformInfo uniformStrength;
        private UniformInfo uniformIsSelected;
        private UniformInfo uniformShowGrid;
        private UniformInfo uniformMouse;
        private UniformInfo uniformScreenSize;
        private UniformInfo uniformCamPosition;
        private List<UniformInfo> uniformTextures = new List<UniformInfo>();

        public Material()
        {
            textures = new List<Texture>();

            ambientColor = new Color32(255, 255, 255, 255);
            diffuseColor = new Color32(250, 213, 157, 255);
            specularColor = new Color32(255, 255, 255, 255);
            lightColor = new Color32(190, 190, 190, 255);
            skyColor = new Color32(166, 166, 137, 255);

            ambientStrength = 0.1f;
            specularStrength = 100.0f;

            lightPosition = new Vector3(-1, 10, 5);
            lightDirection = new Vector3(0.1f, 0.8f, 0.1f);

            resolution = new Vector2(512, 512);
            uvScale = new Vector2(1, 1);

            diffuseTexture = 0;

            strength = 1.0f;

            alphaBlend = false;
            wireframe = false;

            mode = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
        }

        ~Material()
        {

        }

        public void Initialize()
        {
            uniformIdentifier = shader.GetUniform("u_Identifier");
            uniformAmbientColor = shader.GetUniform("u_AmbientColor");
            uniformAmbientStrength = shader.GetUniform("u_AmbientStrength");
            uniformDiffuseColor = shader.GetUniform("u_DiffuseColor");
            uniformSpecularColor = shader.GetUniform("u_SpecularColor");
            uniformSpecularStrength = shader.GetUniform("u_SpecularStrength");
            uniformLightColor = shader.GetUniform("u_LightColor");
            uniformSkyColor = shader.GetUniform("u_SkyColor");
            uniformLightPosition = shader.GetUniform("u_LightPosition");
            uniformLightDirection = shader.GetUniform("u_LightDirection");
            uniformViewPosition = shader.GetUniform("u_ViewPosition");
            uniformDiffuseTexture = shader.GetUniform("u_DiffuseTexture");
            uniformModel = shader.GetUniform("u_Model");
            uniformView = shader.GetUniform("u_View");
            uniformProjection = shader.GetUniform("u_Projection");
            uniformResolution = shader.GetUniform("u_Resolution");
            uniformUVScale = shader.GetUniform("u_UVScale");
            uniformTime = shader.GetUniform("u_Time");
            uniformIsSelected = shader.GetUniform("u_IsSelected");
            uniformShowGrid = shader.GetUniform("u_ShowGrid");
            uniformMouse = shader.GetUniform("u_Mouse");
            uniformScreenSize = shader.GetUniform("u_ScreenSize");
            uniformCamPosition = shader.GetUniform("u_CamPosition");
            uniformStrength = shader.GetUniform("u_Strength");

            if (textures == null)
                return;

            for (int i = 0; i < textures.Count; i++)
            {
                string n = "u_Texture" + i;
                UniformInfo u = shader.GetUniform(n);
                uniformTextures.Add(u);
            }
        }

        public void UpdateUniforms()
        {
            Light light = Light.main;
            lightColor = light.color;
            lightPosition = light.gameObject.transform.position;
            lightDirection = light.gameObject.transform.forward;

            Camera cam = Camera.main;
            viewPosition = cam.gameObject.transform.position;

            if (uniformAmbientColor != null)
                shader.SetFloat4(uniformAmbientColor.location, ambientColor);
            if (uniformAmbientStrength != null)
                shader.SetFloat(uniformAmbientStrength.location, ambientStrength);
            if (uniformDiffuseColor != null)
                shader.SetFloat4(uniformDiffuseColor.location, diffuseColor);
            if (uniformSpecularColor != null)
                shader.SetFloat4(uniformSpecularColor.location, specularColor);
            if (uniformSpecularStrength != null)
                shader.SetFloat(uniformSpecularStrength.location, specularStrength);
            if (uniformLightColor != null)
                shader.SetFloat4(uniformLightColor.location, lightColor);
            if (uniformSkyColor != null)
                shader.SetFloat4(uniformSkyColor.location, skyColor);
            if (uniformLightPosition != null)
                shader.SetFloat3(uniformLightPosition.location, lightPosition);
            if (uniformLightDirection != null)
                shader.SetFloat3(uniformLightDirection.location, lightDirection);
            if (uniformViewPosition != null)
                shader.SetFloat3(uniformViewPosition.location, viewPosition);
            if (uniformDiffuseTexture != null)
                shader.SetInt(uniformDiffuseTexture.location, diffuseTexture);
            if (uniformModel != null)
                shader.SetMat4(uniformModel.location, model);
            if (uniformView != null)
                shader.SetMat4(uniformView.location, view);
            if (uniformProjection != null)
                shader.SetMat4(uniformProjection.location, projection);
            if (uniformResolution != null)
                shader.SetFloat2(uniformResolution.location, resolution);
            if (uniformUVScale != null)
                shader.SetFloat2(uniformUVScale.location, uvScale);
            if (uniformTime != null)
                shader.SetFloat(uniformTime.location, Time.time);

            if (uniformShowGrid != null)
                shader.SetInt(uniformShowGrid.location, showGrid ? 1 : 0);

            if (uniformMouse != null)
                shader.SetFloat2(uniformMouse.location, Input.GetMousePosition());

            if (uniformScreenSize != null)
                shader.SetFloat2(uniformScreenSize.location, Screen.size);

            if (uniformCamPosition != null)
                shader.SetFloat3(uniformCamPosition.location, cam.transform.position);

            if(uniformStrength != null)
                shader.SetFloat(uniformStrength.location, strength);

            if (uniformTextures != null)
            {
                int unit = 0;

                for (int i = 0; i < uniformTextures.Count; i++)
                {
                    if (uniformTextures[i] != null)
                    {
                        shader.SetInt(uniformTextures[i].location, unit);
                    }
                    unit++;
                }
            }
        }

        public void AddTexture(Texture tex)
        {
            textures.Add(tex);
        }
    }
}
