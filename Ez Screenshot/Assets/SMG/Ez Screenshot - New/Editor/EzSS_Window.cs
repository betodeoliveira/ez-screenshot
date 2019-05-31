using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SMG.EzScreenshot
{
    public class EzSS_Window : EditorWindow
    {
        private Vector2 scrollPos;
        // Components
        private EzSS_Settings settings;
        private EzSS_Resolution resolution;

        private void OnEnable() 
        {
            settings = ScriptableObject.CreateInstance<EzSS_Settings>();
            resolution = ScriptableObject.CreateInstance<EzSS_Resolution>();
        }

        private void OnGUI() 
        {
            settings.Draw();
            resolution.Draw();
        }

        [MenuItem("Window/NEW Ez Screenshot/Open")]
        public static void OpenWindow()
        {
            EzSS_Window _window = (EzSS_Window)EditorWindow.GetWindow(typeof(EzSS_Window));
            _window.minSize = new Vector2(380, 650);
            _window.maxSize = new Vector2(500, 1000);
            _window.titleContent = new GUIContent(" Ez Screenshot", EditorGUIUtility.FindTexture("d_RectTransformBlueprint"));
            _window.autoRepaintOnSceneChange = true;
            _window.Show();
        }
    }
}