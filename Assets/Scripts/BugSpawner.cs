using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    private int _currencyTotal;

    public GameObject bugEnemy;

    [SerializeField]
    private GameObject _spawnPoint1, _spawnPoint2, _spawnPoint3, _spawnPoint4;
    [SerializeField]
    private GameObject _fogPoint1, _fogPoint2;
    public int _sectionCheck;
    private bool _isOnSecondPoint;

    private void Start()
    {
        CurrencyCheck(0);
    }

    public void CurrencyCheck(int currency)
    {
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
        Debug.Log("Section check " + _sectionCheck);

        //Bug spawns
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
        else if(_sectionCheck == 1 && _isOnSecondPoint == false)
        {
            transform.position = _spawnPoint4.transform.position;
            _isOnSecondPoint = true;

        }
        else if(_sectionCheck == 1 && _isOnSecondPoint == true)
        {
            transform.position = _spawnPoint3.transform.position;
            _isOnSecondPoint = false;
        }
    }

    public void FogActivation()
    {
        //Fog activations
        if (_sectionCheck == 1)
        {
            _fogPoint1.SetActive(true);
        }
        if (_sectionCheck == 2)
        {
            _fogPoint1.SetActive(false);
            _fogPoint2.SetActive(true);
        }
    }
}
