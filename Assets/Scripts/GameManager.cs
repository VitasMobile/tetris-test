using System.Collections;
using TestTetris.GameData;
using TestTetris.GameData.Theme;
using TestTetris.Sounds;
using TestTetris.Stages;
using UnityEngine;


namespace TestTetris
{
    public class GameManager : MonoBehaviour
    {
        #region EVENTS
        public delegate void OnChangeLineHandle(int lineCount);
        public static event OnChangeLineHandle OnChangeLineCounterEvent;

        public delegate void OnChangeLevelHandle(int level);
        public static event OnChangeLevelHandle OnChangeLevelCounterEvent;
        #endregion


        #region Game Data
        private int _levelCount = 1;
        private int _linesCount = 0;


        public static StageStateMachine Stages { get; private set; }

        private ShapeCountnerManager _shapeCounterManager;
        public static ShapeCountnerManager ShapeCoutnerManager => _instance._shapeCounterManager;

        private Score _score;
        public static Score Score => _instance._score;

        [SerializeField] private ColorPatternManager _colorPatternManager;
        public static ColorPatternManager ColorPatternManager => _instance._colorPatternManager;
        #endregion


        #region Instance or Simple Singleton )
        private static GameManager _instance;
        private void Awake()
        {
            _instance = this;

            Stages = new StageStateMachine();
            _score = new Score();
            _shapeCounterManager = new ShapeCountnerManager();
        }
        #endregion




        #region Mono functions
        private IEnumerator Start()
        {
            MainMenuStage.NewGameEvent += OnNewGame;
            yield return new WaitForEndOfFrame();
            Stages.SetState(StageTypes.MainMenu);
        }

        private void OnDestroy()
        {
            MainMenuStage.NewGameEvent -= OnNewGame;
        }
        #endregion


        #region Public static fucntions
        public static void IncLines()
        {
            _instance._linesCount++;
            OnChangeLineCounterEvent?.Invoke(_instance._linesCount);
            if (_instance._linesCount % 10 == 0)
            {
                _instance.LevelUp();
            }
        }

        #endregion


        #region Private functions
        private void OnNewGame()
        {
            _instance._levelCount = 1;
            OnChangeLevelCounterEvent?.Invoke(_instance._levelCount);

            _instance._linesCount = 0;
            OnChangeLineCounterEvent?.Invoke(_instance._linesCount);

            _instance._shapeCounterManager.Reset();

            _score.Reset();
        }

        private void LevelUp()
        {
            _levelCount++;
            OnChangeLevelCounterEvent?.Invoke(_levelCount);

            SoundManager.PlaySound(SoundClip.LevelUp);
        }
        #endregion
    }
}
