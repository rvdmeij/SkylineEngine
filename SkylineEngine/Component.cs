using System.Collections;

namespace SkylineEngine
{
    public class Component : Object
    {
        public GameObject gameObject;
        public Transform transform;
        private bool m_enabled;

        public bool enabled
        {
            get { return m_enabled; }
            set 
            {
                //Only set value when it's not equal to existing value
                if (value != m_enabled)
                {
                    m_enabled = value;
                    if (GetType().IsSubclassOf(typeof(MonoBehaviour)))
                    {

                        if (m_enabled)
                            MonoBehaviourManager.OnActiveStateChanged(InstanceId, true);
                        else
                            MonoBehaviourManager.OnActiveStateChanged(InstanceId, false);
                    }
                }
            }
        }

        public Component()
        {
            m_enabled = true;
        }

        public virtual void InitializeComponent()
        {

        }

        public void StartCoroutine(IEnumerator routine)
        {
            CoroutineScheduler.StartCoroutine(routine);
        }

        public void Destroy()
        {
            if(this is MeshRenderer)
            {
                RenderPipeline.PopData(this.InstanceId);
            }
            else if (this.GetType().IsSubclassOf(typeof(MonoBehaviour)))
            {
                MonoBehaviourManager.UnRegister(this.InstanceId);
            }
        }

        public Component Clone()
        {
            Component c = new Component();
            c.gameObject = gameObject;
            c.transform = transform;
            c.m_enabled = m_enabled;
            return c;
        }
    }
}
