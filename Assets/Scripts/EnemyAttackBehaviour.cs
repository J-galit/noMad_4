using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject enemyAttack;
   

    void Start()
    {
        StartCoroutine(AttackCoroutine());        
    }

    IEnumerator AttackCoroutine()
    {
        //after 1s the enemy telegraph is destroyed
        yield return new WaitForSeconds(1);
        Instantiate(enemyAttack, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        Destroy(this.gameObject);
    }

    
}
