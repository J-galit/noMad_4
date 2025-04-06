using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCursor : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
       UnityEngine.Cursor.visible = true;
       UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
