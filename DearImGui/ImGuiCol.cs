namespace DearImGui
{
    public enum ImGuiCol : int
    {
        Text = 0,
        TextDisabled = 1,
        WindowBg = 2,              // Background of normal windows
        ChildBg = 3,               // Background of child windows
        PopupBg = 4,               // Background of popups, menus, tooltips windows
        Border = 5,
        BorderShadow = 6,
        FrameBg = 7,               // Background of checkbox, radio button, plot, slider, text input
        FrameBgHovered = 8,
        FrameBgActive = 9,
        TitleBg = 10,
        TitleBgActive = 11,
        TitleBgCollapsed = 12,
        MenuBarBg = 13,
        ScrollbarBg = 14,
        ScrollbarGrab = 15,
        ScrollbarGrabHovered = 16,
        ScrollbarGrabActive = 17,
        CheckMark = 18,
        SliderGrab = 19,
        SliderGrabActive = 20,
        Button = 21,
        ButtonHovered = 22,
        ButtonActive = 23,
        Header = 24,                // Header* colors are used for CollapsingHeader, TreeNode, Selectable, MenuItem
        HeaderHovered = 25,
        HeaderActive = 26,
        Separator = 27,
        SeparatorHovered = 28,
        SeparatorActive = 29,
        ResizeGrip = 30,
        ResizeGripHovered = 31,
        ResizeGripActive = 32,
        Tab = 33,
        TabHovered = 34,
        TabActive = 35,
        TabUnfocused = 36,
        TabUnfocusedActive = 37,
        PlotLines = 38,
        PlotLinesHovered = 39,
        PlotHistogram = 40,
        PlotHistogramHovered = 41,
        TableHeaderBg = 42,         // Table header background
        TableBorderStrong = 43,     // Table outer and header borders (prefer using Alpha=1.0 here)
        TableBorderLight = 44,      // Table inner borders (prefer using Alpha=1.0 here)
        TableRowBg = 45,            // Table row background (even rows)
        TableRowBgAlt = 46,         // Table row background (odd rows)
        TextSelectedBg = 47,
        DragDropTarget = 48,
        NavHighlight = 49,          // Gamepad/keyboard: current highlighted item
        NavWindowingHighlight = 50, // Highlight window when using CTRL+TAB
        NavWindowingDimBg = 51,     // Darken/colorize entire screen behind the CTRL+TAB window list, when active
        ModalWindowDimBg = 52,      // Darken/colorize entire screen behind a modal window, when one is active
        COUNT = 53
    }
}
