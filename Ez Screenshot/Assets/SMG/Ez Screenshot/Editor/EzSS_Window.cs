using UnityEngine;
using UnityEditor;

namespace SMG.EzScreenshot
{
    public class EzSS_Window : EditorWindow
    {
        [MenuItem("Window/Ez Screenshot/Open")]
        public static void OpenWindow()
        {
            EzSS_Window _window = (EzSS_Window)EditorWindow.GetWindow(typeof(EzSS_Window));
        }
    }
}