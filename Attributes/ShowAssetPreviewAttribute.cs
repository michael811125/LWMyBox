using UnityEngine;

namespace MyBox
{
    /**
     * 
     * Reference from NaughtyAttributes by dbrizov
     * https://github.com/dbrizov/NaughtyAttributes/tree/master
     * 
     **/

    public class ShowAssetPreviewAttribute : PropertyAttribute
    {
        public const int DefaultWidth = 64;
        public const int DefaultHeight = 64;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public TextAnchor TextAnchor { get; private set; } = TextAnchor.MiddleRight;

        public ShowAssetPreviewAttribute(TextAnchor anchor)
        {
            Width = DefaultWidth;
            Height = DefaultHeight;
            TextAnchor = anchor;
        }

        public ShowAssetPreviewAttribute(int width = DefaultWidth, int height = DefaultHeight, TextAnchor anchor = TextAnchor.MiddleRight)
        {
            Width = width;
            Height = height;
            TextAnchor = anchor;
        }
    }
}

#if UNITY_EDITOR
namespace MyBox.Internal
{
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(ShowAssetPreviewAttribute))]
    public class ShowAssetPreviewPropertyDrawer : PropertyDrawer
    {
        private static readonly float _offsetY = 5f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                Rect propertyRect = new Rect()
                {
                    x = position.x,
                    y = position.y,
                    width = position.width,
                    height = EditorGUIUtility.singleLineHeight
                };

                EditorGUI.PropertyField(propertyRect, property, label);

                Texture2D previewTexture = GetAssetPreview(property);
                if (previewTexture != null)
                {
                    Rect previewRect = new Rect()
                    {
                        x = position.x,
                        y = position.y + EditorGUIUtility.singleLineHeight + _offsetY,
                        width = position.width,
                        height = GetAssetPreviewSize(property).y
                    };

                    var centeredStyle = GUI.skin.GetStyle("Label");
                    centeredStyle.alignment = (attribute as ShowAssetPreviewAttribute).TextAnchor;
                    GUI.Label(previewRect, previewTexture, centeredStyle);
                }
            }
            else
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.red;
                string message = $"Field [{property.name}] doesn't have an asset preview";
                GUI.Label(position, message, style);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                Texture2D previewTexture = GetAssetPreview(property);

                if (previewTexture != null)
                {
                    return this.GetPropertyHeight(property) + GetAssetPreviewSize(property).y + _offsetY;
                }
                else
                {
                    return GetPropertyHeight(property);
                }
            }

            return GetPropertyHeight(property);
        }

        protected float GetPropertyHeight(SerializedProperty property)
        {
            return EditorGUI.GetPropertyHeight(property, includeChildren: true);
        }

        private Texture2D GetAssetPreview(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue != null)
                {
                    Texture2D previewTexture = AssetPreview.GetAssetPreview(property.objectReferenceValue);
                    return previewTexture;
                }

                return null;
            }

            return null;
        }

        private Vector2 GetAssetPreviewSize(SerializedProperty property)
        {
            Texture2D previewTexture = GetAssetPreview(property);
            if (previewTexture == null)
            {
                return Vector2.zero;
            }
            else
            {
                int targetWidth = ShowAssetPreviewAttribute.DefaultWidth;
                int targetHeight = ShowAssetPreviewAttribute.DefaultHeight;

                ShowAssetPreviewAttribute showAssetPreviewAttribute = attribute as ShowAssetPreviewAttribute;
                if (showAssetPreviewAttribute != null)
                {
                    targetWidth = showAssetPreviewAttribute.Width;
                    targetHeight = showAssetPreviewAttribute.Height;
                }

                int width = Mathf.Clamp(targetWidth, 0, previewTexture.width);
                int height = Mathf.Clamp(targetHeight, 0, previewTexture.height);

                return new Vector2(width, height);
            }
        }
    }
}
#endif