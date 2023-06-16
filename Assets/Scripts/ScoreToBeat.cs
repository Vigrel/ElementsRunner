using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreToBeat : MonoBehaviour
{
    private TextMeshProUGUI _scoreText;
        
    private string FormatScoreWithCommas(int score)
    {
        string scoreString = score.ToString("N0"); // Format the score with commas
        return scoreString; 
    }
        
    private int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
        int highScore = GetHighScore();
        string highScoreString = FormatScoreWithCommas(highScore);
        
        _scoreText.text = "Score to beat: \n" + highScoreString;
    }
}
