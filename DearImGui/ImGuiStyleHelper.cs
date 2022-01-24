namespace DearImGui
{
    public static class ImGuiGuiStyleHelper
    {
        public static void SetCherryTheme()
        {
            ImGuiStyle style = ImGui.GetStyle();            
            style.Colors[ImGuiCol.Text]                  = new ImVec4(0.78f, 0.78f, 0.78f, 1.0f);
            style.Colors[ImGuiCol.TextDisabled]          = ImGuiStyleColor.Text(0.28f);
            style.Colors[ImGuiCol.WindowBg]              = new ImVec4(0.13f, 0.14f, 0.17f, 0.5f);
            style.Colors[ImGuiCol.ChildBg]               = new ImVec4(0.13f, 0.14f, 0.17f, 0.5f);
            style.Colors[ImGuiCol.PopupBg]               = ImGuiStyleColor.Background( 0.9f);
            style.Colors[ImGuiCol.Border]                = new ImVec4(0.31f, 0.31f, 1.00f, 0.00f);
            style.Colors[ImGuiCol.BorderShadow]          = new ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
            style.Colors[ImGuiCol.FrameBg]               = new ImVec4(0.26f, 0.26f, 0.26f, 0.5f);
            style.Colors[ImGuiCol.FrameBgHovered]        = ImGuiStyleColor.Medium( 0.78f);
            style.Colors[ImGuiCol.FrameBgActive]         = ImGuiStyleColor.Medium( 1.00f);
            style.Colors[ImGuiCol.TitleBg]               = ImGuiStyleColor.Low( 1.00f);
            style.Colors[ImGuiCol.TitleBgActive]         = ImGuiStyleColor.High( 1.00f);
            style.Colors[ImGuiCol.TitleBgCollapsed]      = ImGuiStyleColor.Background( 0.75f);
            style.Colors[ImGuiCol.MenuBarBg]             = ImGuiStyleColor.Background( 0.47f);
            style.Colors[ImGuiCol.ScrollbarBg]           = ImGuiStyleColor.Background( 1.00f);
            style.Colors[ImGuiCol.ScrollbarGrab]         = new ImVec4(0.09f, 0.15f, 0.16f, 1.00f);
            style.Colors[ImGuiCol.ScrollbarGrabHovered]  = ImGuiStyleColor.Medium( 0.78f);
            style.Colors[ImGuiCol.ScrollbarGrabActive]   = ImGuiStyleColor.Medium( 1.00f);
            style.Colors[ImGuiCol.CheckMark]             = new ImVec4(0.71f, 0.22f, 0.27f, 1.00f);
            style.Colors[ImGuiCol.SliderGrab]            = new ImVec4(0.47f, 0.77f, 0.83f, 0.14f);
            style.Colors[ImGuiCol.SliderGrabActive]      = new ImVec4(0.71f, 0.22f, 0.27f, 1.00f);
            style.Colors[ImGuiCol.Button]                = new ImVec4(0.47f, 0.77f, 0.83f, 0.14f);
            style.Colors[ImGuiCol.ButtonHovered]         = ImGuiStyleColor.Medium( 0.86f);
            style.Colors[ImGuiCol.ButtonActive]          = ImGuiStyleColor.Medium( 1.00f);
            style.Colors[ImGuiCol.Header]                = ImGuiStyleColor.Medium( 0.76f);
            style.Colors[ImGuiCol.HeaderHovered]         = ImGuiStyleColor.Medium( 0.86f);
            style.Colors[ImGuiCol.HeaderActive]          = ImGuiStyleColor.High( 1.00f);
            style.Colors[ImGuiCol.ResizeGrip]            = new ImVec4(0.47f, 0.77f, 0.83f, 0.04f);
            style.Colors[ImGuiCol.ResizeGripHovered]     = ImGuiStyleColor.Medium( 0.78f);
            style.Colors[ImGuiCol.ResizeGripActive]      = ImGuiStyleColor.Medium( 1.00f);
            style.Colors[ImGuiCol.PlotLines]             = ImGuiStyleColor.Text(0.63f);
            style.Colors[ImGuiCol.PlotLinesHovered]      = ImGuiStyleColor.Medium( 1.00f);
            style.Colors[ImGuiCol.PlotHistogram]         = ImGuiStyleColor.Text(0.63f);
            style.Colors[ImGuiCol.PlotHistogramHovered]  = ImGuiStyleColor.Medium( 1.00f);
            style.Colors[ImGuiCol.TextSelectedBg]        = ImGuiStyleColor.Medium( 0.43f);	
            style.Colors[ImGuiCol.ModalWindowDimBg]      = ImGuiStyleColor.Background( 0.73f);
            style.Colors[ImGuiCol.Border]                = new ImVec4(0.539f, 0.479f, 0.255f, 0.162f);
            style.WindowPadding = new ImVec2(1, 1);
            style.WindowRounding = 5.3f;
            style.FrameRounding = 2.3f;
            style.ScrollbarRounding = 0;
            style.FramePadding = new ImVec2(0, 0);
            style.ItemSpacing = new ImVec2(0, 0);
            style.ColumnsMinSpacing = 1;
            style.WindowTitleAlign.x = 0.50f;            
            style.FrameBorderSize = 0.0f;
            style.WindowBorderSize = 1.0f;            
            ImGui.SetStyle(ref style);
        }

        public static void SetStyle1()
        {
            ImGuiStyle style = ImGui.GetStyle();
            style.Colors[ImGuiCol.Text]                  = new ImVec4(0.90f, 0.90f, 0.90f, 0.90f);
            style.Colors[ImGuiCol.TextDisabled]          = new ImVec4(0.60f, 0.60f, 0.60f, 1.00f);
            style.Colors[ImGuiCol.WindowBg]              = new ImVec4(0.09f, 0.09f, 0.15f, 1.00f);
            style.Colors[ImGuiCol.ChildBg]               = new ImVec4(0.00f, 0.00f, 0.00f, 0.00f); 
            style.Colors[ImGuiCol.PopupBg]               = new ImVec4(0.05f, 0.05f, 0.10f, 0.85f);
            style.Colors[ImGuiCol.Border]                = new ImVec4(0.70f, 0.70f, 0.70f, 0.65f);
            style.Colors[ImGuiCol.BorderShadow]          = new ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
            style.Colors[ImGuiCol.FrameBg]               = new ImVec4(0.00f, 0.00f, 0.01f, 1.00f);
            style.Colors[ImGuiCol.FrameBgHovered]        = new ImVec4(0.90f, 0.80f, 0.80f, 0.40f);
            style.Colors[ImGuiCol.FrameBgActive]         = new ImVec4(0.90f, 0.65f, 0.65f, 0.45f);
            style.Colors[ImGuiCol.TitleBg]               = new ImVec4(0.00f, 0.00f, 0.00f, 0.83f);
            style.Colors[ImGuiCol.TitleBgCollapsed]      = new ImVec4(0.40f, 0.40f, 0.80f, 0.20f);
            style.Colors[ImGuiCol.TitleBgActive]         = new ImVec4(0.00f, 0.00f, 0.00f, 0.87f);
            style.Colors[ImGuiCol.MenuBarBg]             = new ImVec4(0.01f, 0.01f, 0.02f, 0.80f);
            style.Colors[ImGuiCol.ScrollbarBg]           = new ImVec4(0.20f, 0.25f, 0.30f, 0.60f);
            style.Colors[ImGuiCol.ScrollbarGrab]         = new ImVec4(0.55f, 0.53f, 0.55f, 0.51f);
            style.Colors[ImGuiCol.ScrollbarGrabHovered]  = new ImVec4(0.56f, 0.56f, 0.56f, 1.00f);
            style.Colors[ImGuiCol.ScrollbarGrabActive]   = new ImVec4(0.56f, 0.56f, 0.56f, 0.91f);
            style.Colors[ImGuiCol.CheckMark]             = new ImVec4(0.90f, 0.90f, 0.90f, 0.83f);
            style.Colors[ImGuiCol.SliderGrab]            = new ImVec4(0.70f, 0.70f, 0.70f, 0.62f);
            style.Colors[ImGuiCol.SliderGrabActive]      = new ImVec4(0.30f, 0.30f, 0.30f, 0.84f);
            style.Colors[ImGuiCol.Button]                = new ImVec4(0.48f, 0.72f, 0.89f, 0.49f);
            style.Colors[ImGuiCol.ButtonHovered]         = new ImVec4(0.50f, 0.69f, 0.99f, 0.68f);
            style.Colors[ImGuiCol.ButtonActive]          = new ImVec4(0.80f, 0.50f, 0.50f, 1.00f);
            style.Colors[ImGuiCol.Header]                = new ImVec4(0.30f, 0.69f, 1.00f, 0.53f);
            style.Colors[ImGuiCol.HeaderHovered]         = new ImVec4(0.44f, 0.61f, 0.86f, 1.00f);
            style.Colors[ImGuiCol.HeaderActive]          = new ImVec4(0.38f, 0.62f, 0.83f, 1.00f);
            style.Colors[ImGuiCol.ResizeGrip]            = new ImVec4(1.00f, 1.00f, 1.00f, 0.85f);
            style.Colors[ImGuiCol.ResizeGripHovered]     = new ImVec4(1.00f, 1.00f, 1.00f, 0.60f);
            style.Colors[ImGuiCol.ResizeGripActive]      = new ImVec4(1.00f, 1.00f, 1.00f, 0.90f);
            style.Colors[ImGuiCol.PlotLines]             = new ImVec4(1.00f, 1.00f, 1.00f, 1.00f);
            style.Colors[ImGuiCol.PlotLinesHovered]      = new ImVec4(0.90f, 0.70f, 0.00f, 1.00f);
            style.Colors[ImGuiCol.PlotHistogram]         = new ImVec4(0.90f, 0.70f, 0.00f, 1.00f);
            style.Colors[ImGuiCol.PlotHistogramHovered]  = new ImVec4(1.00f, 0.60f, 0.00f, 1.00f);
            style.Colors[ImGuiCol.TextSelectedBg]        = new ImVec4(0.00f, 0.00f, 1.00f, 0.35f);
            style.Colors[ImGuiCol.ModalWindowDimBg]      = new ImVec4(0.20f, 0.20f, 0.20f, 0.35f);
            style.WindowBorderSize = 0;
            style.WindowPadding = new ImVec2(0, 0);
            style.WindowRounding = 5.3f;
            style.FrameRounding = 2.3f;
            style.ScrollbarRounding = 0;
            style.FramePadding = new ImVec2(0, 0);
            style.ItemSpacing = new ImVec2(0, 0);
            style.ItemInnerSpacing = new ImVec2(0, 0);
            style.ColumnsMinSpacing = 0;
            ImGui.SetStyle(ref style);
        }

        public static void SetStyle2()
        {
            ImGuiStyle style = ImGui.GetStyle();
            style.Colors[ImGuiCol.Text]                 = new ImVec4(0.80f, 0.80f, 0.83f, 1.00f);
            style.Colors[ImGuiCol.TextDisabled]         = new ImVec4(0.24f, 0.23f, 0.29f, 1.00f);
            style.Colors[ImGuiCol.WindowBg]             = new ImVec4(0.06f, 0.05f, 0.07f, 1.00f);
            style.Colors[ImGuiCol.ChildBg]              = new ImVec4(0.07f, 0.07f, 0.09f, 1.00f); 
            style.Colors[ImGuiCol.PopupBg]              = new ImVec4(0.07f, 0.07f, 0.09f, 1.00f);
            style.Colors[ImGuiCol.Border]               = new ImVec4(0.80f, 0.80f, 0.83f, 0.88f);
            style.Colors[ImGuiCol.BorderShadow]         = new ImVec4(0.92f, 0.91f, 0.88f, 0.00f);
            style.Colors[ImGuiCol.FrameBg]              = new ImVec4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[ImGuiCol.FrameBgHovered]       = new ImVec4(0.24f, 0.23f, 0.29f, 1.00f);
            style.Colors[ImGuiCol.FrameBgActive]        = new ImVec4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[ImGuiCol.TitleBg]              = new ImVec4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[ImGuiCol.TitleBgCollapsed]     = new ImVec4(1.00f, 0.98f, 0.95f, 0.75f);
            style.Colors[ImGuiCol.TitleBgActive]        = new ImVec4(0.07f, 0.07f, 0.09f, 1.00f);
            style.Colors[ImGuiCol.MenuBarBg]            = new ImVec4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[ImGuiCol.ScrollbarBg]          = new ImVec4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[ImGuiCol.ScrollbarGrab]        = new ImVec4(0.80f, 0.80f, 0.83f, 0.31f);
            style.Colors[ImGuiCol.ScrollbarGrabHovered] = new ImVec4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[ImGuiCol.ScrollbarGrabActive]  = new ImVec4(0.06f, 0.05f, 0.07f, 1.00f);            
            style.Colors[ImGuiCol.CheckMark]            = new ImVec4(0.80f, 0.80f, 0.83f, 0.31f);
            style.Colors[ImGuiCol.SliderGrab]           = new ImVec4(0.80f, 0.80f, 0.83f, 0.31f);
            style.Colors[ImGuiCol.SliderGrabActive]     = new ImVec4(0.06f, 0.05f, 0.07f, 1.00f);
            style.Colors[ImGuiCol.Button]               = new ImVec4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[ImGuiCol.ButtonHovered]        = new ImVec4(0.24f, 0.23f, 0.29f, 1.00f);
            style.Colors[ImGuiCol.ButtonActive]         = new ImVec4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[ImGuiCol.Header]               = new ImVec4(0.10f, 0.09f, 0.12f, 1.00f);
            style.Colors[ImGuiCol.HeaderHovered]        = new ImVec4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[ImGuiCol.HeaderActive]         = new ImVec4(0.06f, 0.05f, 0.07f, 1.00f);
            style.Colors[ImGuiCol.ResizeGrip]           = new ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
            style.Colors[ImGuiCol.ResizeGripHovered]    = new ImVec4(0.56f, 0.56f, 0.58f, 1.00f);
            style.Colors[ImGuiCol.ResizeGripActive]     = new ImVec4(0.06f, 0.05f, 0.07f, 1.00f);
            style.Colors[ImGuiCol.PlotLines]            = new ImVec4(0.40f, 0.39f, 0.38f, 0.63f);
            style.Colors[ImGuiCol.PlotLinesHovered]     = new ImVec4(0.25f, 1.00f, 0.00f, 1.00f);
            style.Colors[ImGuiCol.PlotHistogram]        = new ImVec4(0.40f, 0.39f, 0.38f, 0.63f);
            style.Colors[ImGuiCol.PlotHistogramHovered] = new ImVec4(0.25f, 1.00f, 0.00f, 1.00f);
            style.Colors[ImGuiCol.TextSelectedBg]       = new ImVec4(0.25f, 1.00f, 0.00f, 0.43f);
            style.Colors[ImGuiCol.ModalWindowDimBg]     = new ImVec4(1.00f, 0.98f, 0.95f, 0.73f);
            ImGui.SetStyle(ref style);
        }

        public static void SetStyle3()
        {
            ImGuiStyle style = ImGui.GetStyle();
            
            /// 0 = FLAT APPEARENCE
            /// 1 = MORE "3D" LOOK
            int is3D = 1;
                
            style.Colors[ImGuiCol.Text]                   = new ImVec4(1.00f, 1.00f, 1.00f, 1.00f);
            style.Colors[ImGuiCol.TextDisabled]           = new ImVec4(0.40f, 0.40f, 0.40f, 1.00f);
            style.Colors[ImGuiCol.ChildBg]                = new ImVec4(0.25f, 0.25f, 0.25f, 1.00f);
            style.Colors[ImGuiCol.WindowBg]               = new ImVec4(0.25f, 0.25f, 0.25f, 1.00f);
            style.Colors[ImGuiCol.PopupBg]                = new ImVec4(0.25f, 0.25f, 0.25f, 1.00f);
            style.Colors[ImGuiCol.Border]                 = new ImVec4(0.12f, 0.12f, 0.12f, 0.71f);
            style.Colors[ImGuiCol.BorderShadow]           = new ImVec4(1.00f, 1.00f, 1.00f, 0.06f);
            style.Colors[ImGuiCol.FrameBg]                = new ImVec4(0.42f, 0.42f, 0.42f, 0.54f);
            style.Colors[ImGuiCol.FrameBgHovered]         = new ImVec4(0.42f, 0.42f, 0.42f, 0.40f);
            style.Colors[ImGuiCol.FrameBgActive]          = new ImVec4(0.56f, 0.56f, 0.56f, 0.67f);
            style.Colors[ImGuiCol.TitleBg]                = new ImVec4(0.19f, 0.19f, 0.19f, 1.00f);
            style.Colors[ImGuiCol.TitleBgActive]          = new ImVec4(0.22f, 0.22f, 0.22f, 1.00f);
            style.Colors[ImGuiCol.TitleBgCollapsed]       = new ImVec4(0.17f, 0.17f, 0.17f, 0.90f);
            style.Colors[ImGuiCol.MenuBarBg]              = new ImVec4(0.335f, 0.335f, 0.335f, 1.000f);
            style.Colors[ImGuiCol.ScrollbarBg]            = new ImVec4(0.24f, 0.24f, 0.24f, 0.53f);
            style.Colors[ImGuiCol.ScrollbarGrab]          = new ImVec4(0.41f, 0.41f, 0.41f, 1.00f);
            style.Colors[ImGuiCol.ScrollbarGrabHovered]   = new ImVec4(0.52f, 0.52f, 0.52f, 1.00f);
            style.Colors[ImGuiCol.ScrollbarGrabActive]    = new ImVec4(0.76f, 0.76f, 0.76f, 1.00f);
            style.Colors[ImGuiCol.CheckMark]              = new ImVec4(0.65f, 0.65f, 0.65f, 1.00f);
            style.Colors[ImGuiCol.SliderGrab]             = new ImVec4(0.52f, 0.52f, 0.52f, 1.00f);
            style.Colors[ImGuiCol.SliderGrabActive]       = new ImVec4(0.64f, 0.64f, 0.64f, 1.00f);
            style.Colors[ImGuiCol.Button]                 = new ImVec4(0.54f, 0.54f, 0.54f, 0.35f);
            style.Colors[ImGuiCol.ButtonHovered]          = new ImVec4(0.52f, 0.52f, 0.52f, 0.59f);
            style.Colors[ImGuiCol.ButtonActive]           = new ImVec4(0.76f, 0.76f, 0.76f, 1.00f);
            style.Colors[ImGuiCol.Header]                 = new ImVec4(0.38f, 0.38f, 0.38f, 1.00f);
            style.Colors[ImGuiCol.HeaderHovered]          = new ImVec4(0.47f, 0.47f, 0.47f, 1.00f);
            style.Colors[ImGuiCol.HeaderActive]           = new ImVec4(0.76f, 0.76f, 0.76f, 0.77f);
            style.Colors[ImGuiCol.Separator]              = new ImVec4(0.000f, 0.000f, 0.000f, 0.137f);
            style.Colors[ImGuiCol.SeparatorHovered]       = new ImVec4(0.700f, 0.671f, 0.600f, 0.290f);
            style.Colors[ImGuiCol.SeparatorActive]        = new ImVec4(0.702f, 0.671f, 0.600f, 0.674f);
            style.Colors[ImGuiCol.ResizeGrip]             = new ImVec4(0.26f, 0.59f, 0.98f, 0.25f);
            style.Colors[ImGuiCol.ResizeGripHovered]      = new ImVec4(0.26f, 0.59f, 0.98f, 0.67f);
            style.Colors[ImGuiCol.ResizeGripActive]       = new ImVec4(0.26f, 0.59f, 0.98f, 0.95f);
            style.Colors[ImGuiCol.PlotLines]              = new ImVec4(0.61f, 0.61f, 0.61f, 1.00f);
            style.Colors[ImGuiCol.PlotLinesHovered]       = new ImVec4(1.00f, 0.43f, 0.35f, 1.00f);
            style.Colors[ImGuiCol.PlotHistogram]          = new ImVec4(0.10f, 0.70f, 0.00f, 1.00f);
            style.Colors[ImGuiCol.PlotHistogramHovered]   = new ImVec4(1.00f, 0.60f, 0.00f, 1.00f);
            style.Colors[ImGuiCol.TextSelectedBg]         = new ImVec4(0.73f, 0.73f, 0.73f, 0.35f);
            style.Colors[ImGuiCol.ModalWindowDimBg]       = new ImVec4(0.80f, 0.80f, 0.80f, 0.35f);
            style.Colors[ImGuiCol.DragDropTarget]         = new ImVec4(1.00f, 1.00f, 0.00f, 0.90f);
            style.Colors[ImGuiCol.NavHighlight]           = new ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
            style.Colors[ImGuiCol.NavWindowingHighlight]  = new ImVec4(1.00f, 1.00f, 1.00f, 0.70f);
            style.Colors[ImGuiCol.NavWindowingDimBg]      = new ImVec4(0.80f, 0.80f, 0.80f, 0.20f);
            style.PopupRounding = 3;
            style.WindowPadding = new ImVec2(0, 0);
            style.FramePadding  = new ImVec2(6, 4);
            style.ItemSpacing   = new ImVec2(6, 2);
            style.ScrollbarSize = 18;
            style.WindowBorderSize = 0;
            style.ChildBorderSize  = 1;
            style.PopupBorderSize  = 1;
            style.FrameBorderSize  = is3D;
            style.WindowRounding    = 3;
            style.ChildRounding     = 3;
            style.FrameRounding     = 3;
            style.ScrollbarRounding = 2;
            style.GrabRounding      = 3;
            ImGui.SetStyle(ref style);
        }

        public static void SetStyle4()
        {
            ImGuiStyle style = ImGui.GetStyle();
            style.Colors[ImGuiCol.Text]                  = new ImVec4(0.90f, 0.90f, 0.90f, 0.90f);
            style.Colors[ImGuiCol.TextDisabled]          = new ImVec4(0.60f, 0.60f, 0.60f, 1.00f);
            style.Colors[ImGuiCol.WindowBg]              = new ImVec4(0.09f, 0.09f, 0.15f, 1.00f);
            style.Colors[ImGuiCol.ChildBg]               = new ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
            style.Colors[ImGuiCol.PopupBg]               = new ImVec4(0.05f, 0.05f, 0.10f, 0.85f);
            style.Colors[ImGuiCol.Border]                = new ImVec4(0.70f, 0.70f, 0.70f, 0.65f);
            style.Colors[ImGuiCol.BorderShadow]          = new ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
            style.Colors[ImGuiCol.FrameBg]               = new ImVec4(0.00f, 0.00f, 0.01f, 1.00f);
            style.Colors[ImGuiCol.FrameBgHovered]        = new ImVec4(0.90f, 0.80f, 0.80f, 0.40f);
            style.Colors[ImGuiCol.FrameBgActive]         = new ImVec4(0.90f, 0.65f, 0.65f, 0.45f);
            style.Colors[ImGuiCol.TitleBg]               = new ImVec4(0.00f, 0.00f, 0.00f, 0.83f);
            style.Colors[ImGuiCol.TitleBgCollapsed]      = new ImVec4(0.40f, 0.40f, 0.80f, 0.20f);
            style.Colors[ImGuiCol.TitleBgActive]         = new ImVec4(0.00f, 0.00f, 0.00f, 0.87f);
            style.Colors[ImGuiCol.MenuBarBg]             = new ImVec4(0.01f, 0.01f, 0.02f, 0.80f);
            style.Colors[ImGuiCol.ScrollbarBg]           = new ImVec4(0.20f, 0.25f, 0.30f, 0.60f);
            style.Colors[ImGuiCol.ScrollbarGrab]         = new ImVec4(0.55f, 0.53f, 0.55f, 0.51f);
            style.Colors[ImGuiCol.ScrollbarGrabHovered]  = new ImVec4(0.56f, 0.56f, 0.56f, 1.00f);
            style.Colors[ImGuiCol.ScrollbarGrabActive]   = new ImVec4(0.56f, 0.56f, 0.56f, 0.91f);
            style.Colors[ImGuiCol.CheckMark]             = new ImVec4(0.90f, 0.90f, 0.90f, 0.83f);
            style.Colors[ImGuiCol.SliderGrab]            = new ImVec4(0.70f, 0.70f, 0.70f, 0.62f);
            style.Colors[ImGuiCol.SliderGrabActive]      = new ImVec4(0.30f, 0.30f, 0.30f, 0.84f);
            style.Colors[ImGuiCol.Button]                = new ImVec4(0.48f, 0.72f, 0.89f, 0.49f);
            style.Colors[ImGuiCol.ButtonHovered]         = new ImVec4(0.50f, 0.69f, 0.99f, 0.68f);
            style.Colors[ImGuiCol.ButtonActive]          = new ImVec4(0.80f, 0.50f, 0.50f, 1.00f);
            style.Colors[ImGuiCol.Header]                = new ImVec4(0.30f, 0.69f, 1.00f, 0.53f);
            style.Colors[ImGuiCol.HeaderHovered]         = new ImVec4(0.44f, 0.61f, 0.86f, 1.00f);
            style.Colors[ImGuiCol.HeaderActive]          = new ImVec4(0.38f, 0.62f, 0.83f, 1.00f);
            style.Colors[ImGuiCol.ResizeGrip]            = new ImVec4(1.00f, 1.00f, 1.00f, 0.85f);
            style.Colors[ImGuiCol.ResizeGripHovered]     = new ImVec4(1.00f, 1.00f, 1.00f, 0.60f);
            style.Colors[ImGuiCol.ResizeGripActive]      = new ImVec4(1.00f, 1.00f, 1.00f, 0.90f);
            style.Colors[ImGuiCol.PlotLines]             = new ImVec4(1.00f, 1.00f, 1.00f, 1.00f);
            style.Colors[ImGuiCol.PlotLinesHovered]      = new ImVec4(0.90f, 0.70f, 0.00f, 1.00f);
            style.Colors[ImGuiCol.PlotHistogram]         = new ImVec4(0.90f, 0.70f, 0.00f, 1.00f);
            style.Colors[ImGuiCol.PlotHistogramHovered]  = new ImVec4(1.00f, 0.60f, 0.00f, 1.00f);
            style.Colors[ImGuiCol.TextSelectedBg]        = new ImVec4(0.00f, 0.00f, 1.00f, 0.35f);
            style.WindowRounding = 5.3f;
            style.FrameRounding = 2.3f;
            style.ScrollbarRounding = 0;
            ImGui.SetStyle(ref style);
        }

        public static void SetStyle5()
        {
            ImGuiStyle style = ImGui.GetStyle();
            style.Colors[ImGuiCol.Text]                  = new ImVec4(0.00f, 0.00f, 0.00f, 1.00f);
            style.Colors[ImGuiCol.TextDisabled]          = new ImVec4(0.60f, 0.60f, 0.60f, 1.00f);
            style.Colors[ImGuiCol.WindowBg]              = new ImVec4(0.86f, 0.86f, 0.86f, 1.00f);
            style.Colors[ImGuiCol.ChildBg]               = new ImVec4(0.00f, 0.00f, 0.00f, 0.00f);
            style.Colors[ImGuiCol.PopupBg]               = new ImVec4(0.93f, 0.93f, 0.93f, 0.98f);
            style.Colors[ImGuiCol.Border]                = new ImVec4(0.71f, 0.71f, 0.71f, 0.08f);
            style.Colors[ImGuiCol.BorderShadow]          = new ImVec4(0.00f, 0.00f, 0.00f, 0.04f);
            style.Colors[ImGuiCol.FrameBg]               = new ImVec4(0.71f, 0.71f, 0.71f, 0.55f);
            style.Colors[ImGuiCol.FrameBgHovered]        = new ImVec4(0.94f, 0.94f, 0.94f, 0.55f);
            style.Colors[ImGuiCol.FrameBgActive]         = new ImVec4(0.71f, 0.78f, 0.69f, 0.98f);
            style.Colors[ImGuiCol.TitleBg]               = new ImVec4(0.85f, 0.85f, 0.85f, 1.00f);
            style.Colors[ImGuiCol.TitleBgCollapsed]      = new ImVec4(0.82f, 0.78f, 0.78f, 0.51f);
            style.Colors[ImGuiCol.TitleBgActive]         = new ImVec4(0.78f, 0.78f, 0.78f, 1.00f);
            style.Colors[ImGuiCol.MenuBarBg]             = new ImVec4(0.86f, 0.86f, 0.86f, 1.00f);
            style.Colors[ImGuiCol.ScrollbarBg]           = new ImVec4(0.20f, 0.25f, 0.30f, 0.61f);
            style.Colors[ImGuiCol.ScrollbarGrab]         = new ImVec4(0.90f, 0.90f, 0.90f, 0.30f);
            style.Colors[ImGuiCol.ScrollbarGrabHovered]  = new ImVec4(0.92f, 0.92f, 0.92f, 0.78f);
            style.Colors[ImGuiCol.ScrollbarGrabActive]   = new ImVec4(1.00f, 1.00f, 1.00f, 1.00f);
            style.Colors[ImGuiCol.CheckMark]             = new ImVec4(0.184f, 0.407f, 0.193f, 1.00f);
            style.Colors[ImGuiCol.SliderGrab]            = new ImVec4(0.26f, 0.59f, 0.98f, 0.78f);
            style.Colors[ImGuiCol.SliderGrabActive]      = new ImVec4(0.26f, 0.59f, 0.98f, 1.00f);
            style.Colors[ImGuiCol.Button]                = new ImVec4(0.71f, 0.78f, 0.69f, 0.40f);
            style.Colors[ImGuiCol.ButtonHovered]         = new ImVec4(0.725f, 0.805f, 0.702f, 1.00f);
            style.Colors[ImGuiCol.ButtonActive]          = new ImVec4(0.793f, 0.900f, 0.836f, 1.00f);
            style.Colors[ImGuiCol.Header]                = new ImVec4(0.71f, 0.78f, 0.69f, 0.31f);
            style.Colors[ImGuiCol.HeaderHovered]         = new ImVec4(0.71f, 0.78f, 0.69f, 0.80f);
            style.Colors[ImGuiCol.HeaderActive]          = new ImVec4(0.71f, 0.78f, 0.69f, 1.00f);
            style.Colors[ImGuiCol.Separator]             = new ImVec4(0.39f, 0.39f, 0.39f, 1.00f);
            style.Colors[ImGuiCol.SeparatorHovered]      = new ImVec4(0.14f, 0.44f, 0.80f, 0.78f);
            style.Colors[ImGuiCol.SeparatorActive]       = new ImVec4(0.14f, 0.44f, 0.80f, 1.00f);
            style.Colors[ImGuiCol.ResizeGrip]            = new ImVec4(1.00f, 1.00f, 1.00f, 0.00f);
            style.Colors[ImGuiCol.ResizeGripHovered]     = new ImVec4(0.26f, 0.59f, 0.98f, 0.45f);
            style.Colors[ImGuiCol.ResizeGripActive]      = new ImVec4(0.26f, 0.59f, 0.98f, 0.78f);
            style.Colors[ImGuiCol.PlotLines]             = new ImVec4(0.39f, 0.39f, 0.39f, 1.00f);
            style.Colors[ImGuiCol.PlotLinesHovered]      = new ImVec4(1.00f, 0.43f, 0.35f, 1.00f);
            style.Colors[ImGuiCol.PlotHistogram]         = new ImVec4(0.90f, 0.70f, 0.00f, 1.00f);
            style.Colors[ImGuiCol.PlotHistogramHovered]  = new ImVec4(1.00f, 0.60f, 0.00f, 1.00f);
            style.Colors[ImGuiCol.TextSelectedBg]        = new ImVec4(0.26f, 0.59f, 0.98f, 0.35f);
            style.Colors[ImGuiCol.DragDropTarget]        = new ImVec4(0.26f, 0.59f, 0.98f, 0.95f);
            style.Colors[ImGuiCol.NavHighlight]          = style.Colors[ImGuiCol.HeaderHovered];
            style.Colors[ImGuiCol.NavWindowingHighlight] = new ImVec4(0.70f, 0.70f, 0.70f, 0.70f);
            style.WindowRounding    = 2.0f;             // Radius of window corners rounding. Set to 0.0f to have rectangular windows
            style.ScrollbarRounding = 3.0f;             // Radius of grab corners rounding for scrollbar
            style.GrabRounding      = 2.0f;             // Radius of grabs corners rounding. Set to 0.0f to have rectangular slider grabs.
            style.AntiAliasedLines  = true;
            style.AntiAliasedFill   = true;
            style.WindowRounding    = 2;
            style.ChildRounding     = 2;
            style.ScrollbarSize     = 16;
            style.ScrollbarRounding = 3;
            style.GrabRounding      = 2;
            style.ItemSpacing.x     = 10;
            style.ItemSpacing.y     = 4;
            style.IndentSpacing     = 22;
            style.FramePadding.x    = 6;
            style.FramePadding.y    = 4;
            style.Alpha             = 1.0f;
            style.FrameRounding     = 3.0f;
            ImGui.SetStyle(ref style);
        }
    }

    public static class ImGuiStyleColor
    {
        public static ImVec4 High(float alpha)
        {
            return new ImVec4(0.502f, 0.075f, 0.256f, alpha);
        }

        public static ImVec4 Medium(float alpha)
        {
            return new ImVec4(0.455f, 0.198f, 0.301f, alpha);
        }

        public static ImVec4 Low(float alpha)
        {
            return new ImVec4(0.232f, 0.201f, 0.271f, alpha);
        }

        public static ImVec4 Background(float alpha)
        {
            return new ImVec4(0.200f, 0.220f, 0.270f, alpha);
        }

        public static ImVec4 Text(float alpha)
        {
            return new ImVec4(0.860f, 0.930f, 0.890f, alpha);
        }
    }
}