using DependencyInjection.Scripts.Attributes;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace DependencyInjection.Editor
{
    [CustomPropertyDrawer(typeof(InjectAttribute))]
    public class InjectPropertyDrawer : PropertyDrawer
    {
        private Texture2D _injectorIcon;

        Texture2D LoadIcon()
        {
            if (_injectorIcon == null)
            {
                _injectorIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/DependencyInjection/Icons/injector-icon.png");
            }
            return _injectorIcon;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _injectorIcon = LoadIcon();
            var iconRect = new Rect(position.x, position.y, 20, 20);
            position.xMin += 24;

            if (_injectorIcon != null)
            {
                var savedColor = GUI.color;
                GUI.color = property.objectReferenceValue == null ? savedColor : Color.green;
                GUI.DrawTexture(iconRect, _injectorIcon);
                GUI.color = savedColor;
            }
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
#endif