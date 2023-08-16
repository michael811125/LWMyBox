using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyBox
{
	public static class MyCollections
	{
        #region InsertAt and RemoveAt

        /// <summary>
        /// Returns new array with inserted empty element at index
        /// </summary>
        public static T[] InsertAt<T>(this T[] array, int index)
        {
            if (index < 0)
            {
                Debug.LogError("Index is less than zero. Array is not modified");
                return array;
            }

            if (index > array.Length)
            {
                Debug.LogError("Index exceeds array length. Array is not modified");
                return array;
            }

            T[] newArray = new T[array.Length + 1];
            int index1 = 0;
            for (int index2 = 0; index2 < newArray.Length; ++index2)
            {
                if (index2 == index) continue;

                newArray[index2] = array[index1];
                ++index1;
            }

            return newArray;
        }

        /// <summary>
        /// Returns new array without element at index
        /// </summary>
        public static T[] RemoveAt<T>(this T[] array, int index)
        {
            if (index < 0)
            {
                Debug.LogError("Index is less than zero. Array is not modified");
                return array;
            }

            if (index >= array.Length)
            {
                Debug.LogError("Index exceeds array length. Array is not modified");
                return array;
            }

            T[] newArray = new T[array.Length - 1];
            int index1 = 0;
            for (int index2 = 0; index2 < array.Length; ++index2)
            {
                if (index2 == index) continue;

                newArray[index1] = array[index2];
                ++index1;
            }

            return newArray;
        }

        #endregion

        #region IsNullOrEmpty and NotNullOrEmpty

        /// <summary>
        /// Is array null or empty
        /// </summary>
        public static bool IsNullOrEmpty<T>(this T[] collection) => collection == null || collection.Length == 0;

		/// <summary>
		/// Is list null or empty
		/// </summary>
		public static bool IsNullOrEmpty<T>(this IList<T> collection) => collection == null || collection.Count == 0;

		/// <summary>
		/// Is collection null or empty. IEnumerable is relatively slow. Use Array or List implementation if possible
		/// </summary>
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) => collection == null || !collection.Any();

		/// <summary>
		/// Collection is not null or empty
		/// </summary>
		public static bool NotNullOrEmpty<T>(this T[] collection) => !collection.IsNullOrEmpty();
		
		/// <summary>
		/// Collection is not null or empty
		/// </summary>
		public static bool NotNullOrEmpty<T>(this IList<T> collection) => !collection.IsNullOrEmpty();
		
		/// <summary>
		/// Collection is not null or empty
		/// </summary>
		public static bool NotNullOrEmpty<T>(this IEnumerable<T> collection) => !collection.IsNullOrEmpty();
		
		#endregion
	}
}