using System;
using TMPro;
using UnityEngine;

namespace Descensus
{
    /// <summary>
    /// Used for move text down when button pressed
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ButtonText : MonoBehaviour
    {
        [SerializeField] private float _moveDownOffest;
        private TextMeshProUGUI _text;
        
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        public void MoveDown()
        {
            _text.rectTransform.localPosition = Vector3.down * _moveDownOffest;
        }

        public void MoveUp()
        {
            _text.rectTransform.localPosition = Vector3.zero;
        }
    }
}
