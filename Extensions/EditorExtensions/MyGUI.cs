#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

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
	}
}
#endif