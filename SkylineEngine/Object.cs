namespace SkylineEngine
{
    public class Object
    {
        public string name = "";
        private bool active = true;

        private int instanceId;
        private static int instanceCount = 0;

        public int InstanceId { get { return instanceId; } }

        public bool activeSelf
        {
            get { return active; }
        }

        public Object()
        {
            SetInstanceId();
        }

        protected void SetInstanceId()
        {
            instanceId = instanceCount;
            instanceCount++;
        }

        public void SetActive(bool isActive)
        {
            this.active = isActive;            
            MonoBehaviourManager.OnActiveStateChanged(instanceId, active);            
        }

        public static Object Instantiate(GameObject g)
        {
            return GameObject.Clone(g);
        }
    }
}
