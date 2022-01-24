using System;

namespace DearImGui
{
    [Flags]
    public enum ImGuiKeyModFlags : int
    {
        ImGuiKeyModFlags_None       = 0,
        ImGuiKeyModFlags_Ctrl       = 1 << 0,
        ImGuiKeyModFlags_Shift      = 1 << 1,
        ImGuiKeyModFlags_Alt        = 1 << 2,
        ImGuiKeyModFlags_Super      = 1 << 3
    }
}