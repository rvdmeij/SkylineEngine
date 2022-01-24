using System;
using System.Runtime.InteropServices;

namespace DearImGui
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate string GetClipboardTextFn(IntPtr user_data);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetClipboardTextFn(IntPtr user_data, string text);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ImeSetInputScreenPosFn(int x, int y);

    [StructLayout(LayoutKind.Explicit, Size = 5464)]
    public unsafe struct ImGuiIO
    {
        [FieldOffset(0)] public ImGuiConfigFlags ConfigFlags;
        [FieldOffset(4)] public ImGuiBackendFlags BackendFlags;
        [FieldOffset(8)] public ImVec2 DisplaySize;
        [FieldOffset(16)] public float DeltaTime;
        [FieldOffset(20)] public float IniSavingRate;
        [FieldOffset(24)] public string IniFilename;
        [FieldOffset(32)] public string LogFilename;
        [FieldOffset(40)] public float MouseDoubleClickTime;
        [FieldOffset(44)] public float MouseDoubleClickMaxDist;
        [FieldOffset(48)] public float MouseDragThreshold;
        [FieldOffset(52)] public fixed int KeyMap[(int)ImGuiKey.COUNT];
        [FieldOffset(140)] public float KeyRepeatDelay;
        [FieldOffset(144)] public float KeyRepeatRate;
        [FieldOffset(152)] public IntPtr UserData;
        [FieldOffset(160)] public IntPtr Fonts;
        [FieldOffset(168)] public float FontGlobalScale;
        [FieldOffset(172)] public bool FontAllowUserScaling;
        [FieldOffset(176)] public IntPtr FontDefault;
        [FieldOffset(184)] public ImVec2 DisplayFramebufferScale;
        [FieldOffset(192)] public bool MouseDrawCursor;
        [FieldOffset(193)] public bool ConfigMacOSXBehaviors;
        [FieldOffset(194)] public bool ConfigInputTextCursorBlink;
        [FieldOffset(195)] public bool ConfigWindowsResizeFromEdges;
        [FieldOffset(196)] public bool ConfigWindowsMoveFromTitleBarOnly;
        [FieldOffset(200)] public float ConfigMemoryCompactTimer;
        [FieldOffset(208)] public string BackendPlatformName;
        [FieldOffset(216)] public string BackendRendererName;
        [FieldOffset(224)] public IntPtr BackendPlatformUserData;
        [FieldOffset(232)] public IntPtr BackendRendererUserData;
        [FieldOffset(240)] public IntPtr BackendLanguageUserData;
        [FieldOffset(248)] public GetClipboardTextFn GetClipboardText;
        [FieldOffset(256)] public SetClipboardTextFn SetClipboardText;
        [FieldOffset(264)] public IntPtr ClipboardUserData;
        [FieldOffset(272)] public ImeSetInputScreenPosFn ImeSetInputScreenPos;
        [FieldOffset(280)] public IntPtr ImeWindowHandle;
        [FieldOffset(288)] public ImVec2 MousePos;
        [FieldOffset(296)] public fixed bool MouseDown[5];
        [FieldOffset(304)] public float MouseWheel;
        [FieldOffset(308)] public float MouseWheelH;
        [FieldOffset(312)] public bool KeyCtrl;
        [FieldOffset(313)] public bool KeyShift;
        [FieldOffset(314)] public bool KeyAlt;
        [FieldOffset(315)] public bool KeySuper;
        [FieldOffset(316)] public fixed bool KeysDown[512];
        [FieldOffset(828)] public fixed float NavInputs[(int)ImGuiNavInput.COUNT];
        [FieldOffset(912)] public bool WantCaptureMouse;
        [FieldOffset(913)] public bool WantCaptureKeyboard;
        [FieldOffset(914)] public bool WantTextInput;
        [FieldOffset(915)] public bool WantSetMousePos;
        [FieldOffset(916)] public bool WantSaveIniSettings;
        [FieldOffset(917)] public bool NavActive;
        [FieldOffset(918)] public bool NavVisible;
        [FieldOffset(920)] public float Framerate;
        [FieldOffset(924)] public int MetricsRenderVertices;
        [FieldOffset(928)] public int MetricsRenderIndices;
        [FieldOffset(932)] public int MetricsRenderWindows;
        [FieldOffset(936)] public int MetricsActiveWindows;
        [FieldOffset(940)] public int MetricsActiveAllocations;
        [FieldOffset(944)] public ImVec2 MouseDelta;
        [FieldOffset(952)] public ImGuiKeyModFlags KeyMods;
        [FieldOffset(956)] public ImVec2 MousePosPrev;
        [FieldOffset(964)] public ImVec2Array MouseClickedPos;
        [FieldOffset(1008)] public fixed double MouseClickedTime[5];
        [FieldOffset(1048)] public fixed bool MouseClicked[5];
        [FieldOffset(1053)] public fixed bool MouseDoubleClicked[5];
        [FieldOffset(1058)] public fixed bool MouseReleased[5];
        [FieldOffset(1063)] public fixed bool MouseDownOwned[5];
        [FieldOffset(1068)] public fixed bool MouseDownWasDoubleClick[5];
        [FieldOffset(1076)] public fixed float MouseDownDuration[5];
        [FieldOffset(1096)] public fixed float MouseDownDurationPrev[5];
        [FieldOffset(1116)] public ImVec2Array MouseDragMaxDistanceAbs;
        [FieldOffset(1156)] public fixed float MouseDragMaxDistanceSqr[5];
        [FieldOffset(1176)] public fixed float KeysDownDuration[512];
        [FieldOffset(3224)] public fixed float KeysDownDurationPrev[512];
        [FieldOffset(5272)] public fixed float NavInputsDownDuration[(int)ImGuiNavInput.COUNT];
        [FieldOffset(5356)] public fixed float NavInputsDownDurationPrev[(int)ImGuiNavInput.COUNT];
        [FieldOffset(5440)] public float PenPressure;
        [FieldOffset(5444)] public ushort InputQueueSurrogate;
        [FieldOffset(5448)] public ImVector InputQueueCharacters;
    }

    // public unsafe struct ImGuiIO2
    // {
    //     public ImGuiConfigFlags ConfigFlags;
    //     public ImGuiBackendFlags BackendFlags;
    //     public ImVec2 DisplaySize;
    //     public float DeltaTime;
    //     public float IniSavingRate;
    //     public byte* IniFilename;
    //     public byte* LogFilename;
    //     public float MouseDoubleClickTime;
    //     public float MouseDoubleClickMaxDist;
    //     public float MouseDragThreshold;
    //     public fixed int KeyMap[22];
    //     public float KeyRepeatDelay;
    //     public float KeyRepeatRate;
    //     public void* UserData;
    //     public IntPtr Fonts;
    //     public float FontGlobalScale;
    //     public byte FontAllowUserScaling;
    //     public IntPtr FontDefault;
    //     public ImVec2 DisplayFramebufferScale;
    //     public byte MouseDrawCursor;
    //     public byte ConfigMacOSXBehaviors;
    //     public byte ConfigInputTextCursorBlink;
    //     public byte ConfigWindowsResizeFromEdges;
    //     public byte ConfigWindowsMoveFromTitleBarOnly;
    //     public float ConfigMemoryCompactTimer;
    //     public byte* BackendPlatformName;
    //     public byte* BackendRendererName;
    //     public void* BackendPlatformUserData;
    //     public void* BackendRendererUserData;
    //     public void* BackendLanguageUserData;
    //     public IntPtr GetClipboardTextFn;
    //     public IntPtr SetClipboardTextFn;
    //     public void* ClipboardUserData;
    //     public IntPtr ImeSetInputScreenPosFn;
    //     public IntPtr ImeWindowHandle;
    //     public ImVec2 MousePos;
    //     public fixed byte MouseDown[5];
    //     public float MouseWheel;
    //     public float MouseWheelH;
    //     public byte KeyCtrl;
    //     public byte KeyShift;
    //     public byte KeyAlt;
    //     public byte KeySuper;
    //     public fixed byte KeysDown[512];
    //     public fixed float NavInputs[21];
    //     public byte WantCaptureMouse;
    //     public byte WantCaptureKeyboard;
    //     public byte WantTextInput;
    //     public byte WantSetMousePos;
    //     public byte WantSaveIniSettings;
    //     public byte NavActive;
    //     public byte NavVisible;
    //     public float Framerate;
    //     public int MetricsRenderVertices;
    //     public int MetricsRenderIndices;
    //     public int MetricsRenderWindows;
    //     public int MetricsActiveWindows;
    //     public int MetricsActiveAllocations;
    //     public ImVec2 MouseDelta;
    //     public ImGuiKeyModFlags KeyMods;
    //     public ImVec2 MousePosPrev;
    //     public ImVec2Array MouseClickedPos;
    //     public fixed double MouseClickedTime[5];
    //     public fixed byte MouseClicked[5];
    //     public fixed byte MouseDoubleClicked[5];
    //     public fixed byte MouseReleased[5];
    //     public fixed byte MouseDownOwned[5];
    //     public fixed byte MouseDownWasDoubleClick[5];
    //     public fixed float MouseDownDuration[5];
    //     public fixed float MouseDownDurationPrev[5];
    //     public ImVec2Array MouseDragMaxDistanceAbs;
    //     public fixed float MouseDragMaxDistanceSqr[5];
    //     public fixed float KeysDownDuration[512];
    //     public fixed float KeysDownDurationPrev[512];
    //     public fixed float NavInputsDownDuration[21];
    //     public fixed float NavInputsDownDurationPrev[21];
    //     public float PenPressure;
    //     public ushort InputQueueSurrogate;
    //     public ImVector InputQueueCharacters;
    // }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ImVec2Array 
    {
        public const int MAX_LENGTH = 5*2;
        public fixed float fixedBuffer[MAX_LENGTH];

        public ImVec2 this[int index] 
        {
            get
            {
                index *= 2;
                return new ImVec2(fixedBuffer[index + 0], fixedBuffer[index + 1]);
            }
            set
            {
                index *= 2;
                fixedBuffer[index+0] = value.x;
                fixedBuffer[index+1] = value.y;
            }
        }
    }
}