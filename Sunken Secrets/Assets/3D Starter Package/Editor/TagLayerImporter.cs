using UnityEditor;
using UnityEngine;
using System.Linq;

namespace DigitalWorlds.StarterPackage3D
{
    /// <summary>
    /// This editor script will add the tags and physics layers needed for the example scene.
    /// It runs automatically when Unity loads the project.
    /// </summary>
    [InitializeOnLoad]
    public static class TagLayerImporter
    {
        // Tags to be added
        private static readonly string[] RequiredTags = {
        "Enemy",
        "Death",
        "Health",
        "PlayerAttack"
    };

        // Layers to be added to the project with their corresponding index
        private static readonly (string name, int index)[] RequiredLayers = { ("Ground", 10) };

        const string ImportKey = "3DStarterPackage_ImportedTagsAndLayers_v1";

        static TagLayerImporter()
        {
            // Only import tags and layers if the user setting is true (i.e., already imported)
            if (EditorUserSettings.GetConfigValue(ImportKey) == "true")
            {
                return;
            }

            SetupTagsAndLayers();
        }

        public static void SetupTagsAndLayers()
        {
            // Add all required tags
            foreach (string tag in RequiredTags)
            {
                AddTag(tag);
            }

            // Add all required layers
            foreach (var (name, index) in RequiredLayers)
            {
                AddLayer(name, index);
            }

            // Mark as imported
            EditorUserSettings.SetConfigValue(ImportKey, "true");

            Debug.Log("Example scene tags and layers have been successfully configured");
        }

        /// <summary>
        /// Adds a tag to the project if it doesn't already exist
        /// </summary>
        /// <param name="tag">The tag name to add</param>
        private static void AddTag(string tag)
        {
            // Load the tag manager
            var tagManagerAsset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if (tagManagerAsset == null || tagManagerAsset.Length == 0)
            {
                Debug.LogError("Failed to load TagManager.asset");
                return;
            }

            var tagManager = new SerializedObject(tagManagerAsset[0]);
            var tagsProp = tagManager.FindProperty("tags");

            if (tagsProp == null)
            {
                Debug.LogError("Failed to find tags property in TagManager");
                return;
            }

            // Check if the tag already exists
            bool tagExists = Enumerable.Range(0, tagsProp.arraySize)
                .Select(i => tagsProp.GetArrayElementAtIndex(i).stringValue)
                .Contains(tag);

            // If tag doesn't exist, add it
            if (!tagExists)
            {
                int index = tagsProp.arraySize;
                tagsProp.InsertArrayElementAtIndex(index);
                tagsProp.GetArrayElementAtIndex(index).stringValue = tag;
                tagManager.ApplyModifiedProperties();
                Debug.Log($"Added tag: {tag}");
            }
        }

        /// <summary>
        /// Sets a layer at the specified index
        /// </summary>
        /// <param name="layerName">The name of the layer to add</param>
        /// <param name="layerIndex">The index (0-31) where the layer should be set</param>
        private static void AddLayer(string layerName, int layerIndex)
        {
            // Validate layer index
            if (layerIndex < 0 || layerIndex > 31)
            {
                Debug.LogError($"Layer index {layerIndex} is out of range (0-31)!");
                return;
            }

            // Skip built-in layers (0-7) with a warning
            if (layerIndex < 8)
            {
                Debug.LogWarning($"Attempting to modify built-in layer at index {layerIndex}. This is not recommended!");
            }

            // Load the tag manager
            var tagManagerAsset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if (tagManagerAsset == null || tagManagerAsset.Length == 0)
            {
                Debug.LogError("Failed to load TagManager.asset!");
                return;
            }

            var tagManager = new SerializedObject(tagManagerAsset[0]);
            var layersProp = tagManager.FindProperty("layers");

            if (layersProp == null)
            {
                Debug.LogError("Failed to find layers property in TagManager!");
                return;
            }

            // Get the layer at the specified index
            var layerProp = layersProp.GetArrayElementAtIndex(layerIndex);

            // Check if the layer exists and is different
            string currentLayerName = layerProp.stringValue;
            if (string.IsNullOrEmpty(currentLayerName))
            {
                layerProp.stringValue = layerName;
                tagManager.ApplyModifiedProperties();
                Debug.Log($"Added layer '{layerName}' at index {layerIndex}");
            }
            else if (currentLayerName != layerName)
            {
                Debug.LogWarning($"Replacing existing layer '{currentLayerName}' with '{layerName}' at index {layerIndex}");
                layerProp.stringValue = layerName;
                tagManager.ApplyModifiedProperties();
            }
        }
    }
}