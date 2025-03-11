using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugEnemyDeathDrop : MonoBehaviour
{
    [SerializeField] private GameObject currencyDrop;

    private void OnTriggerEnter(Collider other)
    {
        //if hit by player attack
        if (other.CompareTag("Attack"))
        {
            //drops currency
            Instantiate(currencyDrop, transform.position, Quaternion.identity);
            //destroys bug
            Destroy(gameObject);
        }
    }
}
