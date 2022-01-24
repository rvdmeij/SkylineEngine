using System;
using System.Collections.Generic;
using System.Linq;

namespace SkylineEngine
{
    public sealed class KeyboardState
    {
        private class KeyState
        {
            public int up;
            public int down;
            public int pressed;

            public KeyState()
            {
                up = 0;
                down = 0;
                pressed = 0;
            }
        }

        private static KeyboardState m_instance;
        private Dictionary<KeyCode, KeyState> m_keystates;

        public static KeyboardState Instance
        {
            get { return m_instance; }
        }

        public KeyboardState()
        {
            if(m_instance == null)
                m_instance = this;

            m_keystates = new Dictionary<KeyCode, KeyState>();

            var keys = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>();

            foreach(KeyCode key in keys)
            {
                m_keystates.Add(key, new KeyState());
            }
        }

        public void SetState(KeyCode key, int up, int down, int pressed)
        {
            if(m_keystates.ContainsKey(key))
            {
                m_keystates[key].up = up;
                m_keystates[key].down = down;
                m_keystates[key].pressed = pressed;
            }
        }

        public bool IsKeyDown(KeyCode key)
        {
            return m_keystates[key].down > 0;
        }

        public bool IsKeyPressed(KeyCode key)
        {
            return m_keystates[key].pressed > 0;
        }

        public bool IsKeyUp(KeyCode key)
        {
            return m_keystates[key].up > 0;
        }
    }
}
