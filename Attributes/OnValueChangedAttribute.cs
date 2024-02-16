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

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class OnValueChangedAttribute : PropertyAttribute
    {
        public string CallbackName { get; private set; }

        public OnValueChangedAttribute(string callbackName)
        {
            CallbackName = callbackName;
        }
    }
}