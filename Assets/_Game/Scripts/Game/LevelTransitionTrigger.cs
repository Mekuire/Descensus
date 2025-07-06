using LightDI;
using UnityEngine;

namespace Descensus
{
    [RequireComponent(typeof(Collider2D))]
    public class LevelTransitionTrigger : MonoBehaviour
    {        
        private GameManager _gameManager;
        
        private Collider2D _collider;
        private bool _isTriggered;
        
        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            _collider = GetComponent<Collider2D>();
            _collider.isTrigger = true;
            _isTriggered = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isTriggered) return;

            if (other.GetComponent<Player>() != null)
            {
                _isTriggered = true;
                _gameManager.MoveToNewLocation();
                
                _collider.enabled = false;
                gameObject.SetActive(false);
            }
        }
    }
}
