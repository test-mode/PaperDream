using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PaperDream
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _HUD;
        [SerializeField] private WeatherController _weatherController;

        float previousTimeScale = 1;
        public static bool isPaused = false;

        public static GameManager Instance;

        public GameState State;

        public static event Action<GameState> OnGameStateChanged;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            UpdateGameState(GameState.LevelOpening);
        }

        public void UpdateGameState(GameState newState)
        {
            State = newState;

            switch (newState)
            {
                case GameState.GameStarted:
                    break;
                case GameState.LevelOpening:
                    HandleLevelOpening();
                    break;
                case GameState.LevelRestart:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    UpdateGameState(GameState.LevelOpening);
                    TogglePause();
                    break;
                case GameState.Gameplay:
                    HandleGameplay();
                    break;
                case GameState.LevelCleared:
                    break;
                case GameState.LevelFailed:
                    break;
                case GameState.GamePaused:
                    TogglePause();
                    break;
                case GameState.GameClosed:
                    Application.Quit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            OnGameStateChanged?.Invoke(newState);
        }

        private void HandleLevelOpening()
        {
            _HUD.SetActive(false);

        }
        private void HandleGameplay()
        {
            _HUD.SetActive(true);
            EventManager.OnTimerStart();
            _weatherController.StartRain();

        }

        public void TogglePause()
        {
            if (Time.timeScale > 0)
            {
                previousTimeScale = Time.timeScale;
                Time.timeScale = 0;
                AudioListener.pause = true;

                isPaused = true;
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = previousTimeScale;
                AudioListener.pause = false;

                isPaused = false;
            }
        }
    }

    public enum GameState
    {
        GameStarted,
        LevelOpening,
        LevelRestart,
        Gameplay,
        LevelCleared,
        LevelFailed,
        GamePaused,
        GameClosed
    }
}