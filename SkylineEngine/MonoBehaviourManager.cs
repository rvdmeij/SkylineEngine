using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SkylineEngine
{    
    internal static class MonoBehaviourManager
    {
        private static List<Behaviour> behaviours = new List<Behaviour>();
        private static Queue<int> destroyQueue = new Queue<int>();

        public static void Register(MonoBehaviour m)
        {
            RegisterMethods(m);
        }

        public static void UnRegister(int instanceId)
        {
            AddToDestroyQueue(instanceId);
        }

        private static void RegisterMethods(MonoBehaviour m)
        {
            Type type = m.GetType();
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            Behaviour behaviour = new Behaviour(m as Component);

            for (int i = 0; i < methods.Length; i++)
            {
                Delegate del = CreateDelegate(m, methods[i]);

                if (methods[i].Name == "Awake")
                {
                    behaviour.onAwake += (Action)del;
                }
                else if (methods[i].Name == "Start")
                {
                    behaviour.onStart += (Action)del;
                }
                else if (methods[i].Name == "Update")
                {
                    behaviour.onUpdate += (Action)del;
                }
                else if (methods[i].Name == "FixedUpdate")
                {
                    behaviour.onFixedUpdate += (Action)del;
                }
                else if (methods[i].Name == "LateUpdate")
                {
                    behaviour.onLateUpdate += (Action)del;
                }
                else if (methods[i].Name == "OnEnable")
                {
                    behaviour.onEnable += (Action)del;
                }
                else if (methods[i].Name == "OnDisable")
                {
                    behaviour.onDisable += (Action)del;
                }
                else if (methods[i].Name == "OnGUI")
                {
                    behaviour.onGUI += (Action)del;
                }
                else if (methods[i].Name == "OnApplicationQuit")
                {
                    behaviour.onApplicationQuit += (Action)del;
                }
            }

            behaviour.instanceId = m.InstanceId;
            behaviours.Add(behaviour);

            behaviour.OnEnable();
            behaviour.Awake();
            behaviour.Start();
        }

        private static Delegate[] ExtractMethods(object obj)
        {
            Type type = obj.GetType();

            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            Delegate[] methodsDelegate = new Delegate[methods.Count()];

            for (int i = 0; i < methods.Count(); i++)
            {
                methodsDelegate[i] = CreateDelegate(obj, methods[i]);
            }

            return methodsDelegate;
        }

        public static Delegate CreateDelegate(object instance, MethodInfo method)
        {
            var parameters = method.GetParameters()
                       .Select(p => Expression.Parameter(p.ParameterType, p.Name))
                        .ToArray();

            var call = Expression.Call(Expression.Constant(instance), method, parameters);
            return Expression.Lambda(call, parameters).Compile();
        }

        public static void OnActiveStateChanged(int instanceId, bool active)
        {
            if(active)
            {
                Awake(instanceId);
                OnEnable(instanceId);
                Start(instanceId);
            }
            else
            {
                OnDisable(instanceId);
            }
        }

        public static void OnDisable(int instanceId)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (behaviours[i].instanceId == instanceId || behaviours[i].gameObject.InstanceId == instanceId)
                {                    
                    behaviours[i].OnDisable();
                }
            }
        }

        public static void OnEnable(int instanceId)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (behaviours[i].instanceId == instanceId || behaviours[i].gameObject.InstanceId == instanceId)
                {
                    behaviours[i].OnEnable();
                }
            }
        }

        public static void OnApplicationQuit()
        {
            for (int i = 0; i < behaviours.Count; i++)
                behaviours[i].OnApplicationQuit();
        }

        public static void Awake()
        {
            for (int i = 0; i < behaviours.Count; i++)
                behaviours[i].Awake();
        }

        public static void Awake(int instanceId)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (behaviours[i].instanceId == instanceId || behaviours[i].gameObject.InstanceId == instanceId)
                {
                    behaviours[i].Awake();
                }
            }
        }

        public static void Start(int instanceId)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (behaviours[i].instanceId == instanceId || behaviours[i].gameObject.InstanceId == instanceId)
                {
                    behaviours[i].Start();
                }
            }
        }

        public static void Start()
        {
            for (int i = 0; i < behaviours.Count; i++)
                behaviours[i].Start();
        }

        public static void Update()
        {
    		Time.SetDeltaTime();

            for (int i = 0; i < behaviours.Count; i++)
                behaviours[i].Update();
        }

        public static void FixedUpdate()
        {
            Time.SetFixedDeltaTime();

            for (int i = 0; i < behaviours.Count; i++)
                behaviours[i].FixedUpdate();
        }

        public static void LateUpdate()
        {
            for (int i = 0; i < behaviours.Count; i++)
                behaviours[i].LateUpdate();
        }

        public static void OnGUI()
        {
            for (int i = 0; i < behaviours.Count; i++)
                behaviours[i].OnGUI();

            GUI.ClearIdQueue();
        }

        internal static void AddToDestroyQueue(int instanceId)
        {
            destroyQueue.Enqueue(instanceId);
        }

        internal static void UpdateDestroyQueue()
        {
            if(destroyQueue.Count > 0)
            {
                int count = destroyQueue.Count;

                for (int i = 0; i < count; i++)
                {
                    int instanceId = destroyQueue.Dequeue();
                    Destroy(instanceId);
                }
            }
        }

        internal static void Destroy(int instanceId)
        {
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (behaviours[i].instanceId == instanceId)
                {
                    behaviours.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
