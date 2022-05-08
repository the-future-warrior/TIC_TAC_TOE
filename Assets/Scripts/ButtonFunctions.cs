using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    public static bool playerVSai;

    public static string resultString;
    public Text result;

    // Start is called before the first frame update
    void Start()
    {
        if(GameController.gameOver)
        {
            result.text = resultString;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Player1()
    {
        playerVSai = true;
        SceneManager.LoadScene("1 Player");
    }

    public void Players2()
    {
        playerVSai = false;
        SceneManager.LoadScene("2 Players");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        GameController.gameOver = false;

        //playerVSai is reversed unfortunately
        if (!playerVSai)
        {
            SceneManager.LoadScene("2 Players");
        }
        else
        {
            SceneManager.LoadScene("1 Player");
        }
    }

    public void LoadMenu()
    {
        GameController.gameOver = false;
        SceneManager.LoadScene("Main Menu");
    }
}
