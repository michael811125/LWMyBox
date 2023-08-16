#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

namespace MyBox.EditorTools
{
	public static class MyGUI
	{
		#region Colors

		public static class Colors
		{
			public static readonly Color Red = new Color(.8f, .6f, .6f);

			public static readonly Color Green = new Color(.4f, .6f, .4f);

			public static readonly Color Blue = new Color(.6f, .6f, .8f);

			public static readonly Color Gray = new Color(.3f, .3f, .3f);

			public static readonly Color Yellow = new Color(.8f, .8f, .2f, .6f);

			public static readonly Color Brown = new Color(.7f, .5f, .2f, .6f);
		}

        #endregion

        #region Editor Icons

        public static class EditorIcons
        {
            public static GUIContent Plus => EditorGUIUtility.IconContent("Toolbar Plus");
            public static GUIContent Minus => EditorGUIUtility.IconContent("Toolbar Minus");
            public static GUIContent Refresh => EditorGUIUtility.IconContent("Refresh");

            public static GUIContent ConsoleInfo => EditorGUIUtility.IconContent("console.infoicon.sml");
            public static GUIContent ConsoleWarning => EditorGUIUtility.IconContent("console.warnicon.sml");
            public static GUIContent ConsoleError => EditorGUIUtility.IconContent("console.erroricon.sml");

            public static GUIContent Check => EditorGUIUtility.IconContent("FilterSelectedOnly");

            public static GUIContent Dropdown => EditorGUIUtility.IconContent("icon dropdown");

            public static GUIContent EyeOn => EditorGUIUtility.IconContent("d_VisibilityOn");
            public static GUIContent EyeOff => EditorGUIUtility.IconContent("d_VisibilityOff");
            public static GUIContent Zoom => EditorGUIUtility.IconContent("d_ViewToolZoom");

            public static GUIContent Help => EditorGUIUtility.IconContent("_Help");
            public static GUIContent Favourite => EditorGUIUtility.IconContent("Favorite");
            public static GUIContent Label => EditorGUIUtility.IconContent("FilterByLabel");

            public static GUIContent Settings => EditorGUIUtility.IconContent("d_Settings");
            public static GUIContent SettingsPopup => EditorGUIUtility.IconContent("_Popup");
            public static GUIContent SettingsMixer => EditorGUIUtility.IconContent("Audio Mixer");

            public static GUIContent Circle => EditorGUIUtility.IconContent("TestNormal");
            public static GUIContent CircleYellow => EditorGUIUtility.IconContent("TestInconclusive");
            public static GUIContent CircleDotted => EditorGUIUtility.IconContent("TestIgnored");
            public static GUIContent CircleRed => EditorGUIUtility.IconContent("TestFailed");
        }

        #endregion

        #region Draw Coloured lines and boxes

        /// <summary>
        /// Draw Separator within GuiLayout
        /// </summary>
        public static void Separator()
		{
			var color = GUI.color;

			EditorGUILayout.Space();
			var spaceRect = EditorGUILayout.GetControlRect();
			var separatorRectPosition = new Vector2(spaceRect.position.x, spaceRect.position.y + spaceRect.height / 2);
			var separatorRect = new Rect(separatorRectPosition, new Vector2(spaceRect.width, 1));

			GUI.color = Color.white;
			GUI.Box(separatorRect, GUIContent.none);
			GUI.color = color;
		}

		/// <summary>
		/// Draw Line within GUILayout
		/// </summary>
		public static void DrawLine(Color color, bool withSpace = false)
		{
			if (withSpace) EditorGUILayout.Space();

			var defaultBackgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
			GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
			GUI.backgroundColor = defaultBackgroundColor;

			if (withSpace) EditorGUILayout.Space();
		}

		/// <summary>
		/// Draw line within Rect and get Rect back with offset
		/// </summary>
		public static Rect DrawLine(Color color, Rect rect)
		{
			var h = rect.height;
			var defaultBackgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
			rect.y += 5;
			rect.height = 1;
			GUI.Box(rect, "");
			rect.y += 5;
			GUI.backgroundColor = defaultBackgroundColor;
			rect.height = h;
			return rect;
		}

		/// <summary>
		/// Draw Rect filled with Color
		/// </summary>
		public static void DrawColouredRect(Rect rect, Color color)
		{
			var defaultBackgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
			GUI.Box(rect, "");
			GUI.backgroundColor = defaultBackgroundColor;
		}

		/// <summary>
		/// Draw background Line within GUILayout
		/// </summary>
		public static void DrawBackgroundLine(Color color, int yOffset = 0)
		{
			var defColor = GUI.color;
			GUI.color = color;
			var rect = GUILayoutUtility.GetLastRect();
			rect.center = new Vector2(rect.center.x, rect.center.y + 6 + yOffset);
			rect.height = 17;
			GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);
			GUI.color = defColor;
		}

		/// <summary>
		/// Draw background Line of height
		/// </summary>
		public static void DrawBackgroundBox(Color color, int height, int yOffset = 0)
		{
			var defColor = GUI.color;
			GUI.color = color;
			var rect = GUILayoutUtility.GetLastRect();
			rect.center = new Vector2(rect.center.x, rect.center.y + 6 + yOffset);
			rect.height = height;
			GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);
			GUI.color = defColor;
		}

        #endregion

        #region SearchablePopup

        /// <summary>
        /// A popup window that displays a list of options and may use a search string to filter the displayed content
        /// </summary>
        /// <param name="activatorRect">Rectangle of the button that triggered the popup</param>
        /// <param name="options">List of strings to choose from</param>
        /// <param name="current">Index of the currently selected string</param>
        /// <param name="onSelectionMade">Callback to trigger when a choice is made</param>
        public static void SearchablePopup(Rect activatorRect, string[] options, int current, Action<int> onSelectionMade)
        {
            Internal.SearchablePopup.Show(activatorRect, options, current, onSelectionMade);
        }

        /// <summary>
        /// A popup window that displays a list of options and may use a search string to filter the displayed content
        /// </summary>
        /// <param name="options">List of strings to choose from</param>
        /// <param name="current">Index of the currently selected string</param>
        /// <param name="onSelectionMade">Callback to trigger when a choice is made</param>
        public static void SearchablePopup(string[] options, int current, Action<int> onSelectionMade)
        {
            var position = new Rect(Event.current.mousePosition, Vector2.zero);
            Internal.SearchablePopup.Show(position, options, current, onSelectionMade);
        }

        #endregion
    }
}
#endif