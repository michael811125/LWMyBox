using UnityEngine;

namespace MyBox
{
    /**
     * 
     * Reference from NaughtyAttributes by dbrizov
     * https://github.com/dbrizov/NaughtyAttributes/tree/master
     * 
     **/

    public class ResizableTextAreaAttribute : PropertyAttribute
    {
    }
}

#if UNITY_EDITOR
namespace MyBox.Internal
{
    using System;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(ResizableTextAreaAttribute))]
    public class ResizableTextAreaPropertyDrawer : PropertyDrawer
    {
        private Vector2 _scrollPos;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetPropertyHeight(property);
        }

        protected float GetPropertyHeight(SerializedProperty property)
        {
            return EditorGUI.GetPropertyHeight(property, includeChildren: true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType == SerializedPropertyType.String)
            {
                Rect labelRect = new Rect()
                {
                    x = position.x,
                    y = position.y,
                    width = position.width,
                    height = EditorGUIUtility.singleLineHeight
                };

                EditorGUI.LabelField(labelRect, label.text);

                this._scrollPos = EditorGUILayout.BeginScrollView(this._scrollPos, false, false);

                EditorGUI.BeginChangeCheck();

                Rect textAreaRect = new Rect()
                {
                    x = labelRect.x,
                    y = labelRect.y + EditorGUIUtility.singleLineHeight,
                    width = labelRect.width,
                    height = GetTextAreaHeight(property.stringValue)
                };

                string textAreaValue = EditorGUILayout.TextArea(property.stringValue, GUILayout.Height(textAreaRect.height));

                if (EditorGUI.EndChangeCheck())
                {
                    property.stringValue = textAreaValue;
                }

                EditorGUILayout.EndScrollView();
            }
            else
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.red;
                string message = $"{typeof(ResizableTextAreaAttribute).Name} can only be used on string fields";
                GUI.Label(position, message, style);
            }

            EditorGUI.EndProperty();
        }

        private int GetNumberOfLines(string text)
        {
            string content = Regex.Replace(text, @"\r\n|\n\r|\r|\n", Environment.NewLine);
            string[] lines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            return lines.Length;
        }

        private float GetTextAreaHeight(string text)
        {
            float height = (EditorGUIUtility.singleLineHeight - 3.0f) * GetNumberOfLines(text) + 10f;
            return height;
        }
    }
}
#endif