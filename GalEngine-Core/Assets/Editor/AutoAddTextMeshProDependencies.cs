using TMPro;
using UnityEditor;
using UnityEngine;

namespace Editor {
    [InitializeOnLoad]
    public class Editor : MonoBehaviour
    {
        static Editor()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        private static void OnHierarchyChanged()
        {
            foreach (var go in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (go.GetComponent<TextMeshProUGUI>() != null)
                {
                    AddRequiredComponents(go);
                }
            }
        }

        private static void AddRequiredComponents(GameObject go)
        {
            // 检查是否已经有这些组件，如果没有则添加
            if (go.GetComponent<FontWeightMarker>() == null)
                go.AddComponent<FontWeightMarker>();
        
            if (go.GetComponent<TextIdMarker>() == null)
                go.AddComponent<TextIdMarker>();
        }
    }
}