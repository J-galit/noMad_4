using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class HoverOverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject adaptationDesc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        adaptationDesc.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        adaptationDesc.SetActive(false);
    }
}
