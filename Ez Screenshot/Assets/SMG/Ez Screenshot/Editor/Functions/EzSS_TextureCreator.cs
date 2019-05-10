using UnityEngine;
using UnityEditor;
using System;

namespace SMG.EzScreenshot
{
    public static class EzSS_TextureCreator
    {
        public const TextureFormat textureFormat = TextureFormat.ARGB32;

        public static Texture2D GameView(EzSS_EncodeSettings configuration, int width, int height)
        {
            // Creates three temp textures that will be the base to create the final texture
            Texture2D _resultTexture = new Texture2D(width, height, textureFormat, true);
            Texture2D _backgroundTexture = new Texture2D(width, height, textureFormat, true);
            Texture2D _foregroundTexture = new Texture2D(width, height, textureFormat, true);
            // Creates an array with the cameras list
            Camera[] _cameras = configuration.cameras.ToArray();
            // Sort the array based on the depth of each camera
            Array.Sort(_cameras, delegate (Camera camera0, Camera camera1)
            {
                return EditorUtility.NaturalCompare(camera0.depth.ToString(), camera1.depth.ToString());
            });
            // Combines the render texture of each camera in just one texture
            for (int i = 0; i < _cameras.Length; i++)
            {
                if (i == 0)
                    _resultTexture = SetRenderTexture(_cameras[i], width, height);
                else
                {
                    _backgroundTexture = _resultTexture;
                    _foregroundTexture = SetRenderTexture(_cameras[i], width, height);
                    _resultTexture = EzSS_TextureCombinator.Simple(_backgroundTexture, _foregroundTexture, true);
                }
            }
            // Return the final texture
            return _resultTexture;
        }

        private static Texture2D SetRenderTexture(Camera cam, int width, int height)
        {
            RenderTexture _renderTexture = new RenderTexture(width, height, 24);
            Texture2D _texture = new Texture2D(width, height, textureFormat, true);
            RenderTexture.active = _renderTexture;
            cam.targetTexture = _renderTexture;
            cam.Render();
            _texture.ReadPixels(new Rect(0, 0, width, height), 0, 0, true);
            _texture.Apply();
            cam.targetTexture = null;
            RenderTexture.active = null;
            return _texture;
        }

        public static Texture2D Mockup(EzSS_Mockup mockup, int width, int height)
        {
            Texture2D _mockupTexture = new Texture2D(mockup.mockupTexture.width, mockup.mockupTexture.height, textureFormat, true);
            _mockupTexture.SetPixels(mockup.mockupTexture.GetPixels());
            // Scale the clone to match the wanted size
            _mockupTexture = EzSS_TextureScaler.ScaleTexture(_mockupTexture, width, height, true, FilterMode.Trilinear);
            return _mockupTexture;
        }

        public static Texture2D MockupScreen(EzSS_Mockup mockup, int width, int height)
        {
            Texture2D _mockupScreenTexture = new Texture2D(mockup.screenTexture.width, mockup.screenTexture.height, textureFormat, true);
            _mockupScreenTexture.SetPixels(mockup.screenTexture.GetPixels());
            // Scale the clone to match the wanted size
            _mockupScreenTexture = EzSS_TextureScaler.ScaleTexture(_mockupScreenTexture, width, height, true, FilterMode.Trilinear);
            return _mockupScreenTexture;
        }

        public static Texture2D Shadow(EzSS_Shadow shadow, Texture2D fgTexture)
        {
            // Creates the shadow base texture
            Texture2D _shadowTexture = new Texture2D(fgTexture.width, fgTexture.height, textureFormat, true);
            // Creates the hard shadow
            for (int y = 0; y < fgTexture.height; y++)
            {
                for (int x = 0; x < fgTexture.width; x++)
                {
                    if (fgTexture.GetPixel(x, y).a <= 0)
                    {
                        _shadowTexture.SetPixel(x, y, Color.clear);
                    }
                    else
                    {
                        _shadowTexture.SetPixel(x, y, shadow.color);
                    }
                }
            }
            _shadowTexture.Apply();
            // Applys the blur
            if (shadow.softness > 0)
            {
                Texture2D _gradientTexture = new Texture2D(fgTexture.width + (shadow.softness * 2), fgTexture.height + (shadow.softness * 2), textureFormat, true);
                Color[] _resultColors = _gradientTexture.GetPixels();
                for (int i = 0; i < _resultColors.Length; i++)
                {
                    _resultColors[i] = Color.clear;
                }
                _gradientTexture.SetPixels(_resultColors);
                _gradientTexture = EzSS_TextureCombinator.Simple(_gradientTexture, _shadowTexture, false);
                // Scale down the texture to create the shadow faster
                int _newSize = Math.Min(_gradientTexture.width, _gradientTexture.height);
                int _baseSize = _newSize / 3;
                bool _scaleUp = false;
                _baseSize = _baseSize < 300 ? 300 : _baseSize;
                if (_newSize > _baseSize)
                {
                    _scaleUp = true;
                    _newSize = _baseSize - _newSize;
                    _gradientTexture = EzSS_TextureScaler.ScaleTextureProportionally(_gradientTexture, _newSize, false, FilterMode.Trilinear);
                }

                _gradientTexture = EzSS_TextureFilter.Convolution(_gradientTexture, EzSS_TextureFilter.LINEAR_KERNEL, shadow.softness);

                if (_scaleUp)
                {
                    // The scale up must be based on the higher value to match the actual size of the fgTexture
                    if (fgTexture.width > fgTexture.height)
                    {
                        _newSize = (fgTexture.width - _gradientTexture.width) + (shadow.softness * 2);
                    }
                    else
                    {
                        _newSize = (fgTexture.height - _gradientTexture.height) + (shadow.softness * 2);
                    }
                    // Scale up
                    _gradientTexture = EzSS_TextureScaler.ScaleTextureProportionally(_gradientTexture, _newSize, false, FilterMode.Trilinear);
                }

                return _gradientTexture;
            }

            return _shadowTexture;
        }

        public static Texture2D Background(EzSS_Background background, int width, int height)
        {
            Texture2D backgroundTexture = new Texture2D(width, height, textureFormat, true);
            Color[] _backgroundPixels = backgroundTexture.GetPixels();
            // To prevent an error clear all the pixels
            if (background.bgColors.Count <= 0)
            {
                for (int i = 0; i < _backgroundPixels.Length; i++)
                {
                    _backgroundPixels[i] = Color.clear;
                }
                backgroundTexture.SetPixels(_backgroundPixels);
            }
            else if (background.bgColors.Count == 1)
            {
                for (int i = 0; i < _backgroundPixels.Length; i++)
                {
                    _backgroundPixels[i] = background.bgColors[0].color;
                }
                backgroundTexture.SetPixels(_backgroundPixels);
            }
            else
            {
                // Created the gradient and the variables
                Gradient _gradient = new Gradient();
                GradientColorKey[] _colorKey;
                GradientAlphaKey[] _alphaKey;
                // How many steps will be used to set the color time?
                // float _colorStep = 1f / background.backgroundColors.Count;
                // Configure the color and alpha keys
                _colorKey = new GradientColorKey[background.bgColors.Count];
                _alphaKey = new GradientAlphaKey[background.bgColors.Count];
                for (int i = 0; i < background.bgColors.Count; i++)
                {
                    // This part is important because the initial _step must be 0 and the final _step must be 1
                    // float _step = i > 0 ? _colorStep * (i + 1) : 0;
                    // Set the color and alpha using the _step as the time
                    _colorKey[i].color = background.bgColors[i].color;
                    _colorKey[i].time = background.bgColors[i].time;
                    _alphaKey[i].alpha = background.bgColors[i].color.a;
                    _alphaKey[i].time = background.bgColors[i].time;
                }
                // Set the gradient mode
                if (background.bgType == EzSS_Background.BgTypes.solidVertical || background.bgType == EzSS_Background.BgTypes.solidHorizontal)
                {
                    _gradient.mode = GradientMode.Fixed;
                }
                else
                {
                    _gradient.mode = GradientMode.Blend;
                }
                // Set the keys
                _gradient.SetKeys(_colorKey, _alphaKey);
                // Set the background pixels
                if (background.bgType == EzSS_Background.BgTypes.solidVertical || background.bgType == EzSS_Background.BgTypes.gradientVertical)
                {
                    float _yStep = 1f / backgroundTexture.height;
                    for (int y = 0; y < backgroundTexture.height; y++)
                    {
                        Color color = _gradient.Evaluate(y * _yStep);
                        for (int x = 0; x < backgroundTexture.width; x++)
                        {
                            backgroundTexture.SetPixel(x, y, color);
                        }
                    }
                }
                else
                {
                    float _xStep = 1f / backgroundTexture.width;
                    for (int x = 0; x < backgroundTexture.width; x++)
                    {
                        Color color = _gradient.Evaluate(x * _xStep);
                        for (int y = 0; y < backgroundTexture.height; y++)
                        {
                            backgroundTexture.SetPixel(x, y, color);
                        }
                    }
                }
            }

            backgroundTexture.Apply();
            return backgroundTexture;
        }

        public static Texture2D Transparent(int width, int height)
        {
            Texture2D _noAlphaTexture = new Texture2D(width, height, textureFormat, true);
            Color[] _noAlphaPixels = _noAlphaTexture.GetPixels();
            for (int i = 0; i < _noAlphaPixels.Length; i++)
                _noAlphaPixels[i] = Color.clear;

            _noAlphaTexture.SetPixels(_noAlphaPixels);
            _noAlphaTexture.Apply();
            return _noAlphaTexture;
        }
    }
}