using System.Collections;
using System.Collections.Generic;
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
            _window.minSize = new Vector2(380, 650);
            _window.titleContent = new GUIContent(" Ez Screenshot", EditorGUIUtility.FindTexture("d_RectTransformBlueprint"));
            _window.autoRepaintOnSceneChange = true;
            _window.Show();
        }

        [MenuItem("Window/Ez Screenshot/Guide")]
        public static void Guide()
        {
            Application.OpenURL("https://solomidgames.com/guides/ez-screenshot/quick-overview.html");
        }

        [MenuItem("Window/Ez Screenshot/Help")]
        public static void Help()
        {
            Application.OpenURL("mailto:help@solomidgames.com");
        }

        [MenuItem("Window/Ez Screenshot/Forum Thread")]
        public static void ForumThread()
        {
            Application.OpenURL("https://forum.unity.com/threads/released-ez-screenshot.633328/");
        }

        [MenuItem("Window/Ez Screenshot/More Assets")]
        public static void MoreAssets()
        {
            Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:11524");
        }

        [MenuItem("Window/Ez Screenshot/Website")]
        public static void Website()
        {
            Application.OpenURL("https://solomidgames.com");
        }

        [MenuItem("Window/Ez Screenshot/Follow us on Twitter")]
        public static void FollowTwitter()
        {
            Application.OpenURL("https://twitter.com/solomidgames");
        }

        // Window components
        private Vector2 scrollPos;

        private void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.EndScrollView();    
        }
    }
}