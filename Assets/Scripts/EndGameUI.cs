using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    public GameObject mainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuButton.SetActive(true);
    }

    // Update is called once per frame
    public void goToMainMenu()
    {
        SceneManager.LoadScene("main_menu");
    }
}
