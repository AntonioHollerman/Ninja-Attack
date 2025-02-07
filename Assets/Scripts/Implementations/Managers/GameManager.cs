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
        public static bool InMenu = false;
    
        void Start()
        {

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        
        }

        void Update()
        {

            // Disables the Cursor until the game menu is active
            if (InMenu)
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
            if ((Player.Players.Count == 0 || Hostile.Hostiles.Count == 0) && !InMenu)
            {
                GameOver();
            }
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

            InMenu = true;
        }

        public void Restart()
        {
            InMenu = false;
            Player.Players = new List<Player>();
            Hostile.Hostiles = new List<Hostile>();
            CharacterSheet.CharacterSheets = new List<CharacterSheet>();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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