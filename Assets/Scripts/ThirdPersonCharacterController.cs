using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


//this script is gonna be so fucking long
public class ThirdPersonCharacterController : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private BugSpawner _bugSpawner;

    [SerializeField] private GameObject adaptationsShop;
    [SerializeField] private GameObject jumpBoostButton;
    [SerializeField] private GameObject speedBoostButton;
    [SerializeField] private GameObject smallSizeButton;
    [SerializeField] private GameObject largeSizeButton;
    [SerializeField] private GameObject moreHealthButton;
    [SerializeField] private GameObject fasterHealingButton;
    [SerializeField] private GameObject largeAttackButton;
    [SerializeField] private GameObject fasterAttackButton;

    [SerializeField] private GameObject maxAdaptationErrorUI;

    [SerializeField] private int maxAdaptations = 2;
    private int currentAdaptations;

    private UICurrency _UICurrency;

    //Adaptation costs
    private int jumpBoostCost = 100;
    private bool isJumpBoostOwned;
    private int speedBoostCost = 100;
    private bool isSpeedBoostOwned;
    private int smallerSizeCost = 100;
    private bool isSmallerSizeOwned;
    private int largerSizeCost = 100;
    private bool isLargerSizeOwned;
    private int moreHealthCost = 100;
    private bool isMoreHealthOwned;
    private int fasterHealingCost = 100;
    private bool isFasterHealingOwned;
    private int largerAttackCost = 100;
    private bool isLargerAttackOwned;
    private int fasterAttackCost = 100;
    private bool isFasterAttackOwned;


    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;
    private float currentWalkSpeed;

    [Header("Speed Adaptation Parameters")]
    [SerializeField] private float speedBoostMultiplier;
    [SerializeField] private bool isSpeedBoostActive;

    [Header("Jump Parameters")]
    [SerializeField] private float groundDistance = 0.6f; // Distance to check for ground
    public LayerMask groundMask; // Define ground layer
    private bool isGrounded;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] public float gravity = -9.81f;
    [SerializeField] private float coyoteTimeDuration = 0.2f; // Time window for coyote time
    private float coyoteTimeCounter;       // Timer for coyote time

    [Header("Jump Adaptation Parameters")]
    [SerializeField] private float jumpBoostMultiplier;
    [SerializeField] private bool isJumpBoostActive;

    [Header("Health Adaptation Parameters")]
    [SerializeField] public int newMaxHealth;
    [SerializeField] private bool isMoreHealthActive;
    [SerializeField] public float healingSpeedMultiplier;
    [SerializeField] private bool isFasterHealingActive;

    [Header("Attack Parameters")]
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private bool isAttacking;

    [Header("Attack Adaptation Parameters")]
    [SerializeField] private float attackSizeMultiplier;
    [SerializeField] private bool isLargerAttackActive;
    [SerializeField] private float attackSpeedMultiplier;
    [SerializeField] private bool isFasterAttackActive;

    [Header("Misc. Adaptations")]
    [SerializeField] private bool isSmallerSizeActive;
    [SerializeField] private float smallSizeMultiplier;
    [SerializeField] private bool isLargerSizeActive;
    [SerializeField] private float largeSizeMultiplier;
    [SerializeField] private int totalCurrency;
    [SerializeField] private bool isInDen; //only serialized for debugging
    private bool isAbleToShop = true;
    private bool isShopping = false;

    [Header("Camera Parameters")]
    public Transform cam;
    [SerializeField] private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public CharacterController characterController;
    private Camera mainCamera;
    private PlayerInputHandler inputHandler;

    private Vector3 currentVelocity;

    //Audio variables 
    [Header("Audio")]
    public AudioSource coinGetPlayer;
    public AudioSource enemyTakeDamagePlayer;
    public AudioSource jumpPlayer;
    public AudioSource highJumpPlayer;
    private void Awake()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        _bugSpawner = GameObject.Find("BugSpawner").GetComponent<BugSpawner>();

        attackSpeedMultiplier = 1;
        healingSpeedMultiplier = 1;
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        
        UnityEngine.Cursor.visible = false;
        _UICurrency = GameObject.Find("CurrencyText").GetComponent<UICurrency>();
    }

    private void Start()
    {
        attackPrefab.transform.localScale = attackPrefab.transform.localScale;

        inputHandler = PlayerInputHandler.Instance;

        // Check if groundMask is not set
        if (groundMask == 0)
        {
            Debug.LogError("GroundCheck: GroundMask is not set! Please assign a ground layer in the Inspector.", this);
        }
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance, groundMask);
        HandleCamera();
        HandleMovement();
        HandleShopping();
    }

    void HandleMovement()
    {
        //calls jumping method
        HandleJumping();


        float speed = walkSpeed;
        if (isSpeedBoostActive)
        {
            //makes player faster
            speed = walkSpeed * speedBoostMultiplier;
        }
        else if (isShopping == true)
        {
            speed = 0;
        }
        else
            speed = walkSpeed;

        Vector3 inputDirection = new Vector3(inputHandler.MoveInput.x, 0f, inputHandler.MoveInput.y).normalized; //MoveInput.y because it's a vector 2 so the y is actually the z

        if (inputDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //makes player move
            characterController.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        //calls attack method
        if (inputHandler.AttackTriggered)
        {
            OnAttack();
        }
        

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Den")
        {
            isInDen = true;
        }

        if (other.gameObject.tag == "Currency")
        {
            Destroy(other.gameObject);
            totalCurrency += 100;
            Debug.Log(totalCurrency);
            _UICurrency.UpdateCurrency(totalCurrency);
            _bugSpawner.CurrencyCheck(totalCurrency);
            coinGetPlayer.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Den")
        {
            isInDen = false;
            adaptationsShop.SetActive(false);
            isAbleToShop = true;
        }
    }


    void HandleJumping()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTimeDuration; // Reset coyote time if grounded
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // Decrease timer if not grounded
        }

        //checks if player is on the ground
        if (isGrounded && coyoteTimeCounter > 0)
        {
            print("ground");
            currentVelocity.y = -2f;
            //checks if player pressed jump
            if (inputHandler.JumpTriggered)
            {
                print("Jumping");
                if (isJumpBoostActive == true)
                {
                    //makes player jump higher
                    currentVelocity.y = jumpForce * jumpBoostMultiplier;
                    highJumpPlayer.Play();
                }
                else
                {
                    //makes player jump
                    currentVelocity.y = jumpForce;
                    jumpPlayer.Play();
                }
            }
            coyoteTimeCounter = 0; // Prevent multiple jumps in air

        }
        else
        {
            //gravity 
            currentVelocity.y -= gravity * Time.deltaTime;
        }

        characterController.Move(currentVelocity * Time.deltaTime);
    }



    private void OnAttack()
    {
        //checks if player is already attacking or not
        if (isAttacking == false)
        {
            //attack is instantiated
            Instantiate(attackPrefab, transform);
            isAttacking = true; //set to true to prevent multiple attacks at once
            enemyTakeDamagePlayer.Play();
            //starts attack cooldown coroutine
            StartCoroutine(AttackCooldownCoroutine());
        }
    }

    private IEnumerator AttackCooldownCoroutine()
    {
        //timer to prevent player from spamming attack
        yield return new WaitForSeconds(0.8f / attackSpeedMultiplier);
        isAttacking = false;
    }

    /*
     * SHOPPING AND CURRENCY CODE STARTED
     */

    void HandleShopping()
    {
        //checks if shop button was pressed and if player is in a den
        if (inputHandler.ShopTriggered == true && isInDen)
        {
            //turns on shop if not open
            if (adaptationsShop.activeSelf == false && isAbleToShop == true)
            {
                UnityEngine.Cursor.visible = true;
                
                //stops players from moving in the den
                isShopping = true;
                
                adaptationsShop.SetActive(true);
                //prevents shop from flickering on and off constantly
                StartCoroutine(ShopCooldownCoroutine());
            }
            //turns off shop if open
            else if (adaptationsShop.activeSelf == true && isAbleToShop == false)
            {
                UnityEngine.Cursor.visible = false;
                adaptationsShop.SetActive(false);
                
                //Allows players to move again while in the den
                isShopping = false;
                
                //prevents shop from flickering on and off constantly
                StartCoroutine(ShopCooldownCoroutine());
            }
        }
    }

    private IEnumerator ShopCooldownCoroutine()
    {

        //timer to prevent player from spamming shops
        yield return new WaitForSeconds(0.2f);
        if (isAbleToShop == true)
        {
            isAbleToShop = false;

        }
        else
        {
            isAbleToShop = true;
        }
    }

    void HandleCamera()
    {
        if (isShopping == true)
        {
            mainCamera.GetComponent<CinemachineBrain>().enabled = false;
        }
        if (isShopping == false)
        {
            mainCamera.GetComponent<CinemachineBrain>().enabled = true;
        }
    }


    /*
    * ADAPTATIONS CODE STARTED
    */

    //Adaptation methods follow a very similar format, will explain everything in jumpboost method, and only comment on unique things in other adaptation methods
   
    public void JumpBoostButtonHandler()
    {
        //check if the player has enough currency to buy the adaptation
        //check if the player doesn't own the adaptation
        //check if the player hasn't reached max adaptations
        if (jumpBoostCost <= totalCurrency && isJumpBoostOwned == false && currentAdaptations < maxAdaptations)
        {
            totalCurrency -= jumpBoostCost; //subtract the cost from the total currency
            isJumpBoostOwned = true; //give ownership to player
        }
        if (isJumpBoostOwned == true)
        {
            if (isJumpBoostActive == false) //if adaptation wasn't active
            {
                isJumpBoostActive = true; //set it to active because now the player owns it
                jumpBoostButton.SetActive(true); //displays adaptation icon 
                currentAdaptations++; //increases adaptation count
            }
            else if (isJumpBoostActive == true) //if they clicked on the button while they own the adaptation already (selling the adaptation)
            {
                isJumpBoostActive = false; //deactivate adaptation
                jumpBoostButton.SetActive(false); //remove icon
                totalCurrency += jumpBoostCost / 2; //give player half the money back
                isJumpBoostOwned = false; //player no longer owns the adaptation
                currentAdaptations--; //decrease adaptation count
            }
        }
        else if (currentAdaptations >= maxAdaptations) //if max adaptations reached
        {
            StartCoroutine(MaxAdaptationCoroutine()); //start error coroutine
        }
        _UICurrency.UpdateCurrency(totalCurrency); //updates the currency in the UI
        _bugSpawner.CurrencyCheck(totalCurrency);

    }

    public void SpeedBoostButtonHandler()
    {
        if (speedBoostCost <= totalCurrency && isSpeedBoostOwned == false && currentAdaptations < maxAdaptations)
        {
            totalCurrency -= speedBoostCost;
            isSpeedBoostOwned = true;
        }
        if (isSpeedBoostOwned == true)
        {
            if (isSpeedBoostActive == false)
            {
                isSpeedBoostActive = true;
                speedBoostButton.SetActive(true);
                currentAdaptations++;
            }
            else if (isSpeedBoostActive == true)
            {
                isSpeedBoostActive = false;
                speedBoostButton.SetActive(false);
                totalCurrency += speedBoostCost / 2;
                isSpeedBoostOwned = false;
                currentAdaptations--;
            }
        }
        else if (currentAdaptations >= maxAdaptations)
        {
            StartCoroutine(MaxAdaptationCoroutine());
        }
        _UICurrency.UpdateCurrency(totalCurrency);
        _bugSpawner.CurrencyCheck(totalCurrency);

    }

    public void SmallerSizeButtonHandler()
    {
        if (smallerSizeCost <= totalCurrency && isSmallerSizeOwned == false && currentAdaptations < maxAdaptations)
        {
            totalCurrency -= smallerSizeCost;
            isSmallerSizeOwned = true;
        }

        if (isSmallerSizeOwned == true)
        {

            if (isSmallerSizeActive == false)
            {
                isSmallerSizeActive = true;
                smallSizeButton.SetActive(true);
                this.transform.localScale = transform.localScale * smallSizeMultiplier; //makes player smaller
                currentAdaptations++;
            }
            else if (isSmallerSizeActive == true)
            {
                isSmallerSizeActive = false;
                smallSizeButton.SetActive(false);
                this.transform.localScale = transform.localScale /smallSizeMultiplier; //returns player to normal size
                totalCurrency += smallerSizeCost / 2;
                isSmallerSizeOwned = false;
                currentAdaptations--;
            }

        }
        else if(currentAdaptations >= maxAdaptations)
        {
            StartCoroutine(MaxAdaptationCoroutine());
        }
        _UICurrency.UpdateCurrency(totalCurrency);
        _bugSpawner.CurrencyCheck(totalCurrency);

    }

    public void LargerSizeButtonHandler()
    {
        if (largerSizeCost <= totalCurrency && isLargerSizeOwned == false && currentAdaptations < maxAdaptations)
        {
            totalCurrency -= largerSizeCost;
            isLargerSizeOwned = true;
        }

        if (isLargerSizeOwned == true)
        {

            if (isLargerSizeActive == false)
            {
                isLargerSizeActive = true;
                largeSizeButton.SetActive(true);
                this.transform.localScale = transform.localScale * largeSizeMultiplier; //makes player larger
                currentAdaptations++;
            }
            else if (isLargerSizeOwned == true)
            {
                isLargerSizeActive = false;
                largeSizeButton.SetActive(false);
                this.transform.localScale = transform.localScale/largeSizeMultiplier; //returns player to normal size
                totalCurrency += smallerSizeCost / 2;
                isLargerSizeOwned = false;
                currentAdaptations--;
            }

        }
        else if (currentAdaptations >= maxAdaptations)
        {
            StartCoroutine(MaxAdaptationCoroutine());
        }
        _UICurrency.UpdateCurrency(totalCurrency);
        _bugSpawner.CurrencyCheck(totalCurrency);

    }

    public void MoreHealthButtonHandler()
    {
        if (moreHealthCost <= totalCurrency && isMoreHealthOwned == false && currentAdaptations < maxAdaptations)
        {
            totalCurrency -= moreHealthCost;
            isMoreHealthOwned = true;
        }

        if (isMoreHealthOwned == true)
        {

            if (isMoreHealthActive == false)
            {
                isMoreHealthActive = true;
                moreHealthButton.SetActive(true);
                playerHealth.HealthCheck(newMaxHealth); //sends float to the HealthCheck() method in the PlayerHealth script
                currentAdaptations++;
            }
            else if (isMoreHealthOwned == true)
            {
                isMoreHealthActive = false;
                moreHealthButton.SetActive(false);
                playerHealth.HealthCheck(-newMaxHealth); //sends negative float, to revert back to base health
                totalCurrency += moreHealthCost / 2;
                isMoreHealthOwned = false;
                currentAdaptations--;
            }

        }
        else if (currentAdaptations >= maxAdaptations)
        {
            StartCoroutine(MaxAdaptationCoroutine());
        }
        _UICurrency.UpdateCurrency(totalCurrency);
        _bugSpawner.CurrencyCheck(totalCurrency);

    }

    public void FasterHealingButtonHandler()
    {
        if (fasterHealingCost <= totalCurrency && isFasterHealingOwned == false && currentAdaptations < maxAdaptations)
        {
            totalCurrency -= fasterHealingCost;
            isFasterHealingOwned = true;
        }

        if (isFasterHealingOwned == true)
        {

            if (isFasterHealingActive == false)
            {
                isFasterHealingActive = true;
                fasterHealingButton.SetActive(true);
                healingSpeedMultiplier = healingSpeedMultiplier * 1.5f; //makes health regen faster
                currentAdaptations++;
            }
            else if (isFasterHealingOwned == true)
            {
                isFasterHealingActive = false;
                fasterHealingButton.SetActive(false);
                healingSpeedMultiplier = healingSpeedMultiplier / 1.5f; //returns health regen to normal
                totalCurrency += fasterHealingCost / 2;
                isFasterHealingOwned = false;
                currentAdaptations--;
            }

        }
        else if (currentAdaptations >= maxAdaptations)
        {
            StartCoroutine(MaxAdaptationCoroutine());
        }
        _UICurrency.UpdateCurrency(totalCurrency);
        _bugSpawner.CurrencyCheck(totalCurrency);

    }

    public void LargerAttackButtonHandler()
    {
        if (largerAttackCost <= totalCurrency && isLargerAttackOwned == false && currentAdaptations < maxAdaptations)
        {
            totalCurrency -= largerAttackCost;
            isLargerAttackOwned = true;
        }
        if (isLargerAttackOwned == true)
        {
            if (isLargerAttackActive == false)
            {
                isLargerAttackActive = true;
                largeAttackButton.SetActive(true);
                attackPrefab.transform.localScale = transform.localScale * attackSizeMultiplier; //makes attack bigger
                currentAdaptations++;
            }
            else if (isLargerAttackActive == true)
            {
                isLargerAttackActive = false;
                largeAttackButton.SetActive(false);
                attackPrefab.transform.localScale = attackPrefab.transform.localScale / attackSizeMultiplier; //returns attack to base size
                totalCurrency += largerAttackCost/ 2;
                isLargerAttackOwned = false;
                currentAdaptations--;
            }
        }
        else if (currentAdaptations >= maxAdaptations)
        {
            StartCoroutine(MaxAdaptationCoroutine());
        }
        _UICurrency.UpdateCurrency(totalCurrency);
        _bugSpawner.CurrencyCheck(totalCurrency);

    }

    public void FasterAttackButtonHandler()
    {
        if (fasterAttackCost <= totalCurrency && isFasterAttackOwned == false && currentAdaptations < maxAdaptations)
        {
            totalCurrency -= fasterAttackCost;
            isFasterAttackOwned = true;
        }
        if (isFasterAttackOwned == true)
        {
            if (isFasterAttackActive == false)
            {
                isFasterAttackActive = true;
                fasterAttackButton.SetActive(true);
                attackSpeedMultiplier = attackSpeedMultiplier * 1.5f; //makes attack faster
                currentAdaptations++;
            }
            else if (isFasterAttackActive == true)
            {
                isFasterAttackActive = false;
                fasterAttackButton.SetActive(false);
                attackSpeedMultiplier = attackSpeedMultiplier / 1.5f; //returns attack to base speed
                totalCurrency += fasterAttackCost / 2;
                isFasterAttackOwned = false;
                currentAdaptations--;
            }
        }
        else if (currentAdaptations >= maxAdaptations)
        {
            StartCoroutine(MaxAdaptationCoroutine());
        }
        _UICurrency.UpdateCurrency(totalCurrency);
        _bugSpawner.CurrencyCheck(totalCurrency);

    }

    IEnumerator MaxAdaptationCoroutine()
    {
        maxAdaptationErrorUI.SetActive(true); //display error message
        yield return new WaitForSeconds(1f);
        maxAdaptationErrorUI.SetActive(false); //turn of error message

    }
}
