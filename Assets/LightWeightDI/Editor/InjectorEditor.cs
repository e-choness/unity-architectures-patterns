using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace DependencyInjection
{
    [CustomEditor(typeof(Injector))]
    public class InjectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var injector = (Injector) target;

            if (GUILayout.Button("Validate Dependencies"))
            {
                injector.ValidateDependencies();
            }

            if (GUILayout.Button("Clear All Injectable Fields"))
            {
                injector.ClearDependencies();
                EditorUtility.SetDirty(injector);
            }
        }
    }
}
#endif