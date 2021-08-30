using System.Collections.Generic;
using UnityEngine;

namespace TestTetris.Stages
{
    public class StageStateMachine
    {
        public delegate void OnChangeStageHandle(StageTypes stage);
        public static event OnChangeStageHandle ChangeStageEvent;
        private Dictionary<StageTypes, StageState> _stageStates = new Dictionary<StageTypes, StageState>();
        private StageTypes _currentStageType = StageTypes.MainMenu;
        private StageState _currentStageState;


        public StageStateMachine()
        {
        }

        public void RegisterStage(StageTypes stageType, StageState stageState)
        {
            if (_stageStates.ContainsKey(stageType))
            {
                throw new System.Exception($"{stageType} уже имеется.");
            }
            _stageStates.Add(stageType, stageState);
        }

        public void SetState(StageTypes stageType)
        {
            if (_currentStageState != null)
            {
                _currentStageState.Exit();
            }


            if (_stageStates.ContainsKey(stageType))
            {
                _currentStageState = _stageStates[stageType];
            }
            else
            {
                Debug.LogWarning($"{stageType} не найден!");
            }

            _currentStageType = stageType;
            ChangeStageEvent?.Invoke(_currentStageType);

            _currentStageState?.Enter();
        }
    }
}
