using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PaperDream
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _levelSelectionPanel;
        [SerializeField] private GameObject _levelFailedPanel;
        [SerializeField] private GameObject _pausePanel;

        private Panel panelManager;

        private void Awake()
        {
            GameManager.OnGameStateChanged += GameManagerOnGameStateChange;
            panelManager = new Panel();
        }

        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= GameManagerOnGameStateChange;
        }

        void Start()
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
            StartCoroutine(LoadScene());
        }

        private void GameManagerOnGameStateChange(GameState state)
        {
            //GameManager.Instance.UpdateGameState(GameState.GameStarted);
            _levelFailedPanel.SetActive(state == GameState.LevelFailed);
        }

        private System.Collections.IEnumerator LoadScene()
        {
            Scene currentScene = SceneManager.GetActiveScene();

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

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
        private HashSet<GameObject> activePanels = new HashSet<GameObject>();

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
