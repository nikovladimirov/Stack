using DefaultNamespace;
using TMPro;
using UnityEngine;

namespace Behaviours
{
    public class ScoreBehaviour : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            GameManager.Instance.GameScoreChanged+=GameScoreChanged;
        }

        private void GameScoreChanged(int score)
        {
            _text.text = score.ToString();
        }
    }
}