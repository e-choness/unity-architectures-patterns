using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace DependencyInjection
{
    [CustomPropertyDrawer(typeof(InjectAttribute))]
    public class InjectPropertyDrawer : PropertyDrawer
    {
        private Texture2D injectorIcon;

        Texture2D LoadIcon()
        {
            if (injectorIcon == null)
            {
                injectorIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/LightWeightDI/Icons/injector-icon.png");
            }
            return injectorIcon;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            injectorIcon = LoadIcon();
            var iconRect = new Rect(position.x, position.y, 20, 20);
            position.xMin += 24;

            if (injectorIcon != null)
            {
                var savedColor = GUI.color;
                GUI.color = property.objectReferenceValue == null ? savedColor : Color.green;
                GUI.DrawTexture(iconRect, injectorIcon);
                GUI.color = savedColor;
            }
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
#endif