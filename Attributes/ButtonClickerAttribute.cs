using UnityEngine;

namespace MyBox
{
    /**
     * 
     * Reference from Unity Community by joshrs926
     * https://forum.unity.com/threads/attribute-to-add-button-to-class.660262/ 
     * 
     **/

    public class ButtonClickerAttribute : PropertyAttribute
    {
        public System.Type TargetType { get; }
        public string TargetTypeInstanceValueName { get; }
        public string MethodName { get; }
        public string ButtonName { get; }

        public ButtonClickerAttribute(string methodName)
        {
            MethodName = methodName;
        }

        public ButtonClickerAttribute(System.Type targetType, string targetTypeInstanceValueName, string methodName)
        {
            TargetType = targetType;
            TargetTypeInstanceValueName = targetTypeInstanceValueName;
            MethodName = methodName;
        }

        public ButtonClickerAttribute(string methodName, string buttonName)
        {
            MethodName = methodName;
            ButtonName = buttonName;
        }

        public ButtonClickerAttribute(System.Type targetType, string targetTypeInstanceValueName, string methodName, string buttonName)
        {
            TargetType = targetType;
            TargetTypeInstanceValueName = targetTypeInstanceValueName;
            MethodName = methodName;
            ButtonName = buttonName;
        }
    }
}

#if UNITY_EDITOR
namespace MyBox.Internal
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(ButtonClickerAttribute))]
    public class ButtonClickerAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            System.Type targetType = (attribute as ButtonClickerAttribute).TargetType;
            string targetTypeInstanceValueName = (attribute as ButtonClickerAttribute).TargetTypeInstanceValueName;
            string methodName = (attribute as ButtonClickerAttribute).MethodName;
            string buttonName = (attribute as ButtonClickerAttribute).ButtonName;
            Object target = property.serializedObject.targetObject;
            System.Type type = targetType != null ? targetType : target.GetType();
            System.Reflection.MethodInfo method = type.GetMethod(methodName);

            if (method == null)
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.red;
                GUI.Label(position, $"Button method could not be found. Is it public?", style);
                return;
            }
            else if (method.GetParameters().Length > 0)
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.red;
                GUI.Label(position, "Button method cannot have parameters.", style);
                return;
            }

            if (GUI.Button(new Rect(position.x, position.y, position.width, position.height + 2.5f), !string.IsNullOrEmpty(buttonName) ? buttonName : method.Name))
            {
                if (targetType != null) method.Invoke(target.GetType().GetField(targetTypeInstanceValueName).GetValue(target), null);
                else method.Invoke(target, null);
            }
            if (!(target is ScriptableObject)) EditorGUILayout.Space(0.5f);
        }
    }
}
#endif