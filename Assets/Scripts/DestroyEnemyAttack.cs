using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemyAttack : MonoBehaviour
{
 
    void Start()
    {
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        //after 0.15s enemy attack is destroyed
        yield return new WaitForSeconds(0.15f);
       Destroy(this.gameObject);
    }
}
