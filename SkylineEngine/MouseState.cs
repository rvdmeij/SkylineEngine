using System;
using System.Collections.Generic;
using System.Linq;

namespace SkylineEngine
{
    public sealed class MouseState
    {
        private class ButtonState
        {
            public int up;
            public int down;
            public int pressed;

            public ButtonState()
            {
                up = 0;
                down = 0;
                pressed = 0;
            }
        }

        private Dictionary<MouseButton, ButtonState> m_buttonstates;
        private Vector2 m_position;
        private Vector2 m_positionDelta;
        private Vector3 m_worldspacePosition;
        private float m_scrollDirection;

        public Vector2 Position 
        { 
            get { return m_position; } 
        }

        public Vector3 WorldSpacePosition
        {
            get { return m_worldspacePosition; }
        }

        public Vector2 PositionDelta
        {
            get 
            { 
                var delta = m_positionDelta;
                m_positionDelta.x = 0;

                return delta; 
            }
        }

        public float ScrollDirection
        {
            get { return m_scrollDirection; }
        }

        public MouseState()
        {
            m_position = Vector2.zero;
            m_positionDelta = Vector2.zero;
            m_buttonstates = new Dictionary<MouseButton, ButtonState>();

            var buttons = Enum.GetValues(typeof(MouseButton)).Cast<MouseButton>();

            foreach(MouseButton button in buttons)
            {
                m_buttonstates.Add(button, new ButtonState());
            }
        }

        public void SetState(MouseButton button, int up, int down, int pressed)
        {
            m_buttonstates[button].up = up;
            m_buttonstates[button].down = down;
            m_buttonstates[button].pressed = pressed;
        }

        public void SetPosition(Vector2 mousePosition)
        {
            m_position = mousePosition;
        }

        public void SetWorldSpacePosition(Vector3 position)
        {
            m_worldspacePosition = position;
        }

        public void SetPositionDelta(Vector2 mousePositionDelta)
        {
            //if(Vector2.Distance(mousePositionDelta, m_positionDelta) > 3)
            m_positionDelta = mousePositionDelta;
        }

        public void SetScrollDirection(float direction)
        {
            m_scrollDirection = direction;
        }

        public bool IsKeyDown(MouseButton button)
        {
            return m_buttonstates[button].down > 0;
        }

        public bool IsKeyPressed(MouseButton button)
        {
            return m_buttonstates[button].pressed > 0;
        }

        public bool IsKeyUp(MouseButton button)
        {
            return m_buttonstates[button].up > 0;
        }
    }
}
