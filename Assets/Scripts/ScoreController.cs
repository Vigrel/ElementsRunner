using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public bool isActive = true;
    private TextMeshProUGUI _scoreText;
    private float _counter = 0f;
    private int _score = 0;
    
    private string FormatScoreWithCommas(int score)
    {
        string scoreString = score.ToString("N0"); // Format the score with commas
        return scoreString; 
    }
    
    public void PauseScore()
    {
        isActive = false;
    }
    
    public void ResumeScore()
    {
        isActive = true;
    }

    public void SaveScore()
    {
        // Retrieve the high score
        int savedHighScore = PlayerPrefs.GetInt("HighScore");
        
        if (savedHighScore < _score)
        {
            // Save the high score
            PlayerPrefs.SetInt("HighScore", _score);
            PlayerPrefs.Save();
        }
    }
    
    private void Start()
    {
        isActive = true;
        _scoreText = GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        if (!isActive) return;
        // Get elapsed time since last time called
        _counter += Time.deltaTime;
        
        // Update score
        _score = (int) (_counter * 100f);

        // Update score text with commas
        _scoreText.text = FormatScoreWithCommas(_score);
    }
}
