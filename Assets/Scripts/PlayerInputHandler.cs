using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name Reference")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string look = "Look";
    [SerializeField] private string attack = "Attack";
    [SerializeField] private string shop = "Shop";
    


    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;
    private InputAction attackAction;
    private InputAction shopAction;

    //This is what you reference.
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool AttackTriggered { get; private set; }
    public bool ShopTriggered { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }

    private void Awake()
    {
        print("InputManager Working");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }

        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        lookAction = playerControls.FindActionMap(actionMapName).FindAction(look);
        jumpAction = playerControls.FindActionMap(actionMapName).FindAction(jump);
        attackAction = playerControls.FindActionMap(actionMapName).FindAction(attack);
        shopAction = playerControls.FindActionMap(actionMapName).FindAction(shop);
        RegisterInputActions();
    }

    void RegisterInputActions() 
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => LookInput = Vector2.zero;

        jumpAction.performed += context => JumpTriggered = true;
        jumpAction.canceled += context => JumpTriggered = false;

        attackAction.performed += context => AttackTriggered = true;
        attackAction.canceled += context => AttackTriggered = false;

        shopAction.performed += context => ShopTriggered = true;
        shopAction.canceled += context => ShopTriggered = false;
    }


    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        attackAction.Enable();
        shopAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        attackAction.Disable();
        shopAction.Disable();
    }

}
