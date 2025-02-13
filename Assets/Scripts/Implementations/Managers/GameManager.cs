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
        public GameObject gameOverUI;
        public GameObject credits;
        private bool _listeningForGameOver;
    
        void Start()
        {

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            StartRound(1, 1);
        }

        void Update()
        {

            // Disables the Cursor until the game menu is active
            if (!_listeningForGameOver)
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
            if ((Player.Players.Count == 0 || Hostile.Hostiles.Count == 0) && _listeningForGameOver)
            {
                GameOver();
            }
        }

        public void StartRound(int level, int round)
        {
            StartCoroutine(SpawnManager.Instance.SpawnEnemies(level, round));
            _listeningForGameOver = true;
        } 

        public void GameOver()
        {
            gameOverUI.SetActive(true);
            credits.SetActive(false);

            if (Player.Players.Count == 0)
            {
                gameOverText.text = "Nice Try, you got this next time!";
            }
            else
            {
                gameOverText.text = "CONGRATS ON WINNING!!!";
            }

            _listeningForGameOver = false;
        }

        public void Restart()
        {
            Player.Players = new List<Player>();
            Hostile.Hostiles = new List<Hostile>();
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