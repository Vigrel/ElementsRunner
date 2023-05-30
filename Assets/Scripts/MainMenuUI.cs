using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public GameObject startButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton.SetActive(true);
    }

    // Update is called once per frame
    public void playGame()
    {
        SceneManager.LoadScene("play_game");
    }
}
