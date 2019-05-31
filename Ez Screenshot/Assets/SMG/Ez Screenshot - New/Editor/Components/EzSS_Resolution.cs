using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace SMG.EzScreenshot
{
    public class EzSS_Resolution : ScriptableObject
    {
        // General
        private bool showResolution = true;
        // Screenshot
        public EzSS_AspectRatio.AspectType orientation = EzSS_AspectRatio.AspectType.Free;
        public EzSS_AspectRatio.AspectType lastOrientation = EzSS_AspectRatio.AspectType.Free;
        public int portraitAspectRatioIndex;
        public int landscapeAspectRatioIndex;
        public int screenshotWidth = 1280;
        public int screenshotHeight = 720;
        public string screenshotAspectRatio;
        // Aspect Setter
        public Vector2 aspectRatioVector = Vector2.zero;
        public static string currentAspectRatioName;

        public void Draw()
        {
            EzSS_Style.DrawUILine(EzSS_Style.uiLineColor);
            EditorGUILayout.BeginHorizontal();
            showResolution = EzSS_Style.DrawFoldoutHeader("Screenshot Resolution", showResolution);
            EzSS_Style.DrawHelpButton("https://solomidgames.com/guides/ez-screenshot/resolutions.html");
            EditorGUILayout.EndHorizontal();
            if (showResolution)
            {
                EditorGUI.indentLevel++;
                DrawScreenshotResolution();
                EditorGUI.indentLevel--;
            }
        }

        private void DrawScreenshotResolution()
        {
            orientation = (EzSS_AspectRatio.AspectType)EditorGUILayout.EnumPopup("Orientation:", orientation);
            if (orientation == EzSS_AspectRatio.AspectType.Portrait)
            {
                if (lastOrientation != orientation)
                {
                    screenshotHeight = screenshotWidth;
                    lastOrientation = orientation;
                }
                portraitAspectRatioIndex = EditorGUILayout.Popup("Aspect Ratio:", portraitAspectRatioIndex, EzSS_AspectRatio.portraitAspects);
                screenshotAspectRatio = EzSS_AspectRatio.portraitAspects[portraitAspectRatioIndex];
            }
            else if (orientation == EzSS_AspectRatio.AspectType.Landscape)
            {
                if (lastOrientation != orientation)
                {
                    screenshotWidth = screenshotHeight;
                    lastOrientation = orientation;
                }
                landscapeAspectRatioIndex = EditorGUILayout.Popup("Aspect Ratio:", landscapeAspectRatioIndex, EzSS_AspectRatio.landscapeAspects);
                screenshotAspectRatio = EzSS_AspectRatio.landscapeAspects[landscapeAspectRatioIndex];
            }
            else
            {
                screenshotAspectRatio = EzSS_AspectRatio.AspectType.Free.ToString();
            }
            Vector2 _size = EzSS_SizeDrawer.Draw(orientation, screenshotAspectRatio, screenshotWidth, screenshotHeight, false);
            screenshotWidth = (int)_size.x;
            screenshotHeight = (int)_size.y;
            EditorGUI.indentLevel--;
        }
    }
}