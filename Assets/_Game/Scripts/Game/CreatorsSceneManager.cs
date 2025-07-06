using System.Collections;
using AnnulusGames.SceneSystem;
using UnityEngine;

namespace Descensus
{
    public class CreatorsSceneManager : MonoBehaviour
    {
        [SerializeField] private float _delay = 0.5f;
        
        // This method is called after all text has stopped moving
        public void ReturnToMainMenu()
        {
            StartCoroutine(nameof(Return));
        }

        private IEnumerator Return()
        {
            yield return new WaitForSeconds(_delay);
            Scenes.LoadScene("Menu");
        }
    }
}
