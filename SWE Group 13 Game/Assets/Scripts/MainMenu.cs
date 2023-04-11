using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
   public void PlayGame()
    {
        SceneManager.LoadScene(1);
        //SceneManager.GetActiveScene().buildIndex + 1
    }
    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }
    public void QuiteGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
    
}
