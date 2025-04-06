using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loading1, loading2, loading3, loading4;

    public void PlayGame()
    {
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(StartScene());
    }

    IEnumerator StartScene()
    {
        loading1.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        loading2.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        loading3.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        loading4.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Act 1");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("close game");
    }
}