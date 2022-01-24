using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DearImGui
{
    public static class ImGui
    {
        //const string NATIVELIBNAME = "glsharp";
        const string NATIVELIBNAME = "imgui";

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiBegin(string id, ref bool active, ImGuiWindowFlags flags);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiEnd();

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiSetNextWindowPos(float x, float y);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiSetNextWindowSize(float x, float y);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiSetCursorPos(ref ImVec2 pos);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiSetCursorPosX(float x);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiPushStyleColor(ImGuiCol idx, ref ImVec4 col);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiPopStyleColor(int count = 1);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiIsAnyItemActive();

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiIsAnyItemFocused();
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiIsAnyItemHovered();

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiSetKeyboardFocusHere(int offset);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiCheckbox(string text, ref bool value);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiText(string text);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiButton(string text, ref ImVec2 rect);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiImageButton(int textureId, ref ImVec2 size, ref ImVec2 uv0, ref ImVec2 uv1, int frame_padding, ref ImVec4 bg_col, ref ImVec4 tint_col);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiInputFloat(string label, ref float value, float step, float step_fast, string format, ImGuiInputTextFlags flags);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiSliderFloat(string label, ref float value, float v_min, float v_max, string format, float power);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe bool ImGuiInputFloat3(string label, float* value, int decimal_precision, ImGuiInputTextFlags flags);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe bool ImGuiColorEdit3(string label, float* color, ImGuiColorEditFlags flags);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe bool ImGuiColorEdit4(string label, float* color, ImGuiColorEditFlags flags);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiImage(int textureId, ref ImVec2 size, ref ImVec2 uv0, ref ImVec2 uv1, ref ImVec4 tint_col, ref ImVec4 border_col);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe bool ImGuiInputText(string label, StringBuilder str, int bufferSize, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe bool ImGuiInputTextMultiline(string label, char* str, int bufferSize, ref ImVec2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void ImGuiPlotLines(string label, float* values, int values_count, int values_offset);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void ImGuiProgressBar(float progress, ref ImVec2 size_arg, char* overlay);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiProgressBarWithoutLabel(float progress, ref ImVec2 size_arg);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiSetWindowFontScale(float scale);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiBeginPopupContextItem(string id, ImGuiPopupFlags flags);    
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiEndPopup();
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float ImGuiGetFrameHeightWithSpacing();
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiBeginChild1(string id, ImVec2 size, bool border, ImGuiWindowFlags flags);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiBeginChild2(uint id, ImVec2 size, bool border, ImGuiWindowFlags flags);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiBeginPopupContextWindow();
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiBeginPopupContextWindow1(string id, ImGuiPopupFlags flags);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiBeginPopupContextWindow2(string id, ImGuiMouseButton mb, bool overItems);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiSelectable(string label);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiSelectable1(string label, ref bool selected, ImGuiSelectableFlags flags, ImVec2 size);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiSelectable2(string label, bool selected, ImGuiSelectableFlags flags, ImVec2 size);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiPushStyleVar1(ImGuiStyleVar styleVar, ImVec2 val);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiPushStyleVar2(ImGuiStyleVar styleVar, float val);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiPopStyleVar(int count);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiLogToClipboard(int autoOpenDepth);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float ImGuiGetScrollY();
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float ImGuiGetScrollMaxY();
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiSetScrollHereY(float centerYRatio);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiIsWindowFocused(ImGuiFocusedFlags flags);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiIsMouseClicked(ImGuiMouseButton mb, bool repeat);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiSetItemDefaultFocus();

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiSetStyle(ref ImGuiStyle style);
    
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiGetStyle(out ImGuiStyle style);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiSetIO(ref ImGuiIO io);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiGetIO(out ImGuiIO io);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiMenuItem(string label);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiMenuItem1(string label, string shortcut, ref bool selected, bool enabled);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool ImGuiMenuItem2(string label, string shortcut, bool selected, bool enabled);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiLogFinish();

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiTextUnformatted(string text);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiTextUnformatted1(string text, string textEnd);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiEndChild();

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr ImGuiAddFontFromFileTTF(string filepath, float sizePixels);

        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr ImGuiConsoleAppCreate();
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiConsoleAppDispose(IntPtr app);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiConsoleAppFree(IntPtr app);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiConsoleAppSetCommandCallback(IntPtr app, ProcessCommandCallback callback);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiConsoleAppDraw(IntPtr app, string title, ref ImVec2 position, ref ImVec2 size);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiConsoleAppAddLog(IntPtr app, string message);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiConsoleAppClearLog(IntPtr app);
        
        [DllImport(NATIVELIBNAME, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void ImGuiConsoleAppSetFocus(IntPtr app);

        public static bool Begin(string id, ref bool active, ImGuiWindowFlags flags)
        {
            return ImGuiBegin(id, ref active, flags);
        }

        public static void End()
        {
            ImGuiEnd();
        }

        public static void SetNextWindowPos(float x, float y)
        {
            ImGuiSetNextWindowPos(x, y);
        }

        public static void SetNextWindowSize(float x, float y)
        {
            ImGuiSetNextWindowSize(x, y);
        }

        public static void SetCursorPos(ImVec2 pos)
        {
            ImGuiSetCursorPos(ref pos);
        }        
        
        public static void SetCursorPosX(float x)
        {
            ImGuiSetCursorPosX(x);
        }

        public static void PushStyleColor(ImGuiCol idx, ImVec4 col)
        {
            ImGuiPushStyleColor(idx, ref col);
        }

        public static void PopStyleColor(int count = 1)
        {
            ImGuiPopStyleColor(count);
        }

        public static bool IsAnyItemActive()
        {
            return ImGuiIsAnyItemActive();
        }

        public static bool IsAnyItemFocused()
        {
            return ImGuiIsAnyItemFocused();
        }

        public static bool IsAnyItemHovered()
        {
            return ImGuiIsAnyItemHovered();
        }

        public static void SetKeyboardFocusHere(int offset)
        {
            ImGuiSetKeyboardFocusHere(offset);
        }

        public static bool Checkbox(string text, ref bool value)
        {
            return ImGuiCheckbox(text, ref value);
        }

        public static void Text(string text)
        {
            ImGuiText(text);
        }

        public static bool Button(string text, ImVec2 rect)
        {
            return ImGuiButton(text, ref rect);
        }

        public static bool ImageButton(int textureId, ImVec2 size, ImVec2 uv0, ImVec2 uv1, int frame_padding, ImVec4 bg_col, ImVec4 tint_col)
        {
            return ImGuiImageButton(textureId, ref size, ref uv0, ref uv1, frame_padding, ref bg_col, ref tint_col);
        }

        public static bool InputFloat(string label, ref float value, float step, float step_fast, string format, ImGuiInputTextFlags flags)
        {
            return ImGuiInputFloat(label, ref value, step, step_fast, format, flags);
        }

        public static bool SliderFloat(string label, ref float value, float v_min, float v_max, string format, float power)
        {
            return ImGuiSliderFloat(label, ref value, v_min, v_max, format, power);
        }

        public static unsafe bool InputFloat3(string label, float[] values, int decimal_precision, ImGuiInputTextFlags flags)
        {
            fixed(float* ptr = &values[0])
            {
                return ImGuiInputFloat3(label, ptr, decimal_precision, flags);
            }
        }

        public static unsafe bool ColorEdit3(string label, ref ImVec4 color, ImGuiColorEditFlags flags)
        {
            fixed(float* ptr = &color.x)
            {
                return ImGuiColorEdit3(label, ptr, flags);
            }
        }

        public static unsafe bool ColorEdit4(string label, ref ImVec4 color, ImGuiColorEditFlags flags)
        {
            fixed(float* ptr = &color.x)
            {
                return ImGuiColorEdit4(label, ptr, flags);
            }
        }

        public static void Image(int textureId, ImVec2 size, ImVec2 uv0, ImVec2 uv1, ImVec4 tint_col, ImVec4 border_col)
        {
            ImGuiImage(textureId, ref size, ref uv0, ref uv1, ref tint_col, ref border_col);
        }

        public static unsafe bool InputText(string label, ref string str, int bufferSize, ImGuiInputTextFlags flags)
        {
            StringBuilder sb = new StringBuilder(bufferSize);
            sb.Append(str);

            bool ret = ImGuiInputText(label, sb, bufferSize, ImGuiInputTextFlags.EnterReturnsTrue, null, null);
            str = sb.ToString();
            
            return ret;
        }

        public static unsafe bool InputTextMultiline(string label, string str, int bufferSize, ImVec2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data)
        {
            fixed(char* ptr = str)
            {
                return ImGuiInputTextMultiline(label, ptr, bufferSize, ref size, flags, callback, user_data);
            }
        }

        public static unsafe void PlotLines(string label, float[] values, int values_count, int values_offset)
        {
            fixed(float* ptr = &values[0])
            {
                ImGuiPlotLines(label, ptr, values_count, values_offset);
            }
        }

        public static unsafe void ProgressBar(float fraction)
        {
            ImVec2 size = new ImVec2(-1, 0);
            ImGuiProgressBar(fraction, ref size, null);
        }

        public static void ProgressBarWithoutLabel(float fraction)
        {
            ImVec2 size = new ImVec2(-1, 0);
            ImGuiProgressBarWithoutLabel(fraction, ref size);
        }        

        public static void SetWindowFontScale(float scale)
        {
            ImGuiSetWindowFontScale(scale);
        }

        public static bool BeginPopupContextItem(string id, ImGuiPopupFlags flags)
        {
            return ImGuiBeginPopupContextItem(id, flags);
        }

        public static void EndPopup()
        {
            ImGuiEndPopup();
        }

        public static float GetFrameHeightWithSpacing()
        {
            return ImGuiGetFrameHeightWithSpacing();
        }

        public static bool BeginChild(string id, ImVec2 size = new ImVec2(), bool border = false, ImGuiWindowFlags flags = ImGuiWindowFlags.None)
        {
            return ImGuiBeginChild1(id, size, border, flags);
        }

        public static bool BeginChild(uint id, ImVec2 size = new ImVec2(), bool border = false, ImGuiWindowFlags flags = ImGuiWindowFlags.None)
        {
            return ImGuiBeginChild2(id, size, border, flags);
        }

        public static bool BeginPopupContextWindow()
        {
            return ImGuiBeginPopupContextWindow();
        }

        public static bool BeginPopupContextWindow(string id, ImGuiPopupFlags flags = ImGuiPopupFlags.MouseButtonRight)
        {
            return ImGuiBeginPopupContextWindow1(id, flags);
        }

        public static bool BeginPopupContextWindow(string id, ImGuiMouseButton mb, bool overItems)
        {
            return ImGuiBeginPopupContextWindow2(id, mb, overItems);
        }

        public static bool Selectable(string label)
        {
            return ImGuiSelectable(label);
        }

        public static bool Selectable(string label, ref bool selected, ImGuiSelectableFlags flags = ImGuiSelectableFlags.None, ImVec2 size = new ImVec2())
        {
            return ImGuiSelectable1(label, ref selected, flags, size);
        }

        public static bool Selectable(string label, bool selected, ImGuiSelectableFlags flags = ImGuiSelectableFlags.None, ImVec2 size = new ImVec2())
        {
            return ImGuiSelectable2(label, selected, flags, size);
        }

        public static void PushStyleVar(ImGuiStyleVar styleVar, ImVec2 val)
        {
            ImGuiPushStyleVar1(styleVar, val);
        }

        public static void PushStyleVar(ImGuiStyleVar styleVar, float val)
        {
            ImGuiPushStyleVar2(styleVar, val);
        }

        public static void PopStyleVar(int count = 1)
        {
            ImGuiPopStyleVar(count);
        }

        public static void LogToClipboard(int autoOpenDepth = -1)
        {
            ImGuiLogToClipboard(autoOpenDepth);
        }

        public static float GetScrollY()
        {
            return ImGuiGetScrollY();
        }

        public static float GetScrollMaxY()
        {
            return ImGuiGetScrollMaxY();
        }

        public static void SetScrollHereY(float centerYRatio = 0.5f)
        {
            ImGuiSetScrollHereY(centerYRatio);
        }

        public static bool IsWindowFocused(ImGuiFocusedFlags flags = ImGuiFocusedFlags.None)
        {
            return ImGuiIsWindowFocused(flags);
        }

        public static bool IsMouseClicked(ImGuiMouseButton mb, bool repeat = false)
        {
            return ImGuiIsMouseClicked(mb, repeat);
        }

        public static void SetItemDefaultFocus()
        {
            ImGuiSetItemDefaultFocus();
        }

        public static bool MenuItem(string label)
        {
            return ImGuiMenuItem(label);
        }

        public static bool MenuItem(string label, string shortcut, ref bool selected, bool enabled)
        {
            return ImGuiMenuItem1(label, shortcut, ref selected, enabled);
        }

        public static bool MenuItem(string label, string shortcut, bool selected, bool enabled)
        {
            return ImGuiMenuItem2(label, shortcut, selected, enabled);
        }

        public static void LogFinish()
        {
            ImGuiLogFinish();
        }

        public static void TextUnformatted(string text)
        {
            ImGuiTextUnformatted(text);
        }

        public static void TextUnformatted(string text, string textEnd)
        {
            ImGuiTextUnformatted1(text, textEnd);
        }

        public static void EndChild()
        {
            ImGuiEndChild();
        }
        
        public static void SetStyle(ref ImGuiStyle style)
        {
            ImGuiSetStyle(ref style);
        }      

        public static ImGuiStyle GetStyle()
        {
            ImGuiStyle style;
            ImGuiGetStyle(out style);
            return style;
        }

        public static void SetIO(ref ImGuiIO io)
        {
            ImGuiSetIO(ref io);
        }

        public static ImGuiIO GetIO()
        {
            ImGuiIO io;
            ImGuiGetIO(out io);
            return io;
        }

        public static IntPtr AddFontFromFileTTF(string filepath, float sizePixels)
        {
            return ImGuiAddFontFromFileTTF(filepath, sizePixels);
        }
    }
}