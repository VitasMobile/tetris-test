using UnityEngine;
using UnityEngine.UI;

namespace TestTetris.UI
{
    public class LineCouterUI : MonoBehaviour
    {
        [SerializeField] private Text _lineCounterText;

        private void Start()
        {
            GameManager.OnChangeLineCounterEvent += OnChangedLineCounter;
        }

        private void OnDestroy()
        {
            GameManager.OnChangeLineCounterEvent -= OnChangedLineCounter;
        }

        private void OnChangedLineCounter(int lines)
        {
            _lineCounterText.text = $"LINES-{lines.ToString().PadLeft(3, '0')}";
        }
    }
}
