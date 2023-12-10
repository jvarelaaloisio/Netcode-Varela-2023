using Unity.Netcode;
using Unity.Netcode.Editor.Configuration;
using UnityEditor;

namespace Core.Editor
{
    public static class EditorMethods
    {
        [MenuItem("Tools/Netcode/Select Default Network prefabs asset")]
        private static void SelectDefaultNetworkPrefabsAsset()
        {
            var networkPrefabsPath = NetcodeForGameObjectsProjectSettings.instance.NetworkPrefabsPath;
            var networkPrefabsAsset = AssetDatabase.LoadAssetAtPath(networkPrefabsPath, typeof(NetworkPrefabsList));
            Selection.SetActiveObjectWithContext(networkPrefabsAsset, networkPrefabsAsset);
            EditorGUIUtility.PingObject(networkPrefabsAsset);
        }
    }
}