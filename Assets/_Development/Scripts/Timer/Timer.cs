using System;
using UnityEngine;
using UnityEngine.UI;

namespace PaperDream
{
    public class Timer : MonoBehaviour
    {
        #region Variables

        private Text _timerText;
        enum TimerType { Countdown, Stopwatch }
        [SerializeField] private TimerType _timerType;

        [SerializeField] private float _timeToDisplay = 60.0f;

        private bool _isRunning;

        #endregion

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
        private void EventManagerOnTimerStop() => _isRunning = false;
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
