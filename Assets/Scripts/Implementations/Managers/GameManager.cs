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
        
        private int _curLevel;
        private int _curRound;
        void Start()
        {
            
            statsUI.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {

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
        }

        public void Restart()
        {

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