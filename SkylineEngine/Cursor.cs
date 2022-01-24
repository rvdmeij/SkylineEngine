namespace SkylineEngine
{
    public static class Cursor
    {
        private static CursorLockMode m_lockState = CursorLockMode.None;
        private static bool m_visible;
        private static ApplicationBase m_gameWindow;

        internal static void Initialize(ApplicationBase window)
        {
            m_gameWindow = window;
        }

        public static CursorLockMode lockState
        {
            get
            {
                return m_lockState;
            }
            set
            {
                m_lockState = value;
                ToggleState();
            }
        }

        public static bool visible
        {
            get 
            { 
                return m_visible; 
            }
            set 
            {
                m_visible = value;
                ToggleVisibility();
            }
        }

        internal static void ToggleState()
        {
            switch (m_lockState)
            {
                case CursorLockMode.None:
                    m_gameWindow.LockCursor(false);
                    break;
                case CursorLockMode.Locked:
                    m_gameWindow.LockCursor(true);
                    break;
                case CursorLockMode.Confined:
                    //m_gameWindow.CursorGrabbed = true;
                    break;
            }
        }

        internal static void ToggleVisibility()
        {
            m_gameWindow.ToggleCursor(m_visible);
        }
    }
}