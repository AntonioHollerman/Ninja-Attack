using BaseClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;

    private Player playerOne;
    private Player playerTwo;

    void Start()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


        // Find both players in the scene by their names
        playerOne = GameObject.Find("PlayerOne").GetComponent<Player>();
        playerTwo = GameObject.Find("PlayerTwo").GetComponent<Player>();
    }

    void Update()
    {

        // Disables the Cursor until the game menu is active
        if (gameOverUI.activeInHierarchy)
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
        if (playerOne != null && playerTwo != null && playerOne.IsDefeated() && playerTwo.IsDefeated())
        {
            gameOver();
        }
    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0; // Stop game logic
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("restart");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Debug.Log("main menu");
    }

    public void quit()
    {
        Application.Quit();
        Debug.Log("quit");
    }

}