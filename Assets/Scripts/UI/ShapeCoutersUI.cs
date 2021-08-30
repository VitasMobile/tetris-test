using System.Collections.Generic;
using TestTetris.GameData;
using TestTetris.Types;
using UnityEngine;
using UnityEngine.UI;

namespace TestTetris.UI
{
    [System.Serializable]
    public class ShapeCounter
    {
        public ShapeType type;
        public Text text;
    }

    public class ShapeCoutersUI : MonoBehaviour
    {
        [SerializeField] private ShapeCounter[] _shapeCounterTexts;

        private void Start()
        {
            ShapeCountnerManager.ChangeShapeCountersEvent += OnChangedShapeCounters;
        }

        private void OnDestroy()
        {
            ShapeCountnerManager.ChangeShapeCountersEvent -= OnChangedShapeCounters;
        }

        private void OnChangedShapeCounters(Dictionary<ShapeType, int> shapes)
        {
            foreach (ShapeCounter shapeCounter in _shapeCounterTexts)
            {
                if (shapes.ContainsKey(shapeCounter.type))
                {
                    shapeCounter.text.text = $"{shapes[shapeCounter.type].ToString().PadLeft(3, '0')}";
                }
            }
        }
    }
}
