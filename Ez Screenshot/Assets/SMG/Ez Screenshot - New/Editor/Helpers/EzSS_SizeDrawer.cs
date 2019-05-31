using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SMG.EzScreenshot
{
    public static class EzSS_SizeDrawer
    {
        private static int maxSize = 4096;
        private static int minSize = 128;

        public static Vector2 Draw(EzSS_AspectRatio.AspectType aspectType, string aspectRatio, int width, int height, bool isMockup)
        {
            int _width = width;
            int _height = height;

            if (aspectType == EzSS_AspectRatio.AspectType.Portrait)
            {
                // Width
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Width:");
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.TextArea(_width.ToString());
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
                // Height
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Height:");
                GUILayout.Space(-15);
                _height = EditorGUILayout.IntSlider(_height, minSize, maxSize);
                EditorGUILayout.EndHorizontal();
                // Calculate de width based on the height value
                _width = Mathf.RoundToInt((float)_height * EzSS_AspectRatio.AspectRatioResults[aspectRatio]);
            }
            else if (aspectType == EzSS_AspectRatio.AspectType.Landscape)
            {
                // Width
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Width:");
                GUILayout.Space(-15);
                _width = EditorGUILayout.IntSlider(_width, minSize, maxSize);
                EditorGUILayout.EndHorizontal();
                // Height
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Height:");
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.TextArea(_height.ToString());
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
                // Calculate the height based on the width value
                _height = Mathf.RoundToInt(_width / EzSS_AspectRatio.AspectRatioResults[aspectRatio]);

            }
            else
            {
                // Width
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Width:");
                GUILayout.Space(-15);
                _width = EditorGUILayout.IntSlider(_width, minSize, maxSize);
                EditorGUILayout.EndHorizontal();
                // Height
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Height:");
                GUILayout.Space(-15);
                _height = EditorGUILayout.IntSlider(_height, minSize, maxSize);
                EditorGUILayout.EndHorizontal();
            }

            return new Vector2(_width, _height);
        }
    }
}