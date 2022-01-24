using System;

namespace DearImGui
{    
    public delegate bool ProcessCommandCallback(string command);

    public class ImGuiConsole
    {
        private IntPtr handle;
        private string title;

        public event ProcessCommandCallback processCommand;

        private ProcessCommandCallback onProcessCommand;

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        public ImGuiConsole(string title)
        {
            this.title = title;
            handle = ImGui.ImGuiConsoleAppCreate();
            onProcessCommand = OnProcessCommand;
            ImGui.ImGuiConsoleAppSetCommandCallback(handle, onProcessCommand);
        }

        public void Dispose()
        {
            if(handle == IntPtr.Zero)
                return;
            
            ImGui.ImGuiConsoleAppDispose(handle);
            ImGui.ImGuiConsoleAppFree(handle);
        }

        public void Draw(ImVec2 position, ImVec2 size)
        {
            if(handle == IntPtr.Zero)
                return;

            ImGui.ImGuiConsoleAppDraw(handle, title, ref position, ref size);
        }

        public void AddLog(string message)
        {
            if(handle == IntPtr.Zero)
                return;
            
            ImGui.ImGuiConsoleAppAddLog(handle, message);
        }

        public void ClearLog()
        {
            if(handle == IntPtr.Zero)
                return;

            ImGui.ImGuiConsoleAppClearLog(handle);
        }

        public void SetFocus()
        {
            if(handle == IntPtr.Zero)
                return;

            ImGui.ImGuiConsoleAppSetFocus(handle);
        }

        private bool OnProcessCommand(string command)
        {
            if(processCommand != null)
            {
                return processCommand(command);
            }

            return false;
        }
    }
}
