namespace SkylineEngine.InputManagement
{
    [System.Serializable]
    public class AxisInfo
    {
        public string name;
        public AxisKeys[] keys;
    }

    [System.Serializable]
    public class AxisKeys
    {
        public KeyCode positive;
        public KeyCode negative;
    }

    public enum AxisDirection
    {
        Positive,
        Negative
    }
}
