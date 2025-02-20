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

        private bool IsAnotherRound => SpawnPos.Spawns
            .Any(p => p.level == _curLevel && p.round == _curRound + 1);
        private bool IsAnotherLevel=> SpawnPos.Spawns
            .Any(p => p.level == _curLevel + 1 && p.round == 1);
        void Start()
        {
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
            if (IsAnotherRound)
            {
                _curRound++;
                StartCoroutine(StartRound(_curRound));
                return;
            }

            if (IsAnotherLevel)
            {
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
            
            StartCoroutine(_fs.FadeOut());
            yield return new WaitUntil(() => _fs.Opacity >= 0);
            
            StartCoroutine(StartRound(_curRound));
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