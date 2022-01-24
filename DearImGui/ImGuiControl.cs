using System;
using System.Collections.Generic;
using SDL2;
using SkylineEngine;

namespace DearImGui
{
    public delegate void RenderGUIEvent();

    public class ImGuiControl : ImGuiController
    {
        public event RenderGUIEvent onGUI;
        private IntPtr sdlWindow;
        private List<IntPtr> cursors;
        private ImGuiImplFrameSettings frameSettings;
        private ImGuiImplKeyboardState keyboardState;
        private ImGuiImplMouseState mouseState;
        private ImGuiImplCallbackTypes callbacks;
        private ImGuiImplCursorType cursorTypes;
        private ImGuiImplMouseButton mouseButtons;
        private ImGuiImplKeybindings keybindings;
        private int currentCursorIndex;

        public ImGuiControl(IntPtr sdlWindow)
        {
            this.sdlWindow = sdlWindow;
            this.cursors = new List<IntPtr>();
            this.frameSettings = new ImGuiImplFrameSettings();
            this.keyboardState = new ImGuiImplKeyboardState();
            this.mouseState = new ImGuiImplMouseState();            

            this.callbacks = new ImGuiImplCallbackTypes();
            this.callbacks.getClipboardText = GetClipboardText;
            this.callbacks.setClipboardText = SetClipboardText;
            this.callbacks.showMouseCursor = ShowMouseCursor;
            this.callbacks.setMouseCursor = SetMouseCursor;
            this.callbacks.warpMouse = WarpMouse;
            this.callbacks.getMousePosition = GetMousePosition;
            this.callbacks.getWindowPosition = GetWindowPosition;
            this.callbacks.createCursor = CreateCursor;
            this.callbacks.getMouseButton = GetMouseButton;

            this.cursorTypes = new ImGuiImplCursorType();
            this.mouseButtons = new ImGuiImplMouseButton();
            this.keybindings = new ImGuiImplKeybindings();

            this.cursorTypes.Arrow = (int)SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_ARROW;
            this.cursorTypes.TextInput = (int)SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_IBEAM;
            this.cursorTypes.ResizeAll = (int)SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_SIZEALL;
            this.cursorTypes.ResizeNS = (int)SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_SIZENS;
            this.cursorTypes.ResizeEW = (int)SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_SIZEWE;
            this.cursorTypes.ResizeNESW = (int)SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_SIZENESW;
            this.cursorTypes.ResizeNWSE = (int)SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_SIZENWSE;
            this.cursorTypes.Hand = (int)SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_HAND;
            this.cursorTypes.NotAllowed = (int)SDL.SDL_SystemCursor.SDL_SYSTEM_CURSOR_NO;

            this.mouseButtons.Left = 1;
            this.mouseButtons.Right = 3;
            this.mouseButtons.Middle = 2;

            this.keybindings.Tab = (int)SDL.SDL_Scancode.SDL_SCANCODE_TAB;
            this.keybindings.LeftArrow = (int)SDL.SDL_Scancode.SDL_SCANCODE_LEFT;
            this.keybindings.RightArrow = (int)SDL.SDL_Scancode.SDL_SCANCODE_RIGHT;
            this.keybindings.UpArrow = (int)SDL.SDL_Scancode.SDL_SCANCODE_UP;
            this.keybindings.DownArrow = (int)SDL.SDL_Scancode.SDL_SCANCODE_DOWN;
            this.keybindings.PageUp = (int)SDL.SDL_Scancode.SDL_SCANCODE_PAGEUP;
            this.keybindings.PageDown = (int)SDL.SDL_Scancode.SDL_SCANCODE_PAGEDOWN;
            this.keybindings.Home = (int)SDL.SDL_Scancode.SDL_SCANCODE_HOME;
            this.keybindings.End = (int)SDL.SDL_Scancode.SDL_SCANCODE_END;
            this.keybindings.Insert = (int)SDL.SDL_Scancode.SDL_SCANCODE_INSERT;
            this.keybindings.Delete = (int)SDL.SDL_Scancode.SDL_SCANCODE_DELETE;
            this.keybindings.Backspace = (int)SDL.SDL_Scancode.SDL_SCANCODE_BACKSPACE;
            this.keybindings.Space = (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE;
            this.keybindings.Enter = (int)SDL.SDL_Scancode.SDL_SCANCODE_RETURN;
            this.keybindings.Escape = (int)SDL.SDL_Scancode.SDL_SCANCODE_ESCAPE;
            this.keybindings.KeyPadEnter = (int)SDL.SDL_Scancode.SDL_SCANCODE_KP_ENTER;
            this.keybindings.A = (int)SDL.SDL_Scancode.SDL_SCANCODE_A;
            this.keybindings.C = (int)SDL.SDL_Scancode.SDL_SCANCODE_C;
            this.keybindings.V = (int)SDL.SDL_Scancode.SDL_SCANCODE_V;
            this.keybindings.X = (int)SDL.SDL_Scancode.SDL_SCANCODE_X;
            this.keybindings.Y = (int)SDL.SDL_Scancode.SDL_SCANCODE_Y;
            this.keybindings.Z = (int)SDL.SDL_Scancode.SDL_SCANCODE_Z;
        }

        public override bool InitializeForOpenGL()
        {
            ImGui_Init();
            if(ImGui_CustomBackend_InitForOpenGL(callbacks, keybindings, cursorTypes, mouseButtons))
            {
                return ImGui_OpenGL3_Init("#version 330");
            }
            return false;
        }

        public override void NewFrame()
        {
            frameSettings.deltaTime = Time.deltaTime;
            int minimized = (int)SDL.SDL_GetWindowFlags(sdlWindow) & (int)SDL.SDL_WindowFlags.SDL_WINDOW_MINIMIZED;

            if(minimized > 0)
            {
                frameSettings.isMinimized = true;
            }
            else
            {
                frameSettings.isMinimized = false;
            }

            ImGui_OpenGL3_NewFrame();
            ImGui_CustomBackend_NewFrame(frameSettings);
            ImGui_NewFrame();
            onGUI?.Invoke();
            ImGui_Render();
            ImGui_OpenGL3_RenderDrawData();
        }
        
        public override void Shutdown()
        {
	        ImGui_OpenGL3_Shutdown();
	        ImGui_CustomBackend_Shutdown();

            for(int i = 0; i < cursors.Count; i++)
                SDL.SDL_FreeCursor(cursors[i]);

	        ImGui_DestroyContext();
        }

        private string GetClipboardText()
        {       
            return SDL.SDL_GetClipboardText();
        }

        private void SetClipboardText(string text)
        {
            SDL.SDL_SetClipboardText(text);
        }

        private void ShowMouseCursor(bool visible)
        {
            SDL.SDL_ShowCursor(visible ? SDL.SDL_ENABLE : SDL.SDL_DISABLE);
        }

        private void SetMouseCursor(int index)
        {
            if(index >= cursors.Count)
                return;

            if(index == currentCursorIndex)
                return;

            currentCursorIndex = index;            
            SDL.SDL_SetCursor(cursors[index]);
        }

        private void WarpMouse(int x, int y)
        {
            SDL.SDL_WarpMouseInWindow(sdlWindow, x, y);
        }

        private void GetMousePosition(out int x, out int y)
        {
            SDL.SDL_GetMouseState(out x, out y);
        }

        private void GetWindowPosition(out int x, out int y)
        {
            SDL.SDL_GetWindowPosition(sdlWindow, out x, out y);
        }

        private int CreateCursor(int type)
        {            
            SDL.SDL_SystemCursor t = (SDL.SDL_SystemCursor)type;
            var cursor = SDL.SDL_CreateSystemCursor(t);
            cursors.Add(cursor);
            return cursors.Count - 1;
        }

        private int GetMouseButton(int button)
        {
            if(button == 1)
            {
                return mouseState.buttonLeft;
            }
            if(button == 3)
            {
                return mouseState.buttonRight;
            }
            if(button == 2)
            {
                return mouseState.buttonMiddle;
            }
            return 0;
        }

        public void SetWindowSize(int width, int height)
        {
            frameSettings.windowWidth = width;
            frameSettings.windowHeight = height;
        }

        public void SetDrawableSize(int width, int height)
        {
            frameSettings.drawableWidth = width;
            frameSettings.drawableHeight = height;
        }

        public void SetKeyDown(SDL.SDL_Scancode key)
        {            
            keyboardState.altIsDown = (int)SDL.SDL_GetModState() & (int)SDL.SDL_Keymod.KMOD_ALT;            
            keyboardState.shiftIsDown = (int)SDL.SDL_GetModState() & (int)SDL.SDL_Keymod.KMOD_SHIFT;
            keyboardState.controlisDown = (int)SDL.SDL_GetModState() & (int)SDL.SDL_Keymod.KMOD_CTRL;
            keyboardState.superIsDown = (int)SDL.SDL_GetModState() & (int)SDL.SDL_Keymod.KMOD_GUI;            
            keyboardState.scancode = (int)key;
            ImGui_CustomBackend_ProcessEvent(ImGuiImplWindowEventType.KeyDown, keyboardState, mouseState, IntPtr.Zero);
        }

        public void SetKeyUp(SDL.SDL_Scancode key)
        {
            keyboardState.altIsDown = (int)SDL.SDL_GetModState() & (int)SDL.SDL_Keymod.KMOD_ALT;            
            keyboardState.shiftIsDown = (int)SDL.SDL_GetModState() & (int)SDL.SDL_Keymod.KMOD_SHIFT;
            keyboardState.controlisDown = (int)SDL.SDL_GetModState() & (int)SDL.SDL_Keymod.KMOD_CTRL;
            keyboardState.superIsDown = (int)SDL.SDL_GetModState() & (int)SDL.SDL_Keymod.KMOD_GUI;            
            keyboardState.scancode = (int)key;
            ImGui_CustomBackend_ProcessEvent(ImGuiImplWindowEventType.KeyUp, keyboardState, mouseState, IntPtr.Zero);
        }

        public void SetTextInput(IntPtr text)
        {
            ImGui_CustomBackend_ProcessEvent(ImGuiImplWindowEventType.TextInput, keyboardState, mouseState, text);
        }

        public void SetButtonDown(uint button)
        {
            if(button == 1)
                mouseState.buttonLeft = 1;
            if(button == 3)
                mouseState.buttonRight = 1;
            if(button == 2)
                mouseState.buttonMiddle = 1;

            ImGui_CustomBackend_ProcessEvent(ImGuiImplWindowEventType.MouseButtonDown, keyboardState, mouseState, IntPtr.Zero);
        }

        public void SetButtonUp(uint button)
        {
            if(button == 1)
                mouseState.buttonLeft = 0;
            if(button == 3)
                mouseState.buttonRight = 0;
            if(button == 2)
                mouseState.buttonMiddle = 0;

            ImGui_CustomBackend_ProcessEvent(ImGuiImplWindowEventType.MouseButtonUp, keyboardState, mouseState, IntPtr.Zero);
        }

        public void SetScrollDirection(float x, float y)
        {    
            int direction = 0;

            if(y > 0 || y < 0)
            {
                direction = y > 0 ? 1 : -1;
                mouseState.wheelY = direction;
                mouseState.wheelX = 0;
                
            }
            if(x > 0 || x < 0)
            {
                direction = x > 0 ? 1 : -1;
                mouseState.wheelX = direction;
                mouseState.wheelY = 0;
            }

            if(direction != 0)
                ImGui_CustomBackend_ProcessEvent(ImGuiImplWindowEventType.MouseWheel, keyboardState, mouseState, IntPtr.Zero);
        }
    }
}