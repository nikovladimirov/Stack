using DefaultNamespace.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ChangeStateButtonBehaviour : MonoBehaviour
    {
        private Button _button;

        [SerializeField] private GameState _newState;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ChangeGameState);
        }

        private void ChangeGameState()
        {
            GameManager.Instance.SetGameState(_newState);
        }
    }
}