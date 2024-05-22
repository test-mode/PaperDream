using System;
using UnityEngine;
using UnityEngine.UI;

namespace PaperDream
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private float _timeToDisplay = 60.0f;
        [SerializeField] private TimerType _timerType;
        enum TimerType { Countdown, Stopwatch }

        private Text _timerText;

        private bool _isRunning;

        private void Awake() => _timerText = GetComponent<Text>();

        private void OnEnable()
        {
            EventManager.TimerStart += EventManagerOnTimerStart;
            EventManager.TimerStop += EventManagerOnTimerStop;
            EventManager.TimerUpdate += EventManagerOnTimerUpdate;
        }

        private void OnDisable()
        {
            EventManager.TimerStart -= EventManagerOnTimerStart;
            EventManager.TimerStop -= EventManagerOnTimerStop;
            EventManager.TimerUpdate -= EventManagerOnTimerUpdate;
        }

        private void EventManagerOnTimerStart() => _isRunning = true;
        private void EventManagerOnTimerStop()
        {
            GameManager.Instance.UpdateGameState(GameState.LevelFailed);
            _isRunning = false;
        }
        private void EventManagerOnTimerUpdate(float value) => _timeToDisplay += value;

        private void Update()
        {
            if (!_isRunning) return;
            if (_timerType == TimerType.Countdown && _timeToDisplay < 0.0f)
            {
                EventManager.OnTimerStop();
                return;
            }

            _timeToDisplay += _timerType == TimerType.Countdown ? -Time.deltaTime : Time.deltaTime;

            TimeSpan timeSpan = TimeSpan.FromSeconds(_timeToDisplay);
            _timerText.text = timeSpan.ToString(@"mm\:ss\:ff");
        }
    }
}
