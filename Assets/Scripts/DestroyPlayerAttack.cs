using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.VFX;

public class DestroyPlayerAttack : MonoBehaviour
{
    private ThirdPersonCharacterController thirdPersonCharacterController;
    [SerializeField] private GameObject _newAttack;
    [SerializeField] private GameObject _fastAttack;

    [SerializeField] private VisualEffect[] vfxGraphs;

    [GradientUsage(true)]
    [SerializeField] private Gradient baseColour;
    [GradientUsage(true)]
    [SerializeField] private Gradient fastColour;


    private void Awake()
    {
        thirdPersonCharacterController = GameObject.Find("Player").GetComponent<ThirdPersonCharacterController>();
        if (thirdPersonCharacterController.isFasterAttackActive)
        {
            foreach (VisualEffect clawGraph in vfxGraphs)
                clawGraph.SetGradient(Shader.PropertyToID("ClawColour"), fastColour);
        }
        else
        {
            foreach (VisualEffect clawGraph in vfxGraphs)
                clawGraph.SetGradient(Shader.PropertyToID("ClawColour"), baseColour);
        }

    }

    void Start()
    {
        

        if (thirdPersonCharacterController.isLargerAttackActive == true)
        {
            _newAttack.SetActive(true);
        }
        else
        {
            _newAttack.SetActive(false);
        }
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        //after 0.15s attack object is destroyed
        yield return new WaitForSeconds(0.15f);
        Destroy(this.gameObject);
    }
}
