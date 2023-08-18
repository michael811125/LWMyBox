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
        public string ButtonName { get; }
        public string MethodName { get; }

        public ButtonClickerAttribute(string methodName)
        {
            MethodName = methodName;
        }

        public ButtonClickerAttribute(string methodName, string buttonName)
        {
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
            string methodName = (attribute as ButtonClickerAttribute).MethodName;
            string buttonName = (attribute as ButtonClickerAttribute).ButtonName;
            Object target = property.serializedObject.targetObject;
            System.Type type = target.GetType();
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

            EditorGUILayout.Space(0.5f);
            if (GUI.Button(new Rect(position.x, position.y, position.width, position.height + 2.5f), !string.IsNullOrEmpty(buttonName) ? buttonName : method.Name))
            {
                method.Invoke(target, null);
            }
            EditorGUILayout.Space(0.5f);
        }
    }
}
#endif