using System.Collections.Generic;
using BaseClasses;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Implementations.Managers
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject tutorialScreen;
        public GameObject mainMenu;
        public void SwapScreen()
        {
            tutorialScreen.SetActive(true);
            mainMenu.SetActive(false);
        }

        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}