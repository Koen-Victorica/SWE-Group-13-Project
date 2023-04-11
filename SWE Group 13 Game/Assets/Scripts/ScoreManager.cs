using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public InputField answer;
      
    public List<PScore> Highscores = new List<PScore>();

    private void Awake()
    {
        GetScores();   
    }

    void Start()
    {
        
    }

    public void SaveScore()
    {
        try
        {
            StreamWriter sw = new StreamWriter(Application.dataPath + "/highscores.txt");
            string dataline = "";
            foreach(PScore score in Highscores)
            {
                dataline = score.ToString();
                sw.WriteLine(dataline);
            }
            sw.Close();
        }
        
        catch (FileNotFoundException e)
        {
            Debug.Log(e.Message);
        }
        catch (IOException e)
        {
            Debug.Log(e.Message);
        }
        catch (IndexOutOfRangeException)
        {
            return;
        }
    }
    public void GetScores()
    {
        try
        {
            StreamReader sr = new StreamReader(Application.dataPath + "/highscores.txt");
            string dataline = "";                                 
                while (!sr.EndOfStream)
                {
                    dataline = sr.ReadLine();
                    string[] values = dataline.Split(' ');
                    string name = values[0];
                    int score = int.Parse(values[1]);
                    Highscores.Add(new PScore(name, score));
                }           
            sr.Close();
        }
        
        catch(FileNotFoundException e)
        {
            Debug.Log(e.Message);
        }
        catch (IOException e)
        {
            Debug.Log(e.Message);           
        }
        catch (IndexOutOfRangeException)
        {
            return;
        }

    }
    
    public void AddPScore(string name)
    {
        Highscores.Add(new PScore(name, ScoreBoss.instance.setScore()));
    }
    public void ReadStringInput()
    {        
        AddPScore(answer.text);
        Debug.Log(answer.text);
    }

    public IEnumerable<PScore> sendScores()
    {
        return Highscores.OrderByDescending(x => x.score);
    }
    
}
