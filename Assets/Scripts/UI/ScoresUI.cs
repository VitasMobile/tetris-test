using TestTetris.GameData;
using UnityEngine;
using UnityEngine.UI;

namespace TestTetris.UI
{
    public class ScoresUI : MonoBehaviour
    {
        [SerializeField] private Text _topScoreText;
        [SerializeField] private Text _scoreText;

        private void Awake()
        {
            Score.ChangeScoreEvent += OnChangedScore;
            Score.ChangeTopScoreEvent += OnChangedTopScore;
        }

        private void OnDestroy()
        {
            Score.ChangeScoreEvent -= OnChangedScore;
            Score.ChangeTopScoreEvent -= OnChangedTopScore;
        }

        private void OnChangedScore(int score)
        {
            _scoreText.text = $"SCORE\n{score.ToString().PadLeft(6, '0')}";
        }

        private void OnChangedTopScore(int topScore)
        {
            _topScoreText.text = $"TOP\n{topScore.ToString().PadLeft(6, '0')}";
        }
    }
}
