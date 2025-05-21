using UnityEngine;

public class GameOverController : MonoBehaviour
{
    public AudioSource gameOverMusic; // Assign this in inspector or prefab

    void OnEnable()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.StopBackgroundMusic();
        }

        if (gameOverMusic != null)
        {
            gameOverMusic.Play();
        }
    }
}
