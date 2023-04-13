using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    public Text scoreCounter;
    public Text winScreen;
    public Text loseScreen;

    void Start()
    {
        loseScreen.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(false);
    }
    public void UpdateScore(int score)
    {
        scoreCounter.text = score.ToString();
    }

    public void lose()
    {
        loseScreen.gameObject.SetActive(true);
    }

    public void win()
    {
        winScreen.gameObject.SetActive(true);
    }

    public void playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
