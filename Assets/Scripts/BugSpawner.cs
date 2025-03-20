using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    private int _currencyTotal;

    public GameObject bugEnemy;

    [SerializeField]
    private GameObject _spawnPoint1, _spawnPoint2, _spawnPoint3, _spawnPoint4;
    private int _sectionCheck;
    private bool _isOnSecondPoint;

    private void Start()
    {
        CurrencyCheck(0);
    }

    public void CurrencyCheck(int currency)
    {
        Debug.Log("Spawn Bug: " + _currencyTotal);

        _currencyTotal = currency;

        if(_currencyTotal < 200)
        {
            Debug.Log("Spawn Bug");
            Instantiate(bugEnemy, transform.position, Quaternion.identity);
            SpawnpointChange();
        }
    }

    public void SpawnpointChange()
    {
        if(_sectionCheck == 0 && _isOnSecondPoint == false)
        {
            transform.position = _spawnPoint2.transform.position;
            _isOnSecondPoint = true;
        }
        else if(_sectionCheck == 0 && _isOnSecondPoint == true)
        {
            transform.position = _spawnPoint1.transform.position;
            _isOnSecondPoint = false;
        }
    }
}
