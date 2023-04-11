using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PScore
{
    public string Name;
    public float score;

    public PScore(string name, float score)
    {
        this.Name = name;
        this.score = score;
    }
    public override string ToString()
    {
        return Name + " " + score;
    }
}
