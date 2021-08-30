using TestTetris.GameData;
using TestTetris.Sounds;
using UnityEngine;
using UnityEngine.UI;

namespace TestTetris.Stages
{
    public class WinStage : StageState
    {
        [SerializeField] private Text _score;
        [SerializeField] private Text _topScore;

        [SerializeField] private GameObject _yourScore;
        [SerializeField] private GameObject _newRecord;


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

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Stages.SetState(StageTypes.MainMenu);
            }
        }

        public override void Enter()
        {
            gameObject.SetActive(true);

            if (Score.IsTopScore())
            {
                _yourScore.gameObject.SetActive(false);
                _newRecord.gameObject.SetActive(true);

                SoundManager.PlaySound(SoundClip.TopScore);
            }
            else
            {
                _yourScore.gameObject.SetActive(true);
                _newRecord.gameObject.SetActive(false);

                SoundManager.PlaySound(SoundClip.GameOver);
            }
        }

        public override void Exit()
        {
            gameObject.SetActive(false);
        }

        private void OnChangedScore(int score)
        {
            _score.text = score.ToString().PadLeft(7, '0');
        }

        private void OnChangedTopScore(int topScore)
        {
            _topScore.text = topScore.ToString().PadLeft(7, '0');
        }

    }
}
