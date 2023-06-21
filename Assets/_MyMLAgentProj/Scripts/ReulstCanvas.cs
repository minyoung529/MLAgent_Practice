using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReulstCanvas : MonoBehaviour
{
    public Text text;
    
    public void Victory()
    {
        gameObject.SetActive(true);
        text.text = "승리";
    }

    public void Defeat()
    {
        gameObject.SetActive(true);
        text.text = "패배";
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Reload()
    {
        SceneManager.LoadScene("SoccerGame");
    }
}
