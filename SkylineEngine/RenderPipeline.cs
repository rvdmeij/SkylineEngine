using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SkylineEngine.Shaders;

namespace SkylineEngine
{
    public static class RenderPipeline
    {
        private static List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
        private static List<LineRenderer> lineRenderers = new List<LineRenderer>();
        private static List<ParticleSystem> particleRenderers = new List<ParticleSystem>();
        private static Camera camera;
        private static GameObject camObject;
        private static GameObject lightObject;
        private static Skybox skybox;
        private static Shader skyboxShader;
        private static WaterFrameBuffer waterFrameBuffer;

        public static int FrameBufferReflectionTexture
        {
            get 
            { 
                if(waterFrameBuffer == null)
                    return 0;
                return waterFrameBuffer.ReflectionTexture; 
            }
        }

        public static void Initialize()
        {
            camObject = new GameObject();
            lightObject = new GameObject();

            camera = camObject.AddComponent<Camera>();
            lightObject.AddComponent<Light>();

            waterFrameBuffer = new WaterFrameBuffer();

            Debug.Log("RenderPipeline initialized");
        }

        public static void Dispose()
        {
            waterFrameBuffer.Dispose();
        }

        public static List<MeshRenderer> GetMeshRenderers() 
        { 
            return meshRenderers; 
        }

        public static void Update()
        {
            if (camera == null)
                return;

            //GL.Enable(EnableCap.ClipDistance0);
            
            waterFrameBuffer.BindReflectionFrameBuffer();
            Render();
            waterFrameBuffer.UnbindCurrentFrameBuffer();
            
            //GL.Disable(EnableCap.ClipDistance0);
            Render();
        }

        private static void Render()
        {
            if(skybox != null)
            {
                RenderSkybox();
            }

            for (int i = 0; i < meshRenderers.Count; i++)
            {
                meshRenderers[i].Render();
            }

            for (int i = 0; i < lineRenderers.Count; i++)
            {               
                lineRenderers[i].Render();
            }

            for (int i = 0; i < particleRenderers.Count; i++)
            {
                particleRenderers[i].Render();
            }
        }

        public static int PushData<T>(GameObject gameObject) where T : Renderer
        {
            int index = meshRenderers.Count;

            var renderer = gameObject.GetComponent<T>();

            if (renderer == null)
            {
                Debug.Log("No Renderer component found on game object with ID " + gameObject.InstanceId);
                return index;
            }

            Type type = renderer.GetType();

            if(type == typeof(MeshRenderer))
            {
                MeshRenderer meshRenderer = renderer as MeshRenderer;

                for (int i = 0; i < meshRenderers.Count; i++)
                {
                    if (meshRenderers[i].InstanceId == meshRenderer.InstanceId)
                    {
                        meshRenderer.meshFilter.mesh.Initialize();
                        return index;
                    }
                }

                if (meshRenderer.Initialize())
                {
                    meshRenderers.Add(meshRenderer);
                    Debug.Log("Added " + gameObject.name + " to RenderPipeline with ID " + meshRenderer.InstanceId);
                }
                else
                {
                    Debug.Log("Could not add " + gameObject.name + " to RenderPipeline because the MeshRenderer couldn't be initialized");
                }
            }
            else if(type == typeof(LineRenderer))
            {
                LineRenderer lineRenderer = renderer as LineRenderer;

                for (int i = 0; i < lineRenderers.Count; i++)
                {
                    if (lineRenderers[i].InstanceId == lineRenderer.InstanceId)
                    {
                        lineRenderer.Initialize();
                        return index;
                    }
                }

                if (lineRenderer.Initialize())
                {
                    lineRenderers.Add(lineRenderer);
                    Debug.Log("Added " + gameObject.name + " to RenderPipeline with ID " + lineRenderer.InstanceId);
                }
                else
                {
                    Debug.Log("Could not add " + gameObject.name + " to RenderPipeline because the MeshRenderer couldn't be initialized");
                }
            }
            else if(type == typeof(ParticleSystem))
            {
                ParticleSystem particleRenderer = renderer as ParticleSystem;

                for (int i = 0; i < particleRenderers.Count; i++)
                {
                    if (particleRenderers[i].InstanceId == particleRenderer.InstanceId)
                    {
                        particleRenderer.Initialize();
                        return index;
                    }
                }

                if (particleRenderer.Initialize())
                {
                    particleRenderers.Add(particleRenderer);
                    Debug.Log("Added " + gameObject.name + " to RenderPipeline with ID " + particleRenderer.InstanceId);
                }
                else
                {
                    Debug.Log("Could not add " + gameObject.name + " to RenderPipeline because the ParticleSystem couldn't be initialized");
                }
            }

            return index;
        }

        public static void PopData(int meshRendererInstanceId)
        {
            int index = -1;
            string name = "GameObject";
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                if (meshRenderers[i].InstanceId == meshRendererInstanceId)
                {
                    index = i;
                    name = meshRenderers[i].gameObject.name;
                    break;
                }
            }

            if (index >= 0)
            {
                meshRenderers.RemoveAt(index);
                Debug.Log("Removed " + name + " from RenderPipeline with ID " + meshRendererInstanceId);
            }
        }

        public static void LoadSkybox(SkyboxFaces faces)
        {
            skybox = new Skybox();

            skyboxShader = Resources.LoadShader(SkyboxShader.vertex, SkyboxShader.fragment, "Skybox");

            skybox.Load(faces.front, faces.back, faces.top, faces.bottom, faces.left, faces.right);
            skybox.SetShader(skyboxShader);
            skybox.Bind();
        }

        private static void RenderSkybox()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);            

            Color32 c = new Color32(255, 255, 255, 255);
            Vector3 color = new Vector3(c.r, c.g, c.b);
            
            Color32 s = new Color32(166, 166, 137, 255);
            Vector3 skyColor = new Vector3(s.r, s.g, s.b);

            var projection = Camera.main.GetPerspectiveProjectionMatrix();
            var model = OpenTK.Mathematics.Matrix4.Identity;
            var view = new OpenTK.Mathematics.Matrix4(new OpenTK.Mathematics.Matrix3(Camera.main.GetViewMatrix()));

            skyboxShader.Bind();
            skyboxShader.SetInt("u_DiffuseTexture", 0);
            skyboxShader.SetMat4("u_Model", model);
            skyboxShader.SetMat4("u_View", view);
            skyboxShader.SetMat4("u_Projection", projection);
            skyboxShader.SetFloat3("u_CamPosition", Camera.main.transform.position);
            skyboxShader.SetFloat3("u_DiffuseColor", color);
            skyboxShader.SetFloat("u_Time", Time.time);
            skyboxShader.SetFloat3("u_SkyColor", skyColor);
            skybox.Draw();
        }
    }
}
