using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject score;
    private ScoreController _scoreControllerScript;
    
    public void GoToMainMenu()
    {
        _scoreControllerScript = score.GetComponent<ScoreController>();
        _scoreControllerScript.SaveScore();
        
        SceneManager.LoadScene("end_game");
    }
}
