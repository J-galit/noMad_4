using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICurrency : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currencyText;

    public void UpdateCurrency(int currency)
    {
        //displays the currency variable
        currencyText.text = currency.ToString() + " $";
    }
}
