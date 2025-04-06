using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1End : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            StartCoroutine(EndLevelCoroutine());
        }
    }

    IEnumerator EndLevelCoroutine()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("StartScreen");
    }
}
