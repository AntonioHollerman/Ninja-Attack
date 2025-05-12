using TMPro;
using UnityEngine;
using System;

public class Keybind : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string keyName; // Example: "Key_Up", "Key_Down", etc.

    [Header("Objects")]
    [SerializeField] private TextMeshProUGUI buttonLbl;

    private bool awaitingInput = false;

    private void Start()
    {
        if (PlayerPrefs.HasKey(keyName))
        {
            buttonLbl.text = PlayerPrefs.GetString(keyName);
        }
    }

    private void Update()
    {
        if (!awaitingInput) return;

        foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keycode))
            {
                buttonLbl.text = keycode.ToString();
                PlayerPrefs.SetString(keyName, keycode.ToString());
                PlayerPrefs.Save();
                awaitingInput = false;
                break;
            }
        }
    }

    public void ChangeKey()
    {
        buttonLbl.text = "Awaiting Input";
        awaitingInput = true;
    }
}