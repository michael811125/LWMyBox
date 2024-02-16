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
        public string MaxValueName { get; private set; }
        public string ColorHex { get; }

        public ProgressBarAttribute(string name, float maxValue, string colorHex = _DEFAULT_COLOR_HEX)
        {
            Name = name;
            MaxValue = maxValue;
            ColorHex = colorHex;
        }

        public ProgressBarAttribute(string name, string maxValueName, string colorHex = _DEFAULT_COLOR_HEX)
        {
            Name = name;
            MaxValueName = maxValueName;
            ColorHex = colorHex;
        }
    }
}

#if UNITY_EDITOR
namespace MyBox.Internal
{
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarPropertyDrawer : PropertyDrawerBase
    {
        protected override void OnSubGUI(Rect position, SerializedProperty property, GUIContent label)
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
            var maxValue = GetMaxValue(property, progressBarAttribute);

            if (maxValue != null && IsNumber(maxValue))
            {
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
            }
            else
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.red;
                string message = $"The provided dynamic max value for the progress bar is not correct. Please check if the '{nameof(progressBarAttribute.MaxValueName)}' is correct, or the return type is float/int";
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

        private object GetMaxValue(SerializedProperty property, ProgressBarAttribute progressBarAttribute)
        {
            if (string.IsNullOrEmpty(progressBarAttribute.MaxValueName))
            {
                return progressBarAttribute.MaxValue;
            }
            else
            {
                object target = PropertyUtility.GetTargetObjectWithProperty(property);

                FieldInfo valuesFieldInfo = ReflectionUtility.GetField(target, progressBarAttribute.MaxValueName);
                if (valuesFieldInfo != null)
                {
                    return valuesFieldInfo.GetValue(target);
                }

                PropertyInfo valuesPropertyInfo = ReflectionUtility.GetProperty(target, progressBarAttribute.MaxValueName);
                if (valuesPropertyInfo != null)
                {
                    return valuesPropertyInfo.GetValue(target);
                }

                MethodInfo methodValuesInfo = ReflectionUtility.GetMethod(target, progressBarAttribute.MaxValueName);
                if (methodValuesInfo != null &&
                    (methodValuesInfo.ReturnType == typeof(float) || methodValuesInfo.ReturnType == typeof(int)) &&
                    methodValuesInfo.GetParameters().Length == 0)
                {
                    return methodValuesInfo.Invoke(target, null);
                }

                return null;
            }
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

        private bool IsNumber(object obj)
        {
            return (obj is float) || (obj is int);
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