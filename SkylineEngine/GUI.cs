using DearImGui;

namespace SkylineEngine
{
    public static class GUI
    {
        private static uint idCounter = 0;

        private static void BeginHideWindow(Rect rect)
        {
            ImGuiWindowFlags flags = 0;
            flags |= ImGuiWindowFlags.NoTitleBar;
            flags |= ImGuiWindowFlags.NoResize;
            flags |= ImGuiWindowFlags.NoMove;
            flags |= ImGuiWindowFlags.NoScrollbar;	
            flags |= ImGuiWindowFlags.NoBackground;	

            bool active = true;
            uint id = GetId();
            ImGui.SetNextWindowSize(rect.width, rect.height);
            ImGui.SetNextWindowPos(rect.x, rect.y);
            ImGui.Begin(id.ToString(), ref active, flags);
        }

        private static void EndHideWindow()
        {
            ImGui.End();
        }

        public static void Label(Rect rect, string text)
        {
            BeginHideWindow(rect);
            ImGui.Text(text);
            EndHideWindow();
        }

        public static bool Button(Rect rect, string text)
        {
            BeginHideWindow(rect);
            bool result = ImGui.Button(text, new ImVec2(rect.width, rect.height));
            EndHideWindow();
            return result;
        }

        public static string TextField(Rect rect, string text)
        {
            BeginHideWindow(rect);
            ImGui.InputText("", ref text, 4096, ImGuiInputTextFlags.None);
            EndHideWindow();
            return text;
        }

        public static void DrawTexture(Rect rect, Texture texture)
        {
            BeginHideWindow(rect);
            ImGui.Image((int)texture.textureId, new ImVec2(rect.width, rect.height), new ImVec2(0,0), new ImVec2(1,1), new ImVec4(1,1,1,1), new ImVec4(0,0,0,0));
            EndHideWindow();
        }

        public static void DrawTexture(Rect rect, Texture texture, ImVec4 color)
        {
            BeginHideWindow(rect);
            ImGui.Image((int)texture.textureId, new ImVec2(rect.width, rect.height), new ImVec2(0,0), new ImVec2(1,1), color, new ImVec4(0,0,0,0));
            EndHideWindow();
        }

        private static uint GetId()
        {
            idCounter++;
            return idCounter;
        }

        internal static void ClearIdQueue()
        {
            idCounter = 0;
        }
    }
}