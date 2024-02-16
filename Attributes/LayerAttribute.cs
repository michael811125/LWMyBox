using UnityEngine;

namespace MyBox
{
    public class LayerAttribute : PropertyAttribute
    {
    }
}

#if UNITY_EDITOR
namespace MyBox.Internal
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(LayerAttribute))]
    public class LayerAttributeDrawer : PropertyDrawerBase
    {
        private bool _checked;

        protected override void OnSubGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                if (!_checked) Warning(property);
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            property.intValue = EditorGUI.LayerField(position, label, property.intValue);
        }

        private void Warning(SerializedProperty property)
        {
            Debug.LogWarning(string.Format("Property <color=brown>{0}</color> in object <color=brown>{1}</color> is of wrong type. Expected: Int",
                property.name, property.serializedObject.targetObject));
            _checked = true;
        }
    }
}
#endif