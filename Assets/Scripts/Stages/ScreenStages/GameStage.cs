using UnityEngine;

namespace TestTetris.Stages
{

    public class GameStage : StageState
    {
        public override void Enter()
        {
            GameData.Score.DoUpdate();
            gameObject.SetActive(true);
        }

        public override void Exit()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Stages.SetState(StageTypes.MainMenu);
            }
        }
    }

}
