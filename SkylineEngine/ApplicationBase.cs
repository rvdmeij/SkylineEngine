using System;
using SDL2;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SkylineEngine
{
    public delegate void CreatedEvent();
    public delegate void CloseEvent();
    public delegate void RenderEvent();
    public delegate void GUIEvent();
    public delegate void UpdateEvent();
    public delegate void KeyDownEvent(SDL.SDL_Scancode scancode);
    public delegate void KeyUpEvent(SDL.SDL_Scancode scancode);
    public delegate void KeyPressEvent(SDL.SDL_Scancode scancode);
    public delegate void ResizeEvent(int width, int height);
    public delegate void MouseMoveEvent(int x, int y, int dx, int dy);
    public delegate void MouseDownEvent(byte button);
    public delegate void MouseUpEvent(byte button);
    public delegate void MouseScrollEvent(float x, float y);
    public delegate void TextInputEvent(IntPtr text);

    public class ApplicationBase
    {
        [DllImport("imgui", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint GLInitGlew(bool experimental);

        public event CreatedEvent created;
        public event CloseEvent close;        
        
        public event KeyDownEvent keyDown;
        public event KeyUpEvent keyUp;
        public event KeyPressEvent keyPress;
        public event ResizeEvent resize;
        public event MouseMoveEvent mouseMove;
        public event MouseDownEvent mouseDown;
        public event MouseUpEvent mouseUp;
        public event MouseScrollEvent mouseScroll;
        public event TextInputEvent textInput;

        private string title;
        private int width;
        private int height;
        protected IntPtr mainWindow;
        protected IntPtr mainContext;
        private IntPtr renderer;
        private bool run;
        private int versionMajor;
        private int versionMinor;
        private bool vsync;

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public IntPtr Window
        {
            get { return mainWindow; }
        }

        public ApplicationBase(string title, int width, int height, int versionMajor, int versionMinor, bool vsync = true)
        {
            this.title = title;
            this.width = width;
            this.height = height;
            this.versionMajor = versionMajor;
            this.versionMinor = versionMinor;
            this.vsync = vsync;
        }

        private bool Create()
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING) < 0)
            {
                Console.WriteLine("Failed to init SDL");
                return false;
            }

            mainWindow = SDL.SDL_CreateWindow(title, SDL.SDL_WINDOWPOS_CENTERED, SDL.SDL_WINDOWPOS_CENTERED, width, height, SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

            if (mainWindow == IntPtr.Zero)
            {
                Console.WriteLine("Unable to create window");
                CheckSDLError(85);
                return false;
            }

            SDL.SDL_SetWindowMinimumSize(mainWindow, width, height);

            SetOpenGLAttributes();

            mainContext = SDL.SDL_GL_CreateContext(mainWindow);
            renderer = SDL.SDL_GetRenderer(mainWindow);           
            SDL.SDL_GL_MakeCurrent(mainWindow, mainContext);

            InitializeGlBindings();
            GLInitGlew(true);

            SDL.SDL_GL_SetSwapInterval(vsync ? 1 : 0);
            return true;
        }

        private bool SetOpenGLAttributes()
        {
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, SDL.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, versionMajor);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, versionMinor);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_RED_SIZE, 8);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_GREEN_SIZE, 8);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_BLUE_SIZE, 8);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_ALPHA_SIZE, 8);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_BUFFER_SIZE, 32);

            // Turn on double buffering with a 24bit Z buffer.
            // You may need to change this to 16 or 32 for your system
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_DOUBLEBUFFER, 1);

            //Anti aliasing
            //SDL_GL_SetAttribute(SDL_GL_MULTISAMPLESAMPLES, 2);
            //glEnable(GL_MULTISAMPLE);

            return true;
        }

        private void CheckSDLError(int line)
        {
            string error = SDL.SDL_GetError();

            if (error != "")
            {
                Console.WriteLine("SDL Error: " + error);

                if (line != -1)
                    Console.WriteLine("Line: " + line);

                SDL.SDL_ClearError();
            }
        }

        int counter = 0;

        public void Run()
        {
            if(Create())
            {
                created?.Invoke();
                run = true;
            }

            while (run)
            {
                SDL.SDL_Event evnt;

                while (SDL.SDL_PollEvent(out evnt) > 0)
                {
                    if (evnt.type == SDL.SDL_EventType.SDL_QUIT)
                    {
                        run = false;
                    }
                    else if (evnt.type == SDL.SDL_EventType.SDL_KEYDOWN)
                    {
                        if(evnt.key.repeat == 0)
                            keyDown?.Invoke(evnt.key.keysym.scancode);
                        else
                            keyPress?.Invoke(evnt.key.keysym.scancode);
                    }
                    else if (evnt.type == SDL.SDL_EventType.SDL_KEYUP)
                    {
                        keyUp?.Invoke(evnt.key.keysym.scancode);
                    }
                    else if(evnt.type == SDL.SDL_EventType.SDL_WINDOWEVENT)
                    {                        
                        if(evnt.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED)
                        {
                            SDL.SDL_GetWindowSize(mainWindow, out width, out height);
                            resize?.Invoke(width, height);
                        }
                        else if(evnt.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_SHOWN)
                        {
                            SDL.SDL_GetWindowSize(mainWindow, out width, out height);                            
                            resize?.Invoke(width, height);
                        }
                    }
                    else if(evnt.type == SDL.SDL_EventType.SDL_MOUSEMOTION)
                    {
                        SDL.SDL_GetMouseState(out int x, out int y);                        
                        mouseMove?.Invoke(x, y, evnt.motion.xrel, evnt.motion.yrel);
                        //Debug.Log("Mouse motion " + counter);
                        counter++;
                    }
                    else if(evnt.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN)
                    {
                        mouseDown?.Invoke(evnt.button.button);
                    }
                    else if(evnt.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP)
                    {
                        mouseUp?.Invoke(evnt.button.button);
                    }
                    else if(evnt.type == SDL.SDL_EventType.SDL_MOUSEWHEEL)
                    {
                        mouseScroll?.Invoke(evnt.wheel.x, evnt.wheel.y);
                    }
                    else if(evnt.type == SDL.SDL_EventType.SDL_TEXTINPUT)
                    {
                        unsafe
                        {
                            textInput?.Invoke(new IntPtr(evnt.text.text));
                        }
                    }
                }

                OnUpdate();
                OnRender();
                OnRenderGUI();

                SDL.SDL_GL_SwapWindow(mainWindow);
            }

            Close();
        }

        protected virtual void OnRender()
        {
            
        }

        protected virtual void OnUpdate()
        {
            
        }

        protected virtual void OnRenderGUI()
        {
            
        }

        public void ToggleCursor(bool enabled)
        {
            if(enabled)
                SDL.SDL_ShowCursor(SDL.SDL_ENABLE);
            else
                SDL.SDL_ShowCursor(SDL.SDL_DISABLE);
        }

        public void LockCursor(bool locked)
        {
            if(locked)
                SDL.SDL_SetRelativeMouseMode(SDL.SDL_bool.SDL_TRUE);
            else
                SDL.SDL_SetRelativeMouseMode(SDL.SDL_bool.SDL_FALSE);
        }

        public void SetWindowSize(int width, int height)
        {
            SDL.SDL_SetWindowSize(mainWindow, width, height);
        }

        public void WarpMousePosition(int x, int y)
        {
            SDL.SDL_WarpMouseInWindow(mainWindow, x, y);
        }

        private void Close()
        {
            run = false;
            SDL.SDL_GL_MakeCurrent(mainWindow, mainContext);
            close?.Invoke();
            Dispose();
        }

        private void Dispose()
        {
            SDL.SDL_ShowCursor(SDL.SDL_ENABLE);
            SDL.SDL_GL_DeleteContext(mainContext);
            SDL.SDL_DestroyWindow(mainWindow);
            SDL.SDL_DestroyRenderer(renderer);
            SDL.SDL_Quit();
        }

        private static void InitializeGlBindings()
        {
            // We don't put a hard dependency on OpenTK.Graphics here.
            // So we need to use reflection to initialize the GL bindings, so users don't have to.

            // Try to load OpenTK.Graphics assembly.
            Assembly assembly;
            try
            {
                assembly = Assembly.Load("OpenTK.Graphics");
            }
            catch
            {
                // Failed to load graphics, oh well.
                // Up to the user I guess?
                // TODO: Should we expose this load failure to the user better?
                return;
            }

            var provider = new SDLBindingsContext();

            void LoadBindings(string typeNamespace)
            {
                var type = assembly.GetType($"OpenTK.Graphics.{typeNamespace}.GL");
                if (type == null)
                {
                    return;
                }

                var load = type.GetMethod("LoadBindings");
                load.Invoke(null, new object[] { provider });
            }

            LoadBindings("ES11");
            LoadBindings("ES20");
            LoadBindings("ES30");
            LoadBindings("OpenGL");
            LoadBindings("OpenGL4");
        }
    }
}
