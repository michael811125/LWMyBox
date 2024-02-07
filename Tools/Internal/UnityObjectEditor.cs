#if UNITY_EDITOR && !MYBOX_DISABLE_INSPECTOR_OVERRIDE
namespace MyBox.Internal
{
    using UnityEditor;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.MonoBehaviour), true)]
    public class UnityObjectMonoEditor : Editor
    {
        private ButtonMethodHandler _buttonMethod;
        private FoldoutAttributeHandler _foldout;

        private void OnEnable()
        {
            if (target == null) return;

            _foldout = new FoldoutAttributeHandler(target, serializedObject);
            _buttonMethod = new ButtonMethodHandler(target);
        }

        private void OnDisable()
        {
            _foldout?.OnDisable();
        }

        public override void OnInspectorGUI()
        {
            _buttonMethod?.OnBeforeInspectorGUI();

            if (_foldout != null)
            {
                _foldout.Update();
                if (!_foldout.OverrideInspector) base.OnInspectorGUI();
                else _foldout.OnInspectorGUI();
            }

            _buttonMethod?.OnAfterInspectorGUI();
        }
    }

    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.ScriptableObject), true)]
    public class UnityObjectSOEditor : Editor
    {
        private ButtonMethodHandler _buttonMethod;
        private FoldoutAttributeHandler _foldout;

        private void OnEnable()
        {
            if (target == null) return;

            _foldout = new FoldoutAttributeHandler(target, serializedObject);
            _buttonMethod = new ButtonMethodHandler(target);
        }

        private void OnDisable()
        {
            _foldout?.OnDisable();
        }

        public override void OnInspectorGUI()
        {
            _buttonMethod?.OnBeforeInspectorGUI();

            if (_foldout != null)
            {
                _foldout.Update();
                if (!_foldout.OverrideInspector) base.OnInspectorGUI();
                else _foldout.OnInspectorGUI();
            }

            _buttonMethod?.OnAfterInspectorGUI();
        }
    }
}
#endif