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
            Player.Players = new List<Player>();
            Hostile.Hostiles = new List<Hostile>();
            CharacterSheet.CharacterSheets = new List<CharacterSheet>();
            SceneManager.LoadScene(1);
        }
    }
}