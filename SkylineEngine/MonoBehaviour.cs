namespace SkylineEngine
{
    public class MonoBehaviour : Component
    {
        public MonoBehaviour()
        {

        }

        public T GetComponent<T>() where T : Component
        {
            return this.gameObject.GetComponent<T>();
        }

        public T AddComponent<T>() where T : Component, new()
        {
            return this.gameObject.AddComponent<T>();
        }
    }
}
