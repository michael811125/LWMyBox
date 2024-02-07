using UnityEngine;

namespace MyBox
{
    /**
     * 
     * Reference from NaughtyAttributes by dbrizov
     * https://github.com/dbrizov/NaughtyAttributes/tree/master
     * 
     **/

    public class CurveRangeAttribute : PropertyAttribute
    {
        private const string _DEFAULT_COLOR_HEX = "#ffffff";

        public Vector2 Min { get; private set; }
        public Vector2 Max { get; private set; }
        public string ColorHex { get; private set; }

        public CurveRangeAttribute(Vector2 min, Vector2 max, string colorHex = _DEFAULT_COLOR_HEX)
        {
            Min = min;
            Max = max;
            ColorHex = colorHex;
        }

        public CurveRangeAttribute(string colorHex)
            : this(Vector2.zero, Vector2.one, colorHex) { }

        public CurveRangeAttribute(float minX, float minY, float maxX, float maxY, string colorHex = _DEFAULT_COLOR_HEX)
            : this(new Vector2(minX, minY), new Vector2(maxX, maxY), colorHex) { }
    }
}

#if UNITY_EDITOR
namespace MyBox.Internal
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(CurveRangeAttribute))]
    public class CurveRangePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Check user error
            if (property.propertyType != SerializedPropertyType.AnimationCurve)
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.red;
                string message = $"Field [{property.name}] is not an AnimationCurve";
                GUI.Label(position, message, style);
                return;
            }

            var curveRangeAttribute = (CurveRangeAttribute)attribute;
            var curveRanges = new Rect(
                curveRangeAttribute.Min.x,
                curveRangeAttribute.Min.y,
                curveRangeAttribute.Max.x - curveRangeAttribute.Min.x,
                curveRangeAttribute.Max.y - curveRangeAttribute.Min.y);

            ColorUtility.TryParseHtmlString(curveRangeAttribute.ColorHex, out Color curveColor);
            EditorGUI.CurveField(
                position,
                property,
                curveColor,
                curveRanges,
                label);

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