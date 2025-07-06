using UnityEngine;

namespace Descensus
{
    public class FallIndicator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _fill;

        private float _fullHeight;

        private void Awake()
        {
            // Remember max sprite height
            _fullHeight = _fill.size.y;
            Hide();
        }

        public void SetAmount(float amount)
        {
            amount = Mathf.Clamp01(amount);
            
            // saving current x size and changing only y
            Vector2 newSize = _fill.size;
            newSize.y = _fullHeight * amount;
            _fill.size = newSize;
            
            // changing color (if amount = 0 - green, 1 - red)
            _fill.color = Color.Lerp(Color.green, Color.red, amount);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }

}
