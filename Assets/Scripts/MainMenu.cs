using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Act 1");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("close game");
    }
}