using UnityEngine;
using UnityEngine.UI;

namespace TestTetris.UI
{
    public class LevelCouterUI : MonoBehaviour
    {
        [SerializeField] private Text _levelText;

        private void Start()
        {
            GameManager.OnChangeLevelCounterEvent += OnChangedLevelCounter;
        }

        private void OnDestroy()
        {
            GameManager.OnChangeLevelCounterEvent -= OnChangedLevelCounter;
        }

        private void OnChangedLevelCounter(int level)
        {
            _levelText.text = level.ToString().PadLeft(2, '0');
        }
    }
}
