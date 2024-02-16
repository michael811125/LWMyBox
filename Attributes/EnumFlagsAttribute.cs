using UnityEngine;

namespace MyBox
{
    /**
     * 
     * Reference from NaughtyAttributes by dbrizov
     * https://github.com/dbrizov/NaughtyAttributes/tree/master
     * 
     **/

    public class EnumFlagsAttribute : PropertyAttribute
    {
    }
}

#if UNITY_EDITOR
namespace MyBox.Internal
{
    using System;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsPropertyDrawer : PropertyDrawerBase
    {
        protected override void OnSubGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Enum targetEnum = PropertyUtility.GetTargetObjectOfProperty(property) as Enum;
            if (targetEnum != null)
            {
                Enum enumNew = EditorGUI.EnumFlagsField(position, label.text, targetEnum);
                property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
            }
            else
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.red;
                string message = $"[{attribute.GetType().Name}] can be used only on Enums";
                GUI.Label(position, message, style);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetPropertyHeight(property);
        }

        protected float GetPropertyHeight(SerializedProperty property)
        {
            return EditorGUI.GetPropertyHeight(property, includeChildren: true);
        }
    }
}
#endif