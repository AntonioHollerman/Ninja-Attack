using UnityEngine;

public enum DungeonLevelType
{
    Level1_2,
    Level3,
    BossRoom
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioClip level1_2Music;
    public AudioClip level3Music;
    public AudioClip bossRoomMusic;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeMusic(DungeonLevelType levelType)
    {
        AudioClip clipToPlay = null;
        switch (levelType)
        {
            case DungeonLevelType.Level1_2:
                clipToPlay = level1_2Music;
                break;
            case DungeonLevelType.Level3:
                clipToPlay = level3Music;
                break;
            case DungeonLevelType.BossRoom:
                clipToPlay = bossRoomMusic;
                break;
        }

        if (clipToPlay != null && audioSource.clip != clipToPlay)
        {
            audioSource.Stop();
            audioSource.clip = clipToPlay;
            audioSource.Play();
        }
    }
}

