using System.Collections.Generic;
using SkylineEngine.Shaders;

namespace SkylineEngine
{
    public sealed class GameObject : Object
    {
        public Transform transform = null;
        public Layer layer = Layer.Default;
        public string tag;

        private List<Component> components = new List<Component>();
        private static List<GameObject> gameObjects = new List<GameObject>();       

        public GameObject()
        {
            this.transform = AddComponent<Transform>();
            this.name = "GameObject";
            gameObjects.Add(this);
        }

        public GameObject(string name)
        {
            this.transform = AddComponent<Transform>();
            this.name = name;
            gameObjects.Add(this);
        }

        public static GameObject Clone(GameObject target)
        {
            GameObject g = new GameObject(target.name);
            
            for(int i = 0; i < target.components.Count; i++)
            {
                g.AddComponent(target.components[i]);
            }

            return g;
        }

        public T GetComponent<T>() where T : Component
        {
            for(int i = 0; i < components.Count; i++)
            {
                if(components[i].GetType() == typeof(T))
                {
                    return components[i] as T;
                }
            }
            return null;
        }

        public T[] GetComponents<T>() where T : Component
        {
            List<T> comp = new List<T>();

            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].GetType() == typeof(T))
                {
                    T c = components[i] as T;
                    comp.Add(c);
                }
            }

            return comp.ToArray();
        }

        public T[] GetComponentsInChildren<T>() where T : Component
        {
            List<T> comp = new List<T>();

            for(int i = 0; i < transform.children.Count; i++)
            {
                var c = transform.children[i].gameObject.GetComponent<T>();
                if(c != null)
                    comp.Add(c);
            }
            
            return comp.ToArray();
        }

        public Component[] GetComponents()
        {
            return components.ToArray();
        }

        internal Component AddComponent(Component c)
        {
            int count = components.Count;
            Component clone = c.Clone();
            clone.gameObject = this;
            clone.transform = this.transform;
            components.Add(clone);
            clone.InitializeComponent();

            if (typeof(Component).IsSubclassOf(typeof(MonoBehaviour)))
                MonoBehaviourManager.Register(clone as MonoBehaviour);
            else if (typeof(Component) == typeof(AudioSource))
                AudioSourceManager.Register(clone as AudioSource);

            return clone;            
        }

        public T AddComponent<T>() where T : Component, new()
        {
            int count = components.Count;
            T instance = new T();
            instance.gameObject = this;
            instance.transform = this.transform;
            components.Add(instance);
            instance.InitializeComponent();

            if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
                MonoBehaviourManager.Register(instance as MonoBehaviour);
            else if (typeof(T) == typeof(AudioSource))
                AudioSourceManager.Register(instance as AudioSource);

            return instance;
        }

        public static GameObject Find(string name)
        {
            for(int i = 0; i < gameObjects.Count; i++)
                if(gameObjects[i].name == name)
                    return gameObjects[i];
            return null;
        }

        public static GameObject FindWithTag(string tag)
        {
            for(int i = 0; i < gameObjects.Count; i++)
                if(gameObjects[i].tag == tag)
                    return gameObjects[i];
            return null;
        }

        public static GameObject[] FindGameObjectsWithTag(string tag)
        {
            List<GameObject> objects = new List<GameObject>();

            for(int i = 0; i < gameObjects.Count; i++)
                if(gameObjects[i].tag == tag)
                    objects.Add(gameObjects[i]);
            
            return objects.ToArray();
        }        

        public static GameObject CreatePrimitive(PrimitiveType type, float scaleX = 1, float scaleY = 1, float scaleZ = 1)
        {
            Vector3 scale = new Vector3(scaleX, scaleY, scaleZ);

            Texture texture = Resources.Load<Texture>("Default");

            GameObject g = new GameObject();
            g.AddComponent<MeshRenderer>();            

            var filter = g.AddComponent<MeshFilter>();
            var material = g.AddComponent<Material>();

            material.uvScale = new Vector2(1.0f, 1.0f);
            material.skyColor = new Color32(255, 255, 255, 255);
            material.diffuseColor = new Color32(255, 255, 255, 255);
            material.textures.Add(texture);

            switch (type)
            {
                case PrimitiveType.Capsule:
                    material.shader = Resources.LoadShader(DefaultShader.vertex, DefaultShader.fragment, "Default");
                    filter.mesh = MeshPrimitive.CreateCapsule(scale);
                    break;
                case PrimitiveType.Cube:
                    material.shader = Resources.LoadShader(DefaultShader.vertex, DefaultShader.fragment, "Default");
                    filter.mesh = MeshPrimitive.CreateCube(scale);
                    break;
                case PrimitiveType.Cylinder:
                    material.shader = Resources.LoadShader(DefaultShader.vertex, DefaultShader.fragment, "Default");
                    filter.mesh = MeshPrimitive.CreateCylinder(scale);                
                    break;
                case PrimitiveType.Plane:
                    material.shader = Resources.LoadShader(DefaultShader.vertex, DefaultShader.fragment, "Default");
                    filter.mesh = MeshPrimitive.CreatePlane(scale);
                    break;
                case PrimitiveType.Sphere:
                    material.shader = Resources.LoadShader(DefaultShader.vertex, DefaultShader.fragment, "Default");
                    filter.mesh = MeshPrimitive.CreateSphere(scale);
                    break;
                case PrimitiveType.Quad:
                    break;
                case PrimitiveType.Plane2D:
                    material.shader = Resources.LoadShader(DefaultShader.vertex, DefaultShader.fragment, "Default");
                    filter.mesh = MeshPrimitive.CreatePlane2D(scale);
                    break;
                case PrimitiveType.Billboard:
                    material.shader = Resources.Load<Shader>("res/Shaders/Billboard.shader");
                    filter.mesh = MeshPrimitive.CreateTree(scale);
                    var renderer = g.GetComponent<MeshRenderer>();
                    renderer.doubleSided = true;
                    break;
                case PrimitiveType.Circle:
                    material.shader = Resources.LoadShader(DefaultShader.vertex, DefaultShader.fragment, "Default");
                    filter.mesh = MeshPrimitive.CreateCircle(24, scale);
                    break;
                default:
                    break;
            }
            return g;
        }

        public void Destroy()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].InstanceId == InstanceId)
                {
                    var meshRenderers = new List<MeshRenderer>(GetComponents<MeshRenderer>());

                    if(meshRenderers != null)
                    {
                        for (int j = 0; j < meshRenderers.Count; j++)
                        {
                            meshRenderers[j].Destroy();
                        }
                    }

                    var audioSources = new List<AudioSource>(GetComponents<AudioSource>());

                    if(audioSources != null)
                    {
                        for (int j = 0; j < audioSources.Count; j++)
                        {
                            audioSources[j].Destroy();
                        }
                    }

                    for (int j = 0; j < components.Count; j++)
                    {
                        if (components[j] != null)
                        {
                            if (components[j].GetType().IsSubclassOf(typeof(MonoBehaviour)))
                            {
                                components[j].Destroy();
                            }
                        }
                    }

                    transform = null;

                    gameObjects.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
