using UnityEngine;

namespace TestTetris.Stages
{
    public abstract class StageState : MonoBehaviour
    {
        [SerializeField] private StageTypes _stageType;

        protected virtual void Start()
        {
            GameManager.Stages.RegisterStage(_stageType, this);
            gameObject.SetActive(false);
        }

        public abstract void Enter();
        public abstract void Exit();
    }
}
