using System;
using System.Runtime.InteropServices;
using DearImGui;
using OpenTK.Graphics.OpenGL;
using SDL2;

namespace SkylineEngine
{
    public class Application : ApplicationBase
    {
        public event RenderEvent render;
        public event PostRenderEvent postRender;
        private ImGuiControl imGuiControl;

        [DllImport("imgui", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint GLInitGlew(bool experimental);

        public Application(string title, int width, int height, int versionMajor, int versionMinor, bool vsync = true) 
        : base(title, width, height, versionMajor, versionMinor, vsync)
        {
            created += OnCreated;
            close += OnClose;
            keyDown += OnKeyDown;
            keyUp += OnKeyUp;
            keyPress += OnKeyPress;
            resize += OnResize;
            mouseMove += OnMouseMove;
            mouseDown += OnMouseDown;
            mouseUp += OnMouseUp;
            mouseScroll += OnMouseScroll;
            textInput += OnTextInput;
        }

        private void OnCreated()
        {
            RenderPipeline.Initialize();
            PhysicsPipeline.Initialize();

            GLInitGlew(true);
            imGuiControl = new ImGuiControl(mainWindow);
            imGuiControl.onGUI += OnGUI;

            if(!imGuiControl.Initialize())
            {
                Console.WriteLine("Failed to initialize ImGui");
            }

            GL.Enable(EnableCap.DepthTest);
            SDL.SDL_GetWindowSize(mainWindow, out int width, out int height);
            GL.Viewport(0, 0, width, height);
        }

        private void OnClose()
        {
            MonoBehaviourManager.OnApplicationQuit();
            imGuiControl.Shutdown();
        }

        protected override void OnUpdate()
        {
            MonoBehaviourManager.Update();
            MonoBehaviourManager.LateUpdate();
            CoroutineScheduler.Update();
            PhysicsPipeline.Update();
            MonoBehaviourManager.UpdateDestroyQueue();
        }

        protected override void OnRender()
        {
            render?.Invoke();
            RenderPipeline.Update();
            postRender?.Invoke();
        }

        protected override void OnRenderGUI()
        {            
            imGuiControl.NewFrame();            
        }

        private void OnGUI()
        {
            MonoBehaviourManager.OnGUI();
        }

        private void OnKeyDown(SDL.SDL_Scancode scancode)
        {
            Input.SetStateUp((KeyCode)(int)scancode, 0);
            Input.SetStateDown((KeyCode)(int)scancode, 1);
            imGuiControl.SetKeyDown(scancode);
        }

        private void OnKeyUp(SDL.SDL_Scancode scancode)
        {
            Input.SetStateDown((KeyCode)(int)scancode, 0);
            Input.SetStateUp((KeyCode)(int)scancode, 1);
            imGuiControl.SetKeyUp(scancode);
        }

        private void OnKeyPress(SDL.SDL_Scancode scancode)
        {
            
        }

        private void OnResize(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
            
            if(imGuiControl != null)
            {                
                imGuiControl.SetWindowSize(width, height);            
                SDL.SDL_GL_GetDrawableSize(mainWindow, out width, out height);
                imGuiControl.SetDrawableSize(width, height);
            }

            Camera.main.Initialize(Camera.main.fieldOfView, (float)width / (float)height, Camera.main.nearClipPlane, Camera.main.farClipPlane);
            Screen.SetSize(width, height);
        }

        private void OnMouseMove(int x, int y, int dx, int dy)
        {
            Input.SetMousePosition(new Vector2(x, y));
            Input.SetMouseDelta(new Vector2(dx, dy));
        }

        private void OnMouseDown(byte button)
        {
            imGuiControl.SetButtonDown(button);
            Input.SetMouseState((MouseButton)button, 0, 1, 0);
            
        }

        private void OnMouseUp(byte button)
        {
            imGuiControl.SetButtonUp(button);
            Input.SetMouseState((MouseButton)button, 1, 0, 0);
            
        }

        private void OnMouseScroll(float x, float y)
        {
            imGuiControl.SetScrollDirection(x, y);
            Input.SetMouseScrollDirection(y);
        }

        private void OnTextInput(IntPtr text)
        {
            imGuiControl.SetTextInput(text);
        }
    }
}