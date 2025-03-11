using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject enemyAttack;
    public AudioSource enemyTelegraphSound;

    void Start()
    {
        StartCoroutine(AttackCoroutine());        
    }

    IEnumerator AttackCoroutine()
    {
        enemyTelegraphSound.Play();
        //after 1s the enemy telegraph is destroyed
        yield return new WaitForSeconds(1);
        Instantiate(enemyAttack, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        Destroy(this.gameObject);
    }

    
}
