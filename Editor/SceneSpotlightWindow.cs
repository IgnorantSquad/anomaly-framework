#if UNITY_EDITOR
namespace Anomaly.Editor
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEditor.SceneManagement;

    public class SceneSpotlightWindow : AssetSpotlightWindow<SceneSpotlightWindow>
    {
        protected override List<string>[] options => new List<string>[] {
            new List<string> { "#show", "#s", "--s" },
            new List<string> { "#hide", "#h", "--h" },
            new List<string> { "#close", "#c", "--c" }
        };

        [MenuItem("Tools/Organizer/Scene/Spotlight %&SPACE", false, 0)]
        private static void Spotlight()
        {
            ShowWindow();
        }

        protected override void InitializeAssetList()
        {
            var scenes = AssetDatabase.FindAssets("t:Scene");
            assetList = new (string, string)[scenes.Length];
            for (int i = 0; i < scenes.Length; ++i)
            {
                assetList[i].path = AssetDatabase.GUIDToAssetPath(scenes[i]);
                assetList[i].name = Path.GetFileNameWithoutExtension(assetList[i].path);
            }
        }

        protected override void SelectAsset(string path, params string[] options)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            bool show = false, hide = false, close = false;
            for (int i = 0; i < options.Length; ++i)
            {
                var option = options[i];

                show = this.options[0].Contains(option);
                hide = this.options[1].Contains(option);
                close = this.options[2].Contains(option);
            }

            if ((hide || show) && close)
            {
                EditorUtility.DisplayDialog("Exception thrown", "Scene spotlight has wrong option(s)", "Ok");
                return;
            }

            window.Close();

            if (!show && (close || hide))
            {
                var scene = EditorSceneManager.GetSceneByPath(path);
                if (scene == null) return;
                EditorSceneManager.CloseScene(scene, close);
                return;
            }
            else if (show)
            {
                EditorSceneManager.OpenScene(path, hide ? OpenSceneMode.AdditiveWithoutLoading : OpenSceneMode.Additive);
                return;
            }

            EditorSceneManager.OpenScene(path);
        }
    }
}
#endif