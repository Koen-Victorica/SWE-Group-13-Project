using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public RowUI rowUi;
    public ScoreManager scoreManager;
    private int length;

    void Start()
    {
        var scores = scoreManager.sendScores().ToArray();
        if(scores.Length < 5)
        {
            length = scores.Length;
        }
        else
        {
            length = 5;
        }

        
        for (int i = 0; i < length; i++)
        {
            var row = Instantiate(rowUi, transform).GetComponent<RowUI>();
            row.rank.text = (i + 1).ToString();
            row.Name.text = scores[i].Name;
            row.score.text = scores[i].score.ToString();
        }
    }
}