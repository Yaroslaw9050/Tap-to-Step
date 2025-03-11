using UnityEditor;
using UnityEngine;

namespace UI.CustomElements.Buttons
{
    [CustomEditor(typeof(TMPButton))]
    public class TMPButtonEditor : UnityEditor.UI.ButtonEditor
    {
        private SerializedProperty _textMeshProUGUIText;
        private SerializedProperty _normalTextColor;
        private SerializedProperty _disabledTextColor;

        protected override void OnEnable()
        {
            base.OnEnable();
            _textMeshProUGUIText = serializedObject.FindProperty("_text");
            _normalTextColor = serializedObject.FindProperty("_normalTextColor");
            _disabledTextColor = serializedObject.FindProperty("_disabledTextColor");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();
            
            EditorGUILayout.PropertyField(_textMeshProUGUIText, new GUIContent("Text"));
            EditorGUILayout.PropertyField(_normalTextColor, new GUIContent("Text normal color"));
            EditorGUILayout.PropertyField(_disabledTextColor, new GUIContent("Text disabled color"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}