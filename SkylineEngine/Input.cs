using System;
using System.Collections.Generic;
using System.Linq;
using SkylineEngine.InputManagement;

namespace SkylineEngine
{
    public static class Input
    {
        protected class KeyState
        {
            public int down;
            public int up;
            
            public KeyState()
            {
                this.down = 0;
                this.up = 0;
            }
        }

        public const int SDLK_SCANCODE_MASK = (1 << 30);
        private static Dictionary<KeyCode, KeyState> keyStates = new Dictionary<KeyCode, KeyState>();
        private static Dictionary<string, KeyCode[]> actionToKeyDictionary = new Dictionary<string, KeyCode[]>();
        private static Dictionary<KeyCode[], string> keyToActionDictionary = new Dictionary<KeyCode[], string>();
        private static Dictionary<string, AxisInfo> keyToAxisDictionary = new Dictionary<string, AxisInfo>();
        private static Dictionary<AxisInfo, string> axisToKeyDictionary = new Dictionary<AxisInfo, string>();
        private static MouseState mouse;

        static Input()
        {
            var keys = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>();

            foreach(KeyCode key in keys)
            {
                keyStates.Add(key, new KeyState());
            }

            mouse = new MouseState();

            CreateDefaultInputAxis();
        }

        private static void CreateDefaultInputAxis()
        {
            AxisInfo infoVertical = new AxisInfo();
            AxisInfo infoHorizontal = new AxisInfo();
            AxisInfo infoYaw = new AxisInfo();

            AxisKeys keysVertical = new AxisKeys();
            AxisKeys keysHorizontal = new AxisKeys();
            AxisKeys keysYaw = new AxisKeys();

            keysVertical.positive = KeyCode.W;
            keysVertical.negative = KeyCode.S;

            keysHorizontal.positive = KeyCode.D;
            keysHorizontal.negative = KeyCode.A;

            keysYaw.positive = KeyCode.E;
            keysYaw.negative = KeyCode.Q;

            List<AxisKeys> vertical = new List<AxisKeys>();
            List<AxisKeys> horizontal = new List<AxisKeys>();
            List<AxisKeys> yaw = new List<AxisKeys>();

            vertical.Add(keysVertical);
            horizontal.Add(keysHorizontal);
            yaw.Add(keysYaw);

            infoVertical.name = "Vertical";
            infoVertical.keys = vertical.ToArray();

            infoHorizontal.name = "Horizontal";
            infoHorizontal.keys = horizontal.ToArray();

            infoYaw.name = "Yaw";
            infoYaw.keys = yaw.ToArray();

            List<AxisInfo> axisInfo = new List<AxisInfo>();
            axisInfo.Add(infoVertical);
            axisInfo.Add(infoHorizontal);
            axisInfo.Add(infoYaw);

            List<ActionInfo> actionInfo = new List<ActionInfo>();

            Input.RegisterAxis(actionInfo.ToArray(), axisInfo.ToArray());
        }

        public static void RegisterAxis(ActionInfo[] keys, AxisInfo[] axisInfo)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (!actionToKeyDictionary.ContainsKey(keys[i].name))
                    actionToKeyDictionary.Add(keys[i].name, keys[i].keys);
                if (!keyToActionDictionary.ContainsKey(keys[i].keys))
                    keyToActionDictionary.Add(keys[i].keys, keys[i].name);
            }

            for (int i = 0; i < axisInfo.Length; i++)
            {
                if (!keyToAxisDictionary.ContainsKey(axisInfo[i].name))
                    keyToAxisDictionary.Add(axisInfo[i].name, axisInfo[i]);
                if (!axisToKeyDictionary.ContainsKey(axisInfo[i]))
                    axisToKeyDictionary.Add(axisInfo[i], axisInfo[i].name);
            }
        }

        public static float GetAxis(string axis)
        {
            if (keyToAxisDictionary.ContainsKey(axis))
            {
                for (int i = 0; i < keyToAxisDictionary[axis].keys.Length; i++)
                {
                    if (GetKey(keyToAxisDictionary[axis].keys[i].positive))
                        return 1.0f;
                    else if (GetKey(keyToAxisDictionary[axis].keys[i].negative))
                        return -1.0f;
                }
            }

            return 0.0f;
        }

        internal static void SetStateUp(KeyCode keyCode, int state)
        {
            if(keyStates.ContainsKey(keyCode))
            {
                if(keyStates[keyCode].up != state)
                    keyStates[keyCode].up = state;
            }
        }

        internal static void SetStateDown(KeyCode keyCode, int state)
        {
            if(keyStates.ContainsKey(keyCode))
            {
                if(keyStates[keyCode].down != state)
                    keyStates[keyCode].down = state;
            }
        }

        public static bool GetKey(KeyCode key)
        {
            if(keyStates.ContainsKey(key))
            {
                if (keyStates[key].down == 1)
                {
                    return true;
                }   
            }
            return false;
        }

        public static bool GetKeyDown(KeyCode key)
        {
            if(keyStates.ContainsKey(key))
            {
                if (keyStates[key].down == 1)
                {
                    keyStates[key].down = 0;
                    return true;
                }   
            }
            return false;            
        }

        public static bool GetKeyUp(KeyCode key)
        {
            if(keyStates.ContainsKey(key))
            {
                if (keyStates[key].up == 1)
                {
                    keyStates[key].up = 0;
                    return true;
                }   
            }
            return false;            
        }

        public static bool GetMouseDown(MouseButton button)
        {
            if(mouse.IsKeyDown(button))
            {
                return true;
            }
            return false;
        }

        public static bool GetMouseUp(MouseButton button)
        {
            if(mouse.IsKeyUp(button))
            {
                return true;
            }
            return false;
        }    

        public static void SetMouseState(MouseButton button, int up, int down, int pressed) 
        {
            mouse.SetState(button, up, down, pressed);
        }

        public static void ClearMouseScrollDirection()
        {
            mouse.SetScrollDirection(0);
        }

        public static void SetMouseScrollDirection(float direction)   
        {
            if(direction == 0)
                return;
            
            if(direction > 0)
                mouse.SetScrollDirection(1);
            else
                mouse.SetScrollDirection(-1);
        }

        public static float GetMouseScrollDirection()
        {
            return mouse.ScrollDirection;
        }

        public static Vector2 GetMousePosition()
        {
            return mouse.Position;
        }

        public static Vector3 GetMouseWorldSpacePosition()
        {
            return mouse.WorldSpacePosition;
        }

        public static Vector2 GetMouseDelta()
        {
            return mouse.PositionDelta;
        }

        internal static void SetMousePosition(Vector2 position)
        {
            mouse.SetPosition(position);
        }

        public static void SetMouseWorldSpacePosition(Vector3 position)
        {
            mouse.SetWorldSpacePosition(position);
        }

        internal static void SetMouseDelta(Vector2 delta)
        {
            mouse.SetPositionDelta(delta);
        }
    }
}
