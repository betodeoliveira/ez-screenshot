using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace SMG.EzScreenshot
{
    public class EzSS_Mockup : ScriptableObject
    {
        // General
        private EzSS_EncodeSettings encodeSettings;
        private bool showMockup = true;
        private EzSS_Smartphones smartphones = new EzSS_Smartphones();
        private EzSS_Consoles consoles = new EzSS_Consoles();
        private EzSS_Computers computers = new EzSS_Computers();
        private EzSS_Tablets tablets = new EzSS_Tablets();
        private EzSS_Watches watches = new EzSS_Watches();
        // These dictionaries will chance when the category changes
        public Dictionary<Categories, List<string>> mockups = new Dictionary<Categories, List<string>>();
        public Dictionary<string, List<string>> mockupsColors = new Dictionary<string, List<string>>();
        // These dictionaries will be populated inside the init method
        public Dictionary<string, bool> mockupsMultiOriented = new Dictionary<string, bool>();
        public Dictionary<string, Vector2> mockupsPreviewSizes = new Dictionary<string, Vector2>();
        public Dictionary<string, Vector2> mockupsTextureSize = new Dictionary<string, Vector2>();
        public Dictionary<string, string> mockupsAspects = new Dictionary<string, string>();
        public Dictionary<string, Vector2> mockupsScreenSize = new Dictionary<string, Vector2>();
        public Dictionary<string, Vector2> mockupsScreenOffset = new Dictionary<string, Vector2>();
        // The available categories
        public enum Categories
        {
            computer,
            console,
            smartphone,
            tablet,
            watch
        }
        public Categories selectedCategory = Categories.smartphone;
        // The orientations available
        public enum Orientations
        {
            portrait,
            landscapeLeft,
            landscapeRight
        }
        public Orientations selectedOrientation = Orientations.portrait;
        // Selection variables
        public int selectedMockupIndex;
        public int selectedColorIndex;
        private string selectedCategoryName;
        private string selectedMockupName;
        private string selectedColorName;
        private string selectedOrientationName;
        public string selectedAspect;
        public bool selectedMultiOriented;
        public Vector2 selectedPreviewSize;
        public Vector2 selectedTextureSize;
        public Vector2 selectedScreenSize;
        public Vector2 selectedScreenOffset;
        // Mockup textures
        public Texture2D mockupTexture;
        public Texture2D screenTexture;
        private readonly string texturesURL = "https://solomidgames.com/projects/ezScreenshot/mockups/";
        public List<string> mockupsTexturesKeys = new List<string>();
        public List<string> screensTexturesKeys = new List<string>();
        public List<Texture2D> mockupsTextures = new List<Texture2D>();
        public List<Texture2D> screensTextures = new List<Texture2D>();
        private string lastMockupName;
        private string lastColorName;
        private string lastOrientationName;
        private bool downloadMockupTexture = false;
        private bool downloadScreenTexture = false;
        private bool isFirstDownload = false;
        private bool forceGuiChange = false;
        private int mockupDownloadDelayCounter; // Delays the downloaded of the texture when the project gets loaded

        public void Init(EzSS_EncodeSettings encodeSettings)
        {
            this.encodeSettings = encodeSettings;
            smartphones.Init(this);
            consoles.Init(this);
            computers.Init(this);
            tablets.Init(this);
            watches.Init(this);
            // Set the names
            selectedMockupName = mockups[selectedCategory][selectedMockupIndex];
            selectedColorName = mockupsColors[selectedMockupName][selectedColorIndex];
            selectedCategoryName = selectedCategory.ToString();
            selectedOrientationName = selectedOrientation.ToString();
            isFirstDownload = true;
        }

        public void Draw(int mockupDownloadDelayCounter)
        {
            this.mockupDownloadDelayCounter = mockupDownloadDelayCounter;
            // This is important because when the texture is downloaded the gui change is missid and the values storaged for mokcu and color are wrong.
            if (forceGuiChange)
            {
                forceGuiChange = false;
                GUI.changed = true;
            }
            if (encodeSettings.useMockup)
            {
                Rect _rect = EditorGUILayout.BeginVertical();
                EzSS_Style.DrawUILine(EzSS_Style.uiLineColor);
                EditorGUILayout.BeginHorizontal();
                showMockup = EzSS_Style.DrawFoldoutHeader("Mockup", showMockup);
                EzSS_Style.DrawHelpButton("https://solomidgames.com/guides/ez-screenshot/mockup.html");
                EditorGUILayout.EndHorizontal();
                if (showMockup)
                {
                    EditorGUI.indentLevel++;
                    DrawMockupSelector();
                    DrawPreview();
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();
                // Draws the colored line
                if (!showMockup || mockupTexture == null)
                {
                    EditorGUI.DrawRect(new Rect(_rect.x - 3, _rect.y + 7, 2f, _rect.height), new Color32(185, 84, 67, 255));
                }
                else
                {
                    EditorGUI.DrawRect(new Rect(_rect.x + 1, _rect.y + 7, 2f, _rect.height), new Color32(185, 84, 67, 255));
                }
            }
        }

        private void DrawMockupSelector()
        {
            // Choose the category to filter the available mockups
            selectedCategory = (Categories)EditorGUILayout.EnumPopup("Category:", selectedCategory);
            selectedCategoryName = selectedCategory.ToString();
            // Choose the mockup and set it's name
            if (selectedMockupIndex >= mockups[selectedCategory].Count)
            {
                selectedMockupIndex = 0;
            }
            selectedMockupIndex = EditorGUILayout.Popup("Mockup:", selectedMockupIndex, mockups[selectedCategory].ToArray());
            selectedMockupName = mockups[selectedCategory][selectedMockupIndex];
            // Before choosing the color is important to check if the selected index is bigger than the available colors length
            if (selectedColorIndex >= mockupsColors[selectedMockupName].Count)
            {
                selectedColorIndex = 0;
            }
            // Choose the mockup color based on the mokcup name
            selectedColorIndex = EditorGUILayout.Popup("Color:", selectedColorIndex, mockupsColors[selectedMockupName].ToArray());
            selectedColorName = mockupsColors[selectedMockupName][selectedColorIndex];
            // Set other basic values
            selectedTextureSize = mockupsTextureSize[selectedMockupName];
            selectedScreenSize = mockupsScreenSize[selectedMockupName];
            selectedPreviewSize = mockupsPreviewSizes[selectedMockupName];
            selectedAspect = mockupsAspects[selectedMockupName];
            if (mockupsScreenOffset.ContainsKey(selectedMockupName))
            {
                selectedScreenOffset = mockupsScreenOffset[selectedMockupName];
            }
            else
            {
                selectedScreenOffset = Vector2.zero;
            }
            // Choose the mockup orientation if it is available
            selectedMultiOriented = mockupsMultiOriented[selectedMockupName];
            if (selectedMultiOriented)
            {
                selectedOrientation = (Orientations)EditorGUILayout.EnumPopup("Orientation:", selectedOrientation);
                if (EzSS_AspectRatio.AspectRatioResults[selectedAspect] < 1 && (selectedOrientation == Orientations.landscapeLeft || selectedOrientation == Orientations.landscapeRight))
                {
                    selectedAspect = EzSS_AspectRatio.InvertAspectRatio[selectedAspect];
                }
            }
            else
            {
                selectedOrientation = Orientations.portrait;
            }
            selectedOrientationName = selectedOrientation.ToString();
            // After the selection check if the textures exists on the project
            CheckMockupTextures();
        }

        private void DrawPreview()
        {
            EditorGUILayout.LabelField("Preview:");
            if (mockupTexture != null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                if (selectedOrientation == Orientations.portrait)
                    GUILayout.Label(mockupTexture, GUILayout.Width(selectedPreviewSize.x), GUILayout.Height(selectedPreviewSize.y));
                else
                    GUILayout.Label(mockupTexture, GUILayout.Width(selectedPreviewSize.y), GUILayout.Height(selectedPreviewSize.x));
                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();
            }
        }

        private void CheckMockupTextures()
        {
            if (encodeSettings.useMockup &&
            (!string.Equals(lastMockupName, selectedMockupName) ||
            !string.Equals(lastColorName, selectedColorName) ||
            !string.Equals(lastOrientationName, selectedOrientationName) ||
            mockupTexture == null))
            {
                // creates the final url
                string _mockupUrl = texturesURL + selectedCategoryName + "/" + selectedMockupName + "-" + selectedColorName + "-" + selectedOrientationName + ".png";
                string _screenUrl = texturesURL + selectedCategoryName + "/" + selectedMockupName + "-screen" + "-" + selectedOrientationName + ".png";
                // Set the variables to download the textures
                downloadScreenTexture = true;
                downloadMockupTexture = true;
                // The screen texture will be downloaded if the key isn't present inside the dictionary
                if (screensTexturesKeys.Contains(_screenUrl))
                {
                    downloadScreenTexture = false;
                    screenTexture = screensTextures[screensTexturesKeys.IndexOf(_screenUrl)];
                }
                // The mockup texture will be downloaded if the key isn't present inside the dictionary
                if (mockupsTexturesKeys.Contains(_mockupUrl))
                {
                    downloadMockupTexture = false;
                    mockupTexture = mockupsTextures[mockupsTexturesKeys.IndexOf(_mockupUrl)];
                    // Updated the mockup information
                    lastMockupName = selectedMockupName;
                    lastColorName = selectedColorName;
                    lastOrientationName = selectedOrientationName;
                }

                if (mockupDownloadDelayCounter < 5)
                {
                    return;
                }
                else if(isFirstDownload)
                {
                    DownloadScreenTexture(_screenUrl);
                    DownloadMockupTexture(_mockupUrl);
                    isFirstDownload = false;
                }
                else if (downloadMockupTexture && !downloadScreenTexture)
                {
                    DownloadMockupTexture(_mockupUrl);
                    GUIUtility.ExitGUI();
                }
                else if (!downloadMockupTexture && downloadScreenTexture)
                {
                    DownloadScreenTexture(_screenUrl);
                    GUIUtility.ExitGUI();
                }
                else if (downloadMockupTexture && downloadScreenTexture) 
                {
                    DownloadScreenTexture(_screenUrl);
                    DownloadMockupTexture(_mockupUrl);
                    GUIUtility.ExitGUI();
                }
                else
                {
                    // Both textures are inside the dictionary
                }
            }
        }

        private void DownloadMockupTexture(string url)
        {
            EditorUtility.DisplayProgressBar(FEEDBACKS.Titles.wait, FEEDBACKS.Mockup.definingMockTexture, 0);
            WWW _wwwLoader = new WWW(url);
            // Wait until download is done
            while (!_wwwLoader.isDone)
            {
                EditorUtility.DisplayProgressBar(FEEDBACKS.Titles.wait, FEEDBACKS.Mockup.definingMockTexture, _wwwLoader.progress);
            }
            // Download is done
            EditorUtility.ClearProgressBar();
            // Check for errors
            if (!string.IsNullOrEmpty(_wwwLoader.error))
            {
                encodeSettings.useMockup = false;
                EditorUtility.DisplayDialog(FEEDBACKS.Titles.attention, FEEDBACKS.Mockup.errorDefiningMockupTexture + _wwwLoader.error, FEEDBACKS.Buttons.close);
                return;
            }
            // Set the mockupImage
            mockupTexture = _wwwLoader.texture;
            mockupsTexturesKeys.Add(url);
            mockupsTextures.Add(mockupTexture);
            // Updated the mockup information
            forceGuiChange = true;
            lastMockupName = selectedMockupName;
            lastColorName = selectedColorName;
            lastOrientationName = selectedOrientationName;
        }

        private void DownloadScreenTexture(string url)
        {
            EditorUtility.DisplayProgressBar(FEEDBACKS.Titles.wait, FEEDBACKS.Mockup.definingMockupScreen, 0);
            WWW _wwwLoader = new WWW(url);
            // Wait until download is done
            while (!_wwwLoader.isDone)
            {
                EditorUtility.DisplayProgressBar(FEEDBACKS.Titles.wait, FEEDBACKS.Mockup.definingMockupScreen, _wwwLoader.progress);
            }
            // Download is done
            EditorUtility.ClearProgressBar();
            // Check for errors
            if (!string.IsNullOrEmpty(_wwwLoader.error))
            {
                encodeSettings.useMockup = false;
                EditorUtility.DisplayDialog(FEEDBACKS.Titles.attention, FEEDBACKS.Mockup.errorDefiningMockupScreen + _wwwLoader.error, FEEDBACKS.Buttons.close);
            }
            // Set the mockupImage
            screenTexture = _wwwLoader.texture;
            screensTexturesKeys.Add(url);
            screensTextures.Add(screenTexture);
        }
    }
}