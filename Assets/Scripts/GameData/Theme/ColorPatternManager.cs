using UnityEngine;

namespace TestTetris.GameData.Theme
{
    [System.Serializable]
    public class ColorPatternManager
    {
        [SerializeField] private ColorPattern[] _colorPatterns = new ColorPattern[0];

        private int _colorPatternIndex = 0;


        public ColorPatternManager()
        {
            GameManager.OnChangeLevelCounterEvent += OnChangedLevelCounter;
        }

        ~ColorPatternManager()
        {
            GameManager.OnChangeLevelCounterEvent -= OnChangedLevelCounter;
        }

        public Color GetRndColor()
        {
            Color[] colors = _colorPatterns[_colorPatternIndex].colors;
            return colors[Random.Range(0, colors.Length)];
        }

        #region Callbacks
        private void OnChangedLevelCounter(int level)
        {
            if (level == 0)
            {
                return;
            }

            _colorPatternIndex++;
            if (_colorPatternIndex >= _colorPatterns.Length)
            {
                _colorPatternIndex = 0;
            }
        }
        #endregion

    }
}
