using System;
using UnityEngine;

namespace MyBox
{
    /**
     * 
     * Reference from NaughtyAttributes by dbrizov
     * https://github.com/dbrizov/NaughtyAttributes/tree/master
     * 
     **/

    public enum EInfoBoxType
    {
        Normal,
        Warning,
        Error
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class InfoBoxAttribute : PropertyAttribute
    {
        public string Text { get; private set; }
        public EInfoBoxType Type { get; private set; }

        public InfoBoxAttribute(string text, EInfoBoxType type = EInfoBoxType.Normal)
        {
            Text = text;
            Type = type;
        }
    }
}

#if UNITY_EDITOR
namespace MyBox.Internal
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(InfoBoxAttribute))]
    public class InfoBoxDecoratorDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            return GetHelpBoxHeight();
        }

        public override void OnGUI(Rect position)
        {
            InfoBoxAttribute infoBoxAttribute = (InfoBoxAttribute)attribute;

            Rect indentRect = EditorGUI.IndentedRect(position);
            float indentLength = indentRect.x - position.x;
            Rect infoBoxRect = new Rect(
                position.x + indentLength,
                position.y,
                position.width - indentLength,
                GetHelpBoxHeight());

            DrawInfoBox(infoBoxRect, infoBoxAttribute.Text, infoBoxAttribute.Type);
        }

        private float GetHelpBoxHeight()
        {
            InfoBoxAttribute infoBoxAttribute = (InfoBoxAttribute)attribute;
            float minHeight = EditorGUIUtility.singleLineHeight * 2.0f;
            float desiredHeight = GUI.skin.box.CalcHeight(new GUIContent(infoBoxAttribute.Text), EditorGUIUtility.currentViewWidth);
            float height = Mathf.Max(minHeight, desiredHeight);

            return height;
        }

        private void DrawInfoBox(Rect rect, string infoText, EInfoBoxType infoBoxType)
        {
            MessageType messageType = MessageType.None;
            switch (infoBoxType)
            {
                case EInfoBoxType.Normal:
                    messageType = MessageType.Info;
                    break;

                case EInfoBoxType.Warning:
                    messageType = MessageType.Warning;
                    break;

                case EInfoBoxType.Error:
                    messageType = MessageType.Error;
                    break;
            }

            EditorGUI.HelpBox(rect, infoText, messageType);
        }
    }
}
#endif