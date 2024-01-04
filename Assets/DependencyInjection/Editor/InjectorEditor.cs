using DependencyInjection.Scripts.Components;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace DependencyInjection.Editor
{
    [CustomEditor(typeof(Injector))]
    public class InjectorEditor : UnityEditor.Editor
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