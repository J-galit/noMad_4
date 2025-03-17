using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PredatorEnemyHealth : MonoBehaviour
{
    private Rigidbody _rb;

    public int health;
    [SerializeField]
    private TextMeshProUGUI _healthText;

    private bool isIFramesActive;

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
                _rb.AddForce(new Vector3(0, 0, -30), ForceMode.Impulse);
                StartCoroutine(InvincibilityCoroutine());
                //dies when health drops to 0 or below
                if(health <= 0)
                {
                    Destroy(gameObject);
                }
            }
            else if(health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator InvincibilityCoroutine()
    {
        //prevents enemy getting hit twice from entering triggers more than once
        yield return new WaitForSeconds(0.2f);
        isIFramesActive = false;
    }
}
