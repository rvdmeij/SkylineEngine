using System.Runtime.InteropServices;
using System;

namespace DearImGui
{
    public class ImGuiController
    {
        const string NATIVELIBNAME = "imgui";

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ImGui_OpenGL3_Init(string glsl_version);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGui_OpenGL3_NewFrame();
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGui_OpenGL3_RenderDrawData();
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGui_OpenGL3_Shutdown();
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ImGui_CustomBackend_InitForOpenGL(ImGuiImplCallbackTypes callbacks, ImGuiImplKeybindings keybindings, ImGuiImplCursorType cursorTypes, ImGuiImplMouseButton mouseButtons);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGui_CustomBackend_Shutdown();
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGui_CustomBackend_NewFrame(ImGuiImplFrameSettings frameSettings);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ImGui_CustomBackend_ProcessEvent(ImGuiImplWindowEventType evnt, ImGuiImplKeyboardState keyboardState, ImGuiImplMouseState mouseState, IntPtr inputText);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGui_Init();
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGui_NewFrame();
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGui_Render();
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGui_DestroyContext();

        public bool Initialize()
        {
            return InitializeForOpenGL();
        }

        public virtual bool InitializeForOpenGL()
        {
            return true;
        }        

        public virtual void Shutdown()
        {

        }

        public virtual void NewFrame()
        {
            
        }
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetClipboardTextCallback(string text);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate string GetClipboardTextCallback();
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ShowMouseCursorCallback(bool show);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetMouseCursorCallback(int index);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WarpMouseCallback(int x, int y);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GetMousePositionCallback(out int x, out int y);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void GetWindowPositionCallback(out int x, out int y);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int CreateCursorCallback(int type);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetMouseButtonCallback(int gdkButton);

    [StructLayout(LayoutKind.Sequential)]
    public struct ImGuiImplCallbackTypes
    {
        public SetClipboardTextCallback setClipboardText;
        public GetClipboardTextCallback getClipboardText;
        public ShowMouseCursorCallback showMouseCursor;
        public SetMouseCursorCallback setMouseCursor;
        public WarpMouseCallback warpMouse;
        public GetMousePositionCallback getMousePosition;
        public GetWindowPositionCallback getWindowPosition;
        public CreateCursorCallback createCursor;
        public GetMouseButtonCallback getMouseButton;
    }

    public enum ImGuiImplWindowEventType : int
    {
        MouseWheel,
        MouseButtonDown,
        MouseButtonUp,
        TextInput,
        KeyDown,
        KeyUp,
        None
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImGuiImplFrameSettings
    {
        public int windowWidth;
        public int windowHeight;
        public int drawableWidth;
        public int drawableHeight;
        public bool isMinimized;
        public float deltaTime;
    }    

    [StructLayout(LayoutKind.Sequential)]
    public struct ImGuiImplMouseState
    {
        public int buttonLeft;
        public int buttonRight;
        public int buttonMiddle;
        public int wheelX;
        public int wheelY;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImGuiImplKeyboardState
    {
        public int scancode;
        public int shiftIsDown;
        public int controlisDown;
        public int altIsDown;
        public int superIsDown;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImGuiImplCursorType
    {
        public int Arrow;
        public int TextInput;
        public int ResizeAll;
        public int ResizeNS;
        public int ResizeEW;
        public int ResizeNESW;
        public int ResizeNWSE;
        public int Hand;
        public int NotAllowed;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImGuiImplMouseButton
    {
        public int Left;
        public int Right;
        public int Middle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImGuiImplKeybindings
    {
        public int Tab;
        public int LeftArrow;
        public int RightArrow;
        public int UpArrow;
        public int DownArrow;
        public int PageUp;
        public int PageDown;
        public int Home;
        public int End;
        public int Insert;
        public int Delete;
        public int Backspace;
        public int Space;
        public int Enter;
        public int Escape;
        public int KeyPadEnter;
        public int A;
        public int C;
        public int V;
        public int X;
        public int Y;
        public int Z;
    }
}