using System;
using System.Runtime.InteropServices;

namespace DearImGui
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ImGuiInputTextCallbackData
    {
        public ImGuiInputTextFlags EventFlag;      // One ImGuiInputTextFlags_Callback*    // Read-only
        public ImGuiInputTextFlags Flags;          // What user passed to InputText()      // Read-only
        public IntPtr               UserData;       // What user passed to InputText()      // Read-only

        // Arguments for the different callback events
        // - To modify the text buffer in a callback, prefer using the InsertChars() / DeleteChars() function. InsertChars() will take care of calling the resize callback if necessary.
        // - If you know your edits are not going to resize the underlying buffer allocation, you may modify the contents of 'Buf[]' directly. You need to update 'BufTextLen' accordingly (0 <= BufTextLen < BufSize) and set 'BufDirty'' to true so InputText can update its internal state.
        public ushort EventChar;      // Character input                      // Read-write   // [CharFilter] Replace character with another one, or set to zero to drop. return 1 is equivalent to setting EventChar=0;
        public ImGuiKey EventKey;       // Key pressed (Up/Down/TAB)            // Read-only    // [Completion,History]
        public IntPtr Buf;            // Text buffer                          // Read-write   // [Resize] Can replace pointer / [Completion,History,Always] Only write to pointed data, don't replace the actual pointer!
        public int BufTextLen;     // Text length (in bytes)               // Read-write   // [Resize,Completion,History,Always] Exclude zero-terminator storage. In C land: == strlen(some_text), in C++ land: string.length()
        public int BufSize;        // Buffer size (in bytes) = capacity+1  // Read-only    // [Resize,Completion,History,Always] Include zero-terminator storage. In C land == ARRAYSIZE(my_char_array), in C++ land: string.capacity()+1
        public bool BufDirty;       // Set if you modify Buf/BufTextLen!    // Write        // [Completion,History,Always]
        public int CursorPos;      //                                      // Read-write   // [Completion,History,Always]
        public int SelectionStart; //                                      // Read-write   // [Completion,History,Always] == to SelectionEnd when no selection)
        public int SelectionEnd;   //                                      // Read-write   // [Completion,History,Always]

        // Helper functions for text manipulation.
        // Use those function to benefit from the CallbackResize behaviors. Calling those function reset the selection.
        public void SelectAll()
        { 
            SelectionStart = 0; 
            SelectionEnd = BufTextLen; 
        }

        public void ClearSelection()
        { 
            SelectionStart = SelectionEnd = BufTextLen; 
        }

        public bool HasSelection()
        { 
            return SelectionStart != SelectionEnd; 
        }

        public void DeleteChars(int pos, int bytes_count)
        {
            if(pos + bytes_count > BufTextLen)
            {
                return;
            }

            IntPtr p_dst = Buf + pos;            
            IntPtr p_src = Buf + pos + bytes_count;

            unsafe
            {
                string src = Marshal.PtrToStringUTF8(p_src);
                char* dst = (char*)p_dst.ToPointer();

                for(int i = 0; i < src.Length; i++)
                {
                    char c = src[i];
                    *dst++ = c;
                }
                
                *dst = '\0';

                if (CursorPos >= pos + bytes_count)
                    CursorPos -= bytes_count;
                else if (CursorPos >= pos)
                    CursorPos = pos;
                SelectionStart = SelectionEnd = CursorPos;
                BufDirty = true;
                BufTextLen -= bytes_count;
            }
        }

        public void InsertChars(int pos, string text, string text_end)
        {

        }
    }
}
