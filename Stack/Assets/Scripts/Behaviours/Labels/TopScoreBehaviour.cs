using DefaultNamespace;
using TMPro;
using UnityEngine;

namespace Behaviours
{
    public class TopScoreBehaviour : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            GameManager.Instance.GameTopScoreChanged+=GameTopScoreChanged;
            GameTopScoreChanged(GameManager.Instance.TopScore);
        }

        private void GameTopScoreChanged(int score)
        {
            _text.text = score.ToString();
        }
    }
}