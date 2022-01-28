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

        public static void DrawTexture(Rect rect, int textureId)
        {
            BeginHideWindow(rect);
            ImGui.Image(textureId, new ImVec2(rect.width, rect.height), new ImVec2(0,0), new ImVec2(1,1), new ImVec4(1,1,1,1), new ImVec4(0,0,0,0));
            EndHideWindow();
        }

        public static void DrawTexture(Rect rect, int textureId, ImVec2 uv0, ImVec2 uv1)
        {
            BeginHideWindow(rect);
            ImGui.Image(textureId, new ImVec2(rect.width, rect.height), uv0, uv1, new ImVec4(1,1,1,1), new ImVec4(0,0,0,0));
            EndHideWindow();
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

        public static bool ColorEdit3(Rect rect, ref ImVec4 color)
        {
            bool result = false;
            BeginHideWindow(rect);
            result = ImGui.ColorEdit3("", ref color, ImGuiColorEditFlags.None);
            EndHideWindow();
            return result;
        }

        public static bool ColorEdit4(Rect rect, ref ImVec4 color)
        {
            bool result = false;
            BeginHideWindow(rect);
            result = ImGui.ColorEdit4("", ref color, ImGuiColorEditFlags.None);
            EndHideWindow();
            return result;
        }

        public static bool InputFloat(Rect rect, ref float value, float step, float step_fast, string format)
        {
            bool result = false;
            BeginHideWindow(rect);
            result = ImGui.InputFloat("", ref value, step, step_fast, format, ImGuiInputTextFlags.None);
            EndHideWindow();
            return result;
        }

        public static bool InputFloat2(Rect rect, ref ImVec2 value, int decimal_precision)
        {
            bool result = false;
            BeginHideWindow(rect);
            result = ImGui.InputFloat2("", ref value, decimal_precision, ImGuiInputTextFlags.None);
            EndHideWindow();
            return result;
        }

        public static bool InputFloat3(Rect rect, ref ImVec3 value, int decimal_precision)
        {
            bool result = false;
            BeginHideWindow(rect);
            result = ImGui.InputFloat3("", ref value, decimal_precision, ImGuiInputTextFlags.None);
            EndHideWindow();
            return result;
        }

        public static bool InputFloat4(Rect rect, ref ImVec4 value, int decimal_precision)
        {
            bool result = false;
            BeginHideWindow(rect);
            result = ImGui.InputFloat4("", ref value, decimal_precision, ImGuiInputTextFlags.None);
            EndHideWindow();
            return result;
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