#if UNITY_EDITOR
namespace Anomaly.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using UnityEditor.SceneManagement;

    public abstract class AssetSpotlightWindow<T> : EditorWindow where T : EditorWindow
    {
        protected static EditorWindow window;
        protected static Vector2 drawableSize;

        private string spotlightInput = string.Empty;

        protected (string name, string path)[] assetList;
        private int[] recommendIndices;
        private Vector2 assetListScroll;


        protected abstract void InitializeAssetList();
        protected abstract void SelectAsset(string path);


        protected static void ShowWindow()
        {
            if (window != null)
            {
                window.Close();
                window = null;
            }
            window = EditorWindow.CreateInstance<T>();

            window.ShowAuxWindow();

            drawableSize = new Vector2(600F, 50F);
            window.minSize = window.maxSize = drawableSize;

            // Center
            var mainWindow = EditorGUIUtility.GetMainWindowPosition();
            var current = window.position;

            current.x = (mainWindow.x + mainWindow.width * 0.5f) - current.width * 0.5f;
            current.y = (mainWindow.y + mainWindow.height * 0.5f) - current.height * 0.5f - 100F;

            window.position = current;

            window.titleContent = new GUIContent("Spotlight");
        }


        private void OnEnable()
        {
            InitializeAssetList();
        }

        private void OnGUI()
        {
            bool isChanged = DisplaySpotlightHeader();

            if (isChanged)
            {
                SearchRecommend();
            }

            DisplayRecommend();

            string finalPath = recommendIndices != null && recommendIndices.Length > 0 ? assetList[recommendIndices[0]].path : string.Empty;
            HandleKeyboardEvent(finalPath);
        }


        private bool DisplaySpotlightHeader()
        {
            var spotlightStyle = new GUIStyle();
            spotlightStyle.normal.textColor = Color.white;
            spotlightStyle.fontSize = (int)drawableSize.y - 10;
            spotlightStyle.margin = new RectOffset(10, 0, 0, 0);

            GUI.SetNextControlName("Spotlight");

            var previousInput = spotlightInput;
            spotlightInput = EditorGUILayout.TextField(spotlightInput, spotlightStyle, GUILayout.Width(drawableSize.x - 16F), GUILayout.Height(drawableSize.y)).Trim();

            GUI.FocusControl("Spotlight");

            return !previousInput.Equals(spotlightInput);
        }

        private void SearchRecommend()
        {
            List<int> indices = new List<int>(assetList.Length);
            for (int i = 0; i < assetList.Length && !string.IsNullOrEmpty(spotlightInput); ++i)
            {
                if (!assetList[i].name.ToLower().Contains(spotlightInput.ToLower())) continue;
                indices.Add(i);
            }

            recommendIndices = indices.ToArray();
        }

        private void DisplayRecommend()
        {
            if (recommendIndices == null) return;

            Vector2 windowSize = drawableSize;

            bool shouldScroll = recommendIndices.Length > 4;

            windowSize.y += 50F * Mathf.Min(4, recommendIndices.Length);
            window.minSize = window.maxSize = windowSize;

            if (!shouldScroll)
            {
                CreateButtons();
                return;
            }

            assetListScroll = EditorGUILayout.BeginScrollView(assetListScroll, false, false, GUILayout.Width(windowSize.x), GUILayout.Height(200F));
            CreateButtons();
            EditorGUILayout.EndScrollView();

            void CreateButtons()
            {
                for (int i = 0; i < recommendIndices.Length; ++i)
                {
                    GUILayout.Space(2.5f);

                    var info = assetList[recommendIndices[i]];

                    if (!GUILayout.Button($"{info.name}\n{info.path}", GUILayout.Width(windowSize.x - (shouldScroll ? 20F : 6F)), GUILayout.Height(45F))) continue;

                    SelectAsset(info.path);
                    return;
                }
            }
        }

        private void HandleKeyboardEvent(string currentSelectedPath)
        {
            var e = Event.current;

            if (e.keyCode == KeyCode.Escape)
            {
                window.Close();
                return;
            }

            if (e.keyCode != KeyCode.Return) return;

            if (string.IsNullOrEmpty(spotlightInput))
            {
                window.Close();
                return;
            }

            if (string.IsNullOrEmpty(currentSelectedPath)) return;

            SelectAsset(currentSelectedPath);
        }
    }
}
#endif