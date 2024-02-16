#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MyBox.Internal
{
    public abstract class PropertyDrawerBase : PropertyDrawer
    {
        public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check OnValueChanged
            EditorGUI.BeginChangeCheck();

            // Draw views
            this.OnSubGUI(position, property, label);

            // Call OnValueChanged callbacks
            if (EditorGUI.EndChangeCheck())
            {
                PropertyUtility.CallOnValueChangedCallbacks(property);
            }
        }

        protected abstract void OnSubGUI(Rect position, SerializedProperty property, GUIContent label);
    }
}
#endif