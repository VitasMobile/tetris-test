using System;
using System.Collections.Generic;
using TestTetris.Types;

namespace TestTetris.GameData
{
    public class ShapeCountnerManager
    {
        public static event Action<Dictionary<ShapeType, int>> ChangeShapeCountersEvent;

        private Dictionary<ShapeType, int> _shapeCounters;

        public ShapeCountnerManager()
        {
            _shapeCounters = new Dictionary<ShapeType, int>();

            foreach (ShapeType type in Enum.GetValues(typeof(ShapeType)))
            {
                _shapeCounters.Add(type, 0);
            }
            ChangeShapeCountersEvent?.Invoke(_shapeCounters);
        }

        public void Reset()
        {
            for (int i = 0; i < _shapeCounters.Count; i++)
            {
                _shapeCounters[(ShapeType)i] = 0;
            }
            ChangeShapeCountersEvent?.Invoke(_shapeCounters);
        }

        public void Inc(ShapeType type)
        {
            if (_shapeCounters.ContainsKey(type))
            {
                _shapeCounters[type]++;
            }
            ChangeShapeCountersEvent?.Invoke(_shapeCounters);
        }
    }
}
