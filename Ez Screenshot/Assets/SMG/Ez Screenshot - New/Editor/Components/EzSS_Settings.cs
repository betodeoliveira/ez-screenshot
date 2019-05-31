using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditorInternal;

namespace SMG.EzScreenshot
{
    public class EzSS_Settings : ScriptableObject
    {
        // General
        private SerializedProperty property;
        private bool showConfiguration = true;
        // Encode
        public enum EncodeType
        {
            PNG,
            JPG
        };
        public EncodeType encodeType = EncodeType.PNG;
        public int jpgQuality = 75;
        // Name
        public string nameTemp = "screenshot";
        public string nameFinal = "";
        public bool useDate = false;
        public bool useTime = false;
        // Save at
        public string saveAtPath = "";

        public void Draw()
        {
            EzSS_Style.DrawUILine(EzSS_Style.uiLineColor);
            EditorGUILayout.BeginHorizontal();
            showConfiguration = EzSS_Style.DrawFoldoutHeader("Encode Settings", showConfiguration);
            EzSS_Style.DrawHelpButton("https://solomidgames.com/guides/ez-screenshot/encode-settings.html");
            EditorGUILayout.EndHorizontal();
            if (showConfiguration)
            {
                EditorGUI.indentLevel++;
                DrawEncodeSettings();
                EditorGUILayout.Space();
                DrawNameSettings();
                EditorGUILayout.Space();
                DrawSaveAtSettings();
                EditorGUI.indentLevel--;
            }
        }

        private void DrawEncodeSettings()
        {
            encodeType = (EncodeType)EditorGUILayout.EnumPopup("Encode Type:", encodeType);
            if (encodeType == EncodeType.JPG)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Quality:");
                GUILayout.Space(-30);
                jpgQuality = EditorGUILayout.IntSlider(jpgQuality, 1, 100);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
        }

        private void DrawNameSettings()
        {
            nameTemp = EditorGUILayout.TextField("Name:", nameTemp);
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Use Date:");
            GUILayout.Space(-30);
            useDate = EditorGUILayout.Toggle(useDate);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Use Time:");
            GUILayout.Space(-30);
            useTime = EditorGUILayout.Toggle(useTime);
            EditorGUILayout.EndHorizontal();
            // Cofigurates the name and display it
            nameFinal = nameTemp;
            if (useDate)
                nameFinal = nameFinal + "_" + DateTime.Now.ToString("yy-MMM-dd");
            if (useTime)
                nameFinal = nameFinal + "-" + DateTime.Now.ToString("HH-mm-ss");
            EditorGUI.indentLevel--;
            // Name Preview
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Preview:", nameFinal);
            EditorGUI.EndDisabledGroup();
        }

        private void DrawSaveAtSettings()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(" ");
            GUILayout.Space(-15);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(-1);
            // Location Text Area
            if (string.IsNullOrEmpty(saveAtPath))
                EditorGUILayout.TextField(FEEDBACKS.Configuration.mustBrowseLocation);
            else
                EditorGUILayout.TextField(saveAtPath);
            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();
            GUILayout.Space(-10);
            EzSS_Style.DrawFindButton(delegate
            {
                saveAtPath = EditorUtility.SaveFolderPanel("Save At", saveAtPath, Application.dataPath);
            });
            EditorGUILayout.EndHorizontal();
            // Draw the Prefix Label
            GUILayout.Space(-23);
            EditorGUILayout.LabelField("Save At:");
        }
    }
}