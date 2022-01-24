using System;

namespace DearImGui
{
    [Flags]
    public enum ImGuiFocusedFlags : int
    {
        None                          = 0,
        ChildWindows                  = 1 << 0,   // IsWindowFocused(): Return true if any children of the window is focused
        RootWindow                    = 1 << 1,   // IsWindowFocused(): Test from root window (top most parent of the current hierarchy)
        AnyWindow                     = 1 << 2,   // IsWindowFocused(): Return true if any window is focused. Important: If you are trying to tell how to dispatch your low-level inputs, do NOT use this. Use 'io.WantCaptureMouse' instead! Please read the FAQ!
        RootAndChildWindows           = RootWindow | ChildWindows
    }
}