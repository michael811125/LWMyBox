using UnityEngine;

namespace MyBox
{
    /**
     * 
     * Reference from NaughtyAttributes by dbrizov
     * https://github.com/dbrizov/NaughtyAttributes/tree/master
     * 
     **/

    public class ProgressBarAttribute : PropertyAttribute
    {
        private const string _DEFAULT_COLOR_HEX = "#00ffdd";

        public string Name { get; private set; }
        public float MaxValue { get; set; }
        public string ColorHex { get; }

        public ProgressBarAttribute(string name, float maxValue, string colorHex = _DEFAULT_COLOR_HEX)
        {
            Name = name;
            MaxValue = maxValue;
            ColorHex = colorHex;
        }
    }
}

#if UNITY_EDITOR
namespace MyBox.Internal
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (!IsNumber(property))
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.red;
                string message = $"Field [{property.name}] is not a number";
                GUI.Label(position, message, style);
                return;
            }

            ProgressBarAttribute progressBarAttribute = attribute as ProgressBarAttribute;
            var value = property.propertyType == SerializedPropertyType.Integer ? property.intValue : property.floatValue;
            var valueFormatted = property.propertyType == SerializedPropertyType.Integer ? value.ToString() : string.Format("{0:0.00}", value);
            var maxValue = progressBarAttribute.MaxValue;

            var fillPercentage = value / CastToFloat(maxValue);
            var barLabel = (!string.IsNullOrEmpty(progressBarAttribute.Name) ? "[" + progressBarAttribute.Name + "] " : "") + valueFormatted + "/" + maxValue;
            ColorUtility.TryParseHtmlString(progressBarAttribute.ColorHex, out Color barColor);
            var labelColor = Color.white;

            Rect barRect = new Rect()
            {
                x = position.x,
                y = position.y,
                width = position.width,
                height = EditorGUIUtility.singleLineHeight
            };

            DrawBar(barRect, Mathf.Clamp01(fillPercentage), barLabel, barColor, labelColor);

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

        private void DrawBar(Rect rect, float fillPercent, string label, Color barColor, Color labelColor)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            var fillRect = new Rect(rect.x, rect.y, rect.width * fillPercent, rect.height);

            EditorGUI.DrawRect(rect, new Color(0.13f, 0.13f, 0.13f));
            EditorGUI.DrawRect(fillRect, barColor);

            // set alignment and cache the default
            var align = GUI.skin.label.alignment;
            GUI.skin.label.alignment = TextAnchor.UpperCenter;

            // set the color and cache the default
            var c = GUI.contentColor;
            GUI.contentColor = labelColor;

            // calculate the position
            var labelRect = new Rect(rect.x, rect.y - 2, rect.width, rect.height);

            // draw~
            EditorGUI.DropShadowLabel(labelRect, label);

            // reset color and alignment
            GUI.contentColor = c;
            GUI.skin.label.alignment = align;
        }

        private bool IsNumber(SerializedProperty property)
        {
            bool isNumber = property.propertyType == SerializedPropertyType.Float || property.propertyType == SerializedPropertyType.Integer;
            return isNumber;
        }

        private float CastToFloat(object obj)
        {
            if (obj is int)
            {
                return (int)obj;
            }
            else
            {
                return (float)obj;
            }
        }
    }
}
#endif