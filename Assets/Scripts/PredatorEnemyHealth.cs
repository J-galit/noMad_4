using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PredatorEnemyHealth : MonoBehaviour
{
    private Rigidbody _rb;

    public int health;
    private bool isInFog;
    [SerializeField]
    private TextMeshProUGUI _healthText;

    private bool isIFramesActive;

    [SerializeField] private Animator animator;

    private void Awake()
    {
        isIFramesActive = false;
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Attack"))
        {
            if(health > 0 && isIFramesActive == false)
            {
                //lose health
                health--;
                //give invincibility
                isIFramesActive = true;
                _healthText.text = health.ToString();
                _rb.AddForce(new Vector3(0, 0, -75), ForceMode.Impulse);
                StartCoroutine(InvincibilityCoroutine());
                //dies when health drops to 0 or below
                if(health <= 0)
                {
                    StartCoroutine(DeathCoroutine());
                }
            }
            else if(health <= 0)
            {
                StartCoroutine(DeathCoroutine());
            }
        }

        if (other.CompareTag("Fog"))
        {
            isInFog = true;
            StartCoroutine(FogCoroutine());
        }
    }
    private IEnumerator FogCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        if (isInFog == true)
        {
            health--;
            _healthText.text = health.ToString();
            if (health <= 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, -90);
                StartCoroutine(DeathCoroutine());
            }
            else
            {
                StartCoroutine(FogCoroutine());
            }
        }
    }

    private IEnumerator DeathCoroutine()
    {
        animator.SetBool("isDead", true);
        //transform.localRotation = Quaternion.Euler(0, 0, -90);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    IEnumerator InvincibilityCoroutine()
    {
        //prevents enemy getting hit twice from entering triggers more than once
        yield return new WaitForSeconds(0.2f);
        isIFramesActive = false;
    }
}
