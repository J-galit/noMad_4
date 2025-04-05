using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class HoverOverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ThirdPersonCharacterController _thirdPersonCharacterController;
    [SerializeField] private GameObject adaptationDesc;

    // Start is called before the first frame update
    void Start()
    {
        _thirdPersonCharacterController = GameObject.Find("Player").GetComponent<ThirdPersonCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab) || Input.GetKey(KeyCode.Escape))
        {
            adaptationDesc.SetActive(false);
        }
    }

    public void DisableDescription()
    {
        adaptationDesc.SetActive(false);

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
