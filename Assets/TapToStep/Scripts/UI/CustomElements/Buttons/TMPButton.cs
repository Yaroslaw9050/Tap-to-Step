using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CustomElements.Buttons
{
    [ExecuteInEditMode]
    public class TMPButton : Button
    {
        [Header("Text setting:")]
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Color _normalTextColor = Color.white;
        [SerializeField] private Color _disabledTextColor = Color.gray;

        public TextMeshProUGUI Text
        {
            get => _text;
            set
            {
                _text = value;
                UpdateTextColor();
            }
        }

        public Color NormalTextColor
        {
            get => _normalTextColor;
            set
            {
                _normalTextColor = value;
                UpdateTextColor();
            }
        }

        public Color DisabledTextColor
        {
            get => _disabledTextColor;
            set
            {
                _disabledTextColor = value;
                UpdateTextColor();
            }
        }
        
        public new bool interactable
        {
            get => base.interactable;
            set
            {
                if (base.interactable != value)
                {
                    base.interactable = value;
                    UpdateTextColor();
                }
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (_text == null)
            {
                _text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            }
            UpdateTextColor();
        }

        private void UpdateTextColor()
        {
            if (_text is not null)
            {
                _text.color = interactable ? _normalTextColor : _disabledTextColor;
            }
        }
    }
}