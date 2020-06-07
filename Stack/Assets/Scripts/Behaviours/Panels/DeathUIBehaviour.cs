using DefaultNamespace;
using DefaultNamespace.Enums;
using UnityEngine;

namespace Behaviours
{
    public class DeathUIBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.Instance.GameStateChanged += GameStateChanged;
            GameStateChanged(GameState.Menu);
        }

        private void GameStateChanged(GameState state)
        {
            if (state != GameState.Death)
            {
                gameObject.SetActive(false);
                return;
            }
            
            gameObject.SetActive(true);
        }
        
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                GameManager.Instance.SetGameState(GameState.Menu);
        }
    }
}