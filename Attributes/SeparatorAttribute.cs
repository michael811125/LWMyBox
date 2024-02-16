using UnityEngine;

namespace MyBox
{
    public class SeparatorAttribute : PropertyAttribute
    {
        public readonly string Title;
        public readonly bool WithOffset;
        public readonly float Thickness;
        public readonly string ColorHex;

        public SeparatorAttribute()
        {
            Title = string.Empty;
            Thickness = 1f;
        }

        public SeparatorAttribute(float thickness, string colorHex = null, bool withOffset = false)
        {
            Title = string.Empty;
            WithOffset = withOffset;
            Thickness = thickness;
            ColorHex = colorHex;
        }

        public SeparatorAttribute(string title, string colorHex = null, float thickness = 1f, bool withOffset = false)
        {
            Title = title;
            WithOffset = withOffset;
            Thickness = thickness;
            ColorHex = colorHex;
        }
    }
}

#if UNITY_EDITOR
namespace MyBox.Internal
{
    using UnityEditor;

    [CustomPropertyDrawer(typeof(SeparatorAttribute))]
    public class SeparatorAttributeDrawer : DecoratorDrawer
    {
        private SeparatorAttribute Separator => (SeparatorAttribute)attribute;

        public override float GetHeight() => Separator.WithOffset ? 40 : Separator.Title.IsNullOrEmpty() ? 28 : 32;

        public override void OnGUI(Rect position)
        {
            ColorUtility.TryParseHtmlString(string.IsNullOrEmpty(Separator.ColorHex) ? "#282828" : Separator.ColorHex, out Color color);
            var title = Separator.Title;
            if (title.IsNullOrEmpty())
            {
                position.height = Separator.Thickness;
                position.y += 14;
                var t2d = MakeT2d(1, 1, color);
                GUI.DrawTexture(position, t2d);
            }
            else
            {
                Vector2 textSize = GUI.skin.label.CalcSize(new GUIContent(title));
                float separatorWidth = (position.width - textSize.x) / 2 - 5;
                float thickness = Separator.Thickness;
                position.y += 19;
                var t2d = MakeT2d(1, 1, color);
                GUI.DrawTexture(new Rect(position.xMin, position.yMin, separatorWidth, thickness), t2d);
                GUI.Label(new Rect(position.xMin + separatorWidth + 5, position.yMin - 10, textSize.x, 20), title);
                GUI.DrawTexture(new Rect(position.xMin + separatorWidth + 10 + textSize.x, position.yMin, separatorWidth, thickness), t2d);
            }
        }

        private Texture2D MakeT2d(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; ++i)
            {
                pixels[i] = color;
            }
            Texture2D t2d = new Texture2D(width, height);
            t2d.SetPixels(pixels);
            t2d.Apply();
            return t2d;
        }
    }
}
#endif