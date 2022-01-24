# SkylineEngine
Unity inspired game engine written in C#

# Dependencies
- https://www.nuget.org/packages/OpenTK/4.0.0
- https://www.nuget.org/packages/AssimpNet/4.1.0
- https://www.nuget.org/packages/BulletSharp.NetCore/
- https://github.com/libsdl-org/SDL (build from source or install through package manager)
- https://github.com/japajoe/imgui (build from source)


```csharp
using SkylineEngine;
using OpenTK.Graphics.OpenGL;

namespace SkylineEngineApplication
{
    class Program
    {
        private static Application application;
        private static GameObject gameObject;

        static void Main(string[] args)
        {
            application = new Application("Skyline Engine", 512, 512, 3, 3, false);
            application.created += OnCreated;
            application.render += OnRender;
            application.Run();            
        }

        static void OnCreated()
        {
            gameObject = new GameObject();
            gameObject.AddComponent<GameManager>();
        }

        static void OnRender()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(0, 0, 0, 1);
        }
    }

    public class GameManager : MonoBehaviour
    {
        private Camera camera;
        private GameObject cube;

        void Start()
        {
            camera = Camera.main;            
            camera.farClipPlane = 10000.0f;
            camera.transform.position = new Vector3(0, 1, 5);

            var firstPerson = camera.gameObject.AddComponent<FirstPersonController>();
            firstPerson.speed *= 50;
            firstPerson.zoomSpeed *= 200;            

            Light.main.strength = 1.0f;
            Light.main.transform.position = new Vector3(0, 1000, -500);
            Light.main.transform.rotation = new Quaternion(-0.26127627f,-0.00029115385f,0.26208368f,-0.9291085f);

            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = Vector3.zero;
        }

        //Only call ImGui/GUI functions from void OnGUI
        void OnGUI()
        {
            GUI.Label(new Rect(5,5,200,20), "Skyline Engine");
        }
    }
}
```
