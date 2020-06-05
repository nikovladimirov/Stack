using DefaultNamespace;
using DefaultNamespace.Enums;
using UnityEngine;

namespace Behaviours
{
    public class GameUIBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.Instance.GameStateChanged += GameStateChanged;
            GameStateChanged(GameState.Menu);
        }

        private void GameStateChanged(GameState state)
        {
            if (state != GameState.Playing)
            {
                gameObject.SetActive(false);
                return;
            }
            
            gameObject.SetActive(true);
        }
    }
}