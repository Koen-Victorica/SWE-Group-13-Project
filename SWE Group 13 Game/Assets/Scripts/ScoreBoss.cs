using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class ScoreBoss : MonoBehaviour
{
    public static ScoreBoss instance;
    public Text scoreText;
    int score = 0;
    
    private void Awake()
    {
        instance = this;

    }
    void Start()
    {
        scoreText.text = score.ToString() + " POINTS";
    }

    
    void Update()
    {
        
    }

    public void AddPoint()
    {
        score += 1;
        scoreText.text = score.ToString() + " POINTS";
    }
    public int setScore()
    {
        return score;
    }
    
}
