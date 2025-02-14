using System.Collections.Generic;
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
        private bool _listeningForRoundOver;
    
        void Start()
        {
            statsUI.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            StartRound(1, 1);
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
            if ((Player.Players.Count == 0 || Hostile.Hostiles.Count == 0) && _listeningForRoundOver)
            {
                GameOver();
            }
        }

        public void StartRound(int level, int round)
        {
            StartCoroutine(SpawnManager.Instance.SpawnEnemies(level, round));
            _listeningForRoundOver = true;
        } 

        public void GameOver()
        {
            CharacterSheet.UniversalStopCsUpdateLoop = true;
            gameOverUI.SetActive(true);
            credits.SetActive(false);
            statsUI.SetActive(false);

            if (Player.Players.Count == 0)
            {
                gameOverText.text = "Nice Try, you got this next time!";
            }
            else
            {
                gameOverText.text = "CONGRATS ON WINNING!!!";
            }

            _listeningForRoundOver = false;
        }

        public void Restart()
        {
            CharacterSheet.CharacterSheets = new List<CharacterSheet>();
            SceneManager.LoadScene(1);
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