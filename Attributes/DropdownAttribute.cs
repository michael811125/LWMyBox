using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyBox
{
    /**
     * 
     * Reference from NaughtyAttributes by dbrizov
     * https://github.com/dbrizov/NaughtyAttributes/tree/master
     * 
     **/

    public class DropdownAttribute : PropertyAttribute
    {
        public string ValuesName { get; private set; }

        public DropdownAttribute(string valuesName)
        {
            ValuesName = valuesName;
        }
    }

    public interface IDropdownList : IEnumerable<KeyValuePair<string, object>>
    {
    }

    public class DropdownList<T> : IDropdownList
    {
        private List<KeyValuePair<string, object>> _values;

        public DropdownList()
        {
            _values = new List<KeyValuePair<string, object>>();
        }

        public void Add(string displayName, T value)
        {
            _values.Add(new KeyValuePair<string, object>(displayName, value));
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static explicit operator DropdownList<object>(DropdownList<T> target)
        {
            DropdownList<object> result = new DropdownList<object>();
            foreach (var kvp in target)
            {
                result.Add(kvp.Key, kvp.Value);
            }

            return result;
        }
    }
}

#if UNITY_EDITOR
namespace MyBox.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownPropertyDrawer : PropertyDrawerBase
    {
        protected override void OnSubGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            DropdownAttribute dropdownAttribute = (DropdownAttribute)attribute;
            object target = PropertyUtility.GetTargetObjectWithProperty(property);

            object valuesObject = GetValues(property, dropdownAttribute.ValuesName);
            FieldInfo dropdownField = ReflectionUtility.GetField(target, property.name);

            if (AreValuesValid(valuesObject, dropdownField))
            {
                if (valuesObject is IList && dropdownField.FieldType == GetElementType(valuesObject))
                {
                    // Selected value
                    object selectedValue = dropdownField.GetValue(target);

                    // Values and display options
                    IList valuesList = (IList)valuesObject;
                    object[] values = new object[valuesList.Count];
                    string[] displayOptions = new string[valuesList.Count];

                    for (int i = 0; i < values.Length; i++)
                    {
                        object value = valuesList[i];
                        values[i] = value;
                        displayOptions[i] = value == null ? "<null>" : value.ToString();
                    }

                    // Selected value index
                    int selectedValueIndex = Array.IndexOf(values, selectedValue);
                    if (selectedValueIndex < 0)
                    {
                        selectedValueIndex = 0;
                    }

                    Dropdown(position, property.serializedObject, target, dropdownField, label.text, selectedValueIndex, values, displayOptions);
                }
                else if (valuesObject is IDropdownList)
                {
                    // Current value
                    object selectedValue = dropdownField.GetValue(target);

                    // Current value index, values and display options
                    int index = -1;
                    int selectedValueIndex = -1;
                    List<object> values = new List<object>();
                    List<string> displayOptions = new List<string>();
                    IDropdownList dropdown = (IDropdownList)valuesObject;

                    using (IEnumerator<KeyValuePair<string, object>> dropdownEnumerator = dropdown.GetEnumerator())
                    {
                        while (dropdownEnumerator.MoveNext())
                        {
                            index++;

                            KeyValuePair<string, object> current = dropdownEnumerator.Current;
                            if (current.Value?.Equals(selectedValue) == true)
                            {
                                selectedValueIndex = index;
                            }

                            values.Add(current.Value);

                            if (current.Key == null)
                            {
                                displayOptions.Add("<null>");
                            }
                            else if (string.IsNullOrWhiteSpace(current.Key))
                            {
                                displayOptions.Add("<empty>");
                            }
                            else
                            {
                                displayOptions.Add(current.Key);
                            }
                        }
                    }

                    if (selectedValueIndex < 0)
                    {
                        selectedValueIndex = 0;
                    }

                    Dropdown(position, property.serializedObject, target, dropdownField, label.text, selectedValueIndex, values.ToArray(), displayOptions.ToArray());
                }
            }
            else
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.red;
                string message = $"Invalid values with name '{dropdownAttribute.ValuesName}' provided to '{dropdownAttribute.GetType().Name}'. Either the values name is incorrect or the types of the target field and the values field/property/method don't match";
                GUI.Label(position, message, style);
            }

            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Creates a dropdown
        /// </summary>
        /// <param name="rect">The rect the defines the position and size of the dropdown in the inspector</param>
        /// <param name="serializedObject">The serialized object that is being updated</param>
        /// <param name="target">The target object that contains the dropdown</param>
        /// <param name="dropdownField">The field of the target object that holds the currently selected dropdown value</param>
        /// <param name="label">The label of the dropdown</param>
        /// <param name="selectedValueIndex">The index of the value from the values array</param>
        /// <param name="values">The values of the dropdown</param>
        /// <param name="displayOptions">The display options for the values</param>
        public void Dropdown(
            Rect rect, SerializedObject serializedObject, object target, FieldInfo dropdownField,
            string label, int selectedValueIndex, object[] values, string[] displayOptions)
        {
            EditorGUI.BeginChangeCheck();

            int newIndex = EditorGUI.Popup(rect, label, selectedValueIndex, displayOptions);
            object newValue = values[newIndex];

            object dropdownValue = dropdownField.GetValue(target);
            if (dropdownValue == null || !dropdownValue.Equals(newValue))
            {
                Undo.RecordObject(serializedObject.targetObject, "Dropdown");

                // TODO: Problem with structs, because they are value type.
                // The solution is to make boxing/unboxing but unfortunately I don't know the compile time type of the target object
                dropdownField.SetValue(target, newValue);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GetPropertyHeight(property);
        }

        protected float GetPropertyHeight(SerializedProperty property)
        {
            return EditorGUI.GetPropertyHeight(property, includeChildren: true);
        }

        private object GetValues(SerializedProperty property, string valuesName)
        {
            object target = PropertyUtility.GetTargetObjectWithProperty(property);

            FieldInfo valuesFieldInfo = ReflectionUtility.GetField(target, valuesName);
            if (valuesFieldInfo != null)
            {
                return valuesFieldInfo.GetValue(target);
            }

            PropertyInfo valuesPropertyInfo = ReflectionUtility.GetProperty(target, valuesName);
            if (valuesPropertyInfo != null)
            {
                return valuesPropertyInfo.GetValue(target);
            }

            MethodInfo methodValuesInfo = ReflectionUtility.GetMethod(target, valuesName);
            if (methodValuesInfo != null &&
                methodValuesInfo.ReturnType != typeof(void) &&
                methodValuesInfo.GetParameters().Length == 0)
            {
                return methodValuesInfo.Invoke(target, null);
            }

            return null;
        }

        private bool AreValuesValid(object values, FieldInfo dropdownField)
        {
            if (values == null || dropdownField == null)
            {
                return false;
            }

            if ((values is IList && dropdownField.FieldType == GetElementType(values)) ||
                (values is IDropdownList))
            {
                return true;
            }

            return false;
        }

        private Type GetElementType(object values)
        {
            Type valuesType = values.GetType();
            Type elementType = ReflectionUtility.GetListElementType(valuesType);

            return elementType;
        }
    }
}
#endif