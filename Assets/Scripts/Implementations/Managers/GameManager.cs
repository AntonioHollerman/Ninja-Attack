using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BaseClasses;
using Implementations.Extras;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Implementations.Managers
{
    public class GameManager : MonoBehaviour
    {
        public TextMeshProUGUI gameOverText;
        public GameObject statsUI;
        public GameObject gameOverUI;
        public GameObject credits;
        public GameObject pauseMenu;
        public GameObject fadeScreen;
        public int level;
        
        private bool _listeningForRoundOver;
        private int _curLevel;
        private int _curRound;
        private FadeScreen _fs;
        private Dictionary<int, List<int>> _avaRounds = new Dictionary<int, List<int>>();

        private bool IsAnotherRound => _avaRounds[_curRound].Contains(_curRound + 1);
        private bool IsAnotherLevel=> _avaRounds.ContainsKey(_curLevel + 1);
        private bool _transitioning;
        void Start()
        {
            _avaRounds[1] = new List<int>{1};
            _avaRounds[2] = new List<int>{1, 2};
            _avaRounds[3] = new List<int>{1, 2};
            
            _fs = fadeScreen.GetComponent<FadeScreen>();
            _curLevel = level;
            _curRound = 1;
            
            statsUI.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            StartCoroutine(StartRound(_curRound, true));
        }

        void Update()
        {
            if (!_listeningForRoundOver)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {

                Cursor.visible = false;
                Cursor.lockState= CursorLockMode.Locked;
            }



            // Check if both players are defeated
            if (Player.Players.Count == 0  && _listeningForRoundOver)
            {
                GameOver();
                return;
            }

            if (Hostile.Hostiles.Count == 0 && _listeningForRoundOver)
            {
                RoundOver();
            }
        }

        public IEnumerator StartRound(int round, bool fadeOut = false)
        {
            CharacterSheet.UniversalStopCsUpdateLoop = true;
            if (fadeOut)
            {
                StartCoroutine(_fs.FadeOut());
                yield return new WaitUntil(() => _fs.Opacity <= 0);
            }
            
            StartCoroutine(SpawnManager.Instance.SpawnEnemies(_curLevel, round));
            _listeningForRoundOver = true;
            yield return null;
        }

        public void RoundOver()
        {
            if (_transitioning)
            {
                return;
            }
            if (IsAnotherRound)
            {
                _curRound++;
                StartCoroutine(StartRound(_curRound));
                return;
            }

            if (IsAnotherLevel)
            {
                _transitioning = true;
                _curLevel++;
                _curRound = 1;
                StartCoroutine(SwapLevel());
                return;
            }
            GameOver();
        }

        private IEnumerator SwapLevel()
        {
            StartCoroutine(_fs.FadeIn());
            yield return new WaitUntil(() => Mathf.Approximately(_fs.Opacity, 1.0f));
            SceneManager.LoadScene(_curLevel);
        }
        public void GameOver()
        {
            CharacterSheet.UniversalStopCsUpdateLoop = true;
            gameOverUI.SetActive(true);
            credits.SetActive(false);
            statsUI.SetActive(false);

            if (Player.Players.Count == 0)
            {
                gameOverText.text = "Nice Try";
            }
            else
            {
                gameOverText.text = "CONGRATS ON WINNING!!!";
            }

            _listeningForRoundOver = false;
        }

        public void Restart()
        {
            _curLevel = 1;
            _curRound = 1;
            StartCoroutine(SwapLevel());
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }

        public void GoCredits()
        {
            gameOverUI.SetActive(false);
            credits.SetActive(true);
            statsUI.SetActive(false);
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }
    }
}