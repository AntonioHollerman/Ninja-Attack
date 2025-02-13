using System.Collections.Generic;
using BaseClasses;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Implementations.Managers
{
    public class MenuManager : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}