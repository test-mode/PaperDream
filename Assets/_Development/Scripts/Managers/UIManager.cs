using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PaperDream
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _levelSelectionPanel;
        [SerializeField] private GameObject _levelFailedPanel;
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _playPanel;
        [SerializeField] private GameObject _winPanel;

        private Panel panelManager;

        private void Awake()
        {
            GameManager.OnGameStateChanged += GameManagerOnGameStateChange;
            EventManager.ReachedDestination += OnReachedDestination;
            panelManager = new Panel();
        }

        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= GameManagerOnGameStateChange;
            EventManager.ReachedDestination -= OnReachedDestination;
        }

        private async void OnReachedDestination()
        {
            await Task.Delay(2000);
            _playPanel.SetActive(false);
            _winPanel.SetActive(true);
        }

        private void Start()
        {

        }

        public void OnPlayButton()
        {
            _levelSelectionPanel.SetActive(true);
        }

        public void OnPauseToggleButton()
        {
            TogglePanel(_pausePanel);
        }

        public void OnRestartButton()
        {
            GameManager.Instance.UpdateGameState(GameState.LevelRestart);
        }

        public void OnReturnMenu()
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        public void OnBackButton()
        {
            _levelSelectionPanel.SetActive(false);
        }

        public void OnQuitButton()
        {
            GameManager.Instance.UpdateGameState(GameState.GameClosed);
        }

        public void OnSelectLevelButton(int index)
        {
            //GameManager.Instance.UpdateGameState(GameState.LevelOpening);
            //StartCoroutine(LoadScene(index));
            SceneManager.LoadScene(index, LoadSceneMode.Single);
        }

        private void GameManagerOnGameStateChange(GameState state)
        {
            //GameManager.Instance.UpdateGameState(GameState.GameStarted);
            _levelFailedPanel.SetActive(state == GameState.LevelFailed);
        }

        private System.Collections.IEnumerator LoadScene(int index = 0)
        {
            Scene currentScene = SceneManager.GetActiveScene();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            yield return SceneManager.UnloadSceneAsync(currentScene);

            SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        }

        public void TogglePanel(GameObject panel)
        {
            GameManager.Instance.UpdateGameState(GameState.GamePaused);
            panel.SetActive(GameManager.isPaused);
            if (panel.activeSelf)
                panelManager.RegisterPanel(panel);
            else
                panelManager.UnregisterPanel(panel);
        }
    }

    public class Panel
    {
        private readonly HashSet<GameObject> activePanels = new();

        public void RegisterPanel(GameObject panel)
        {
            activePanels.Add(panel);
        }

        public void UnregisterPanel(GameObject panel)
        {
            activePanels.Remove(panel);
        }

        public bool AnyPanelActive()
        {
            return activePanels.Count > 0;
        }
    }
}
