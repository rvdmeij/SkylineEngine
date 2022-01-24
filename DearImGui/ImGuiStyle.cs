using System.Runtime.InteropServices;

namespace DearImGui
{
    [StructLayout(LayoutKind.Explicit, Size = 1044)]
    public unsafe struct ImGuiStyle
    {
        [FieldOffset(0)]   public float       Alpha;                      // Global alpha applies to everything in Dear ImGui.
        [FieldOffset(4)]   public ImVec2      WindowPadding;              // Padding within a window.
        [FieldOffset(12)]  public float       WindowRounding;             // Radius of window corners rounding. Set to 0.0f to have rectangular windows. Large values tend to lead to variety of artifacts and are not recommended.
        [FieldOffset(16)]  public float       WindowBorderSize;           // Thickness of border around windows. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly).
        [FieldOffset(20)]  public ImVec2      WindowMinSize;              // Minimum window size. This is a global setting. If you want to constraint individual windows, use SetNextWindowSizeConstraints().
        [FieldOffset(28)]  public ImVec2      WindowTitleAlign;           // Alignment for title bar text. Defaults to (0.0f,0.5f) for left-aligned,vertically centered.
        [FieldOffset(36)]  public ImGuiDir    WindowMenuButtonPosition;   // Side of the collapsing/docking button in the title bar (None/Left/Right). Defaults to ImGuiDir_Left.
        [FieldOffset(40)]  public float       ChildRounding;              // Radius of child window corners rounding. Set to 0.0f to have rectangular windows.
        [FieldOffset(44)]  public float       ChildBorderSize;            // Thickness of border around child windows. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly).
        [FieldOffset(48)]  public float       PopupRounding;              // Radius of popup window corners rounding. (Note that tooltip windows use WindowRounding)
        [FieldOffset(52)]  public float       PopupBorderSize;            // Thickness of border around popup/tooltip windows. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly).
        [FieldOffset(56)]  public ImVec2      FramePadding;               // Padding within a framed rectangle (used by most widgets).
        [FieldOffset(64)]  public float       FrameRounding;              // Radius of frame corners rounding. Set to 0.0f to have rectangular frame (used by most widgets).
        [FieldOffset(68)]  public float       FrameBorderSize;            // Thickness of border around frames. Generally set to 0.0f or 1.0f. (Other values are not well tested and more CPU/GPU costly).
        [FieldOffset(72)]  public ImVec2      ItemSpacing;                // Horizontal and vertical spacing between widgets/lines.
        [FieldOffset(80)]  public ImVec2      ItemInnerSpacing;           // Horizontal and vertical spacing between within elements of a composed widget (e.g. a slider and its label).
        [FieldOffset(88)]  public ImVec2      CellPadding;                // Padding within a table cell
        [FieldOffset(96)]  public ImVec2      TouchExtraPadding;          // Expand reactive bounding box for touch-based system where touch position is not accurate enough. Unfortunately we don't sort widgets so priority on overlap will always be given to the first widget. So don't grow this too much!
        [FieldOffset(104)] public float       IndentSpacing;              // Horizontal indentation when e.g. entering a tree node. Generally == (FontSize + FramePadding.x*2).
        [FieldOffset(108)] public float       ColumnsMinSpacing;          // Minimum horizontal spacing between two columns. Preferably > (FramePadding.x + 1).
        [FieldOffset(112)] public float       ScrollbarSize;              // Width of the vertical scrollbar, Height of the horizontal scrollbar.
        [FieldOffset(116)] public float       ScrollbarRounding;          // Radius of grab corners for scrollbar.
        [FieldOffset(120)] public float       GrabMinSize;                // Minimum width/height of a grab box for slider/scrollbar.
        [FieldOffset(124)] public float       GrabRounding;               // Radius of grabs corners rounding. Set to 0.0f to have rectangular slider grabs.
        [FieldOffset(128)] public float       LogSliderDeadzone;          // The size in pixels of the dead-zone around zero on logarithmic sliders that cross zero.
        [FieldOffset(132)] public float       TabRounding;                // Radius of upper corners of a tab. Set to 0.0f to have rectangular tabs.
        [FieldOffset(136)] public float       TabBorderSize;              // Thickness of border around tabs.
        [FieldOffset(140)] public float       TabMinWidthForCloseButton;  // Minimum width for close button to appears on an unselected tab when hovered. Set to 0.0f to always show when hovering, set to FLT_MAX to never show close button unless selected.
        [FieldOffset(144)] public ImGuiDir    ColorButtonPosition;        // Side of the color button in the ColorEdit4 widget (left/right). Defaults to ImGuiDir_Right.
        [FieldOffset(148)] public ImVec2      ButtonTextAlign;            // Alignment of button text when button is larger than text. Defaults to (0.5f, 0.5f) (centered).
        [FieldOffset(156)] public ImVec2      SelectableTextAlign;        // Alignment of selectable text. Defaults to (0.0f, 0.0f) (top-left aligned). It's generally important to keep this left-aligned if you want to lay multiple items on a same line.
        [FieldOffset(164)] public ImVec2      DisplayWindowPadding;       // Window position are clamped to be visible within the display area or monitors by at least this amount. Only applies to regular windows.
        [FieldOffset(172)] public ImVec2      DisplaySafeAreaPadding;     // If you cannot see the edges of your screen (e.g. on a TV) increase the safe area padding. Apply to popups/tooltips as well regular windows. NB: Prefer configuring your TV sets correctly!
        [FieldOffset(180)] public float       MouseCursorScale;           // Scale software rendered mouse cursor (when io.MouseDrawCursor is enabled). May be removed later.
        [FieldOffset(184)] public bool        AntiAliasedLines;           // Enable anti-aliased lines/borders. Disable if you are really tight on CPU/GPU. Latched at the beginning of the frame (copied to ImDrawList).
        [FieldOffset(185)] public bool        AntiAliasedLinesUseTex;     // Enable anti-aliased lines/borders using textures where possible. Require backend to render with bilinear filtering. Latched at the beginning of the frame (copied to ImDrawList).
        [FieldOffset(186)] public bool        AntiAliasedFill;            // Enable anti-aliased edges around filled shapes (rounded rectangles, circles, etc.). Disable if you are really tight on CPU/GPU. Latched at the beginning of the frame (copied to ImDrawList).
        [FieldOffset(188)] public float       CurveTessellationTol;       // Tessellation tolerance when using PathBezierCurveTo() without a specific number of segments. Decrease for highly tessellated curves (higher quality, more polygons), increase to reduce quality.
        [FieldOffset(192)] public float       CircleSegmentMaxError;      // Maximum error (in pixels) allowed when using AddCircle()/AddCircleFilled() or drawing rounded corner rectangles with no explicit segment count specified. Decrease for higher quality but more geometry.
        [FieldOffset(196)] public ImGuiColors Colors;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct ImGuiColors 
    {
        public const int MAX_LENGTH = 53*4;
        public fixed float fixedBuffer[MAX_LENGTH];

        public ImVec4 this[ImGuiCol key] 
        {
            get
            {
                int index = (int)key;
                index *= 4;
                return new ImVec4(fixedBuffer[index + 0], fixedBuffer[index + 1], fixedBuffer[index + 2], fixedBuffer[index + 3]);
            }
            set
            {
                int index = (int)key;
                index *= 4;
                fixedBuffer[index+0] = value.x;
                fixedBuffer[index+1] = value.y;
                fixedBuffer[index+2] = value.z;
                fixedBuffer[index+3] = value.w;
            }
        }
    }
}