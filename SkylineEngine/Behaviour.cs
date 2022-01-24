using System;

namespace SkylineEngine
{
    internal class Behaviour
    {
        public int instanceId;

        public event Action onUpdate;
        public event Action onFixedUpdate;
        public event Action onLateUpdate;
        public event Action onStart;
        public event Action onAwake;
        public event Action onEnable;
        public event Action onDisable;
        public event Action onGUI;
        public event Action onApplicationQuit;

        private Component behaviour;

        public GameObject gameObject
        {
            get { return behaviour.gameObject; }
        }

        public Behaviour(Component behaviour)
        {
            this.behaviour = behaviour;
        }

        public void OnDisable()
        {
            onDisable?.Invoke();
        }

        public void OnEnable()
        {
            onEnable?.Invoke();
        }

        public void Awake()
        {
            onAwake?.Invoke();
        }

        public void Start()
        {
            onStart?.Invoke();
        }

        public void Update()
        {
            if(behaviour.enabled && behaviour.gameObject.activeSelf)
                onUpdate?.Invoke();
        }

        public void FixedUpdate()
        {
            if(behaviour.enabled && behaviour.gameObject.activeSelf)
                onFixedUpdate?.Invoke();
        }

        public void LateUpdate()
        {
            if(behaviour.enabled && behaviour.gameObject.activeSelf)
                onLateUpdate?.Invoke();
        }

        public void OnGUI()
        {
            if(behaviour.enabled && behaviour.gameObject.activeSelf)
                onGUI?.Invoke();
        }

        public void OnApplicationQuit()
        {
            if(behaviour.enabled && behaviour.gameObject.activeSelf)
                onApplicationQuit?.Invoke();
        }
    }
}
