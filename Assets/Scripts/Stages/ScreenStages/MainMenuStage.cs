using System;
using TestTetris.Sounds;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TestTetris.Stages
{
    public class MainMenuStage : StageState
    {
        #region EVENTS
        public static event Action NewGameEvent;
        #endregion

        [SerializeField] private Selectable _firstFocusUIElement;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Toggle[] _toggleButtons;


        public void Awake()
        {
            Playfield.ChangeStateEvent += OnChangedState;
            _continueButton.gameObject.SetActive(false);
        }

        public void OnDestroy()
        {
            Playfield.ChangeStateEvent -= OnChangedState;
        }

        public override void Enter()
        {
            EventSystem.current.SetSelectedGameObject(_firstFocusUIElement.gameObject);

            int toggleMusicIndex = (int)MusicManager.MusicType;
            for (int i = 0; i < _toggleButtons.Length; i++)
            {
                _toggleButtons[i].SetIsOnWithoutNotify(i == toggleMusicIndex);
            }

            gameObject.SetActive(true);
        }

        public override void Exit()
        {
            gameObject.SetActive(false);
        }

        #region GUI Button Callback functions
        public void OnNewGameButtonClicked()
        {
            NewGameEvent?.Invoke();

            GameManager.Stages.SetState(StageTypes.Play);
        }

        public void OnContinueGameButtonClicked()
        {
            GameManager.Stages.SetState(StageTypes.Play);
        }

        public void OnSelectMusicClicked(int musicTypeId)
        {
            if (!_toggleButtons[musicTypeId].isOn)
            {
                return;
            }

            if (musicTypeId != (int)MusicManager.MusicType)
            {
                MusicManager.MusicType = (MusicType)musicTypeId;
            }
        }
        #endregion

        // Callback methods
        private void OnChangedState(Playfield.State state)
        {
            _continueButton.gameObject.SetActive(state == Playfield.State.Play);

            Navigation newButtonNavigation = _firstFocusUIElement.GetComponent<Button>().navigation;
            Navigation toggleNavigation = _toggleButtons[0].GetComponent<Toggle>().navigation;
            if (_continueButton.gameObject.activeSelf)
            {
                newButtonNavigation.selectOnDown = _continueButton;
                toggleNavigation.selectOnUp = _continueButton;
            }
            else
            {
                newButtonNavigation.selectOnDown = _toggleButtons[0];
                toggleNavigation.selectOnUp = _firstFocusUIElement;
            }
            _firstFocusUIElement.GetComponent<Button>().navigation = newButtonNavigation;
            _toggleButtons[0].GetComponent<Toggle>().navigation = toggleNavigation;
        }

    }
}
