#if UNITY_EDITOR
namespace Anomaly.Editor
{
    using System.IO;
    using UnityEditor;
    using UnityEditor.SceneManagement;

    public class SceneSpotlightWindow : AssetSpotlightWindow<SceneSpotlightWindow>
    {
        [MenuItem("Tools/Spotlight/Scene %&SPACE")]
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

        protected override void SelectAsset(string path)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            window.Close();
            EditorSceneManager.OpenScene(path);
        }
    }
}
#endif