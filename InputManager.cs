using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    Animator animator;
    AnimatorManager animatorManager;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;

    [Header("Inputs")]
    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("Bools")]
    public bool isInteracting;
    public bool comboDoing;

    public bool sprintInput;
    public bool jumpInput;
    public bool dodgeInput;
    public bool laInput; //Light Attack input
    public bool haInput; //heavy attack input

    private void Awake()
    {
        Cursor.visible = false;
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animator = GetComponent<Animator>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
    }

    private void Update()
    {
        isInteracting = animator.GetBool("isInteracting");
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.Movement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.Movement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.Actions.Sprint.performed += i => sprintInput = true;
            playerControls.Actions.Sprint.canceled += i => sprintInput = false;

            playerControls.Actions.Jump.performed += i => jumpInput = true;
            playerControls.Actions.Dodge.performed += i => dodgeInput = true;

            playerControls.Actions.LA.performed += i => laInput = true;
            playerControls.Actions.HA.performed += i => haInput = true;

        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        MovementInput();
        SprintingInput();
        JumpingInput();
        DodgeInput();
        AttackInput();
    }

    private void MovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
    }

    private void SprintingInput()
    {
        if (sprintInput && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    private void JumpingInput()
    {
        if (jumpInput)
        {
            jumpInput = false;
            playerLocomotion.Jumping();
        }
    }

    private void DodgeInput()
    {
        if (dodgeInput)
        {
            dodgeInput = false;
            playerLocomotion.Dodge();
        }
    }

    private void AttackInput()
    {
        //LA Input handles right hand light attacks
        if (laInput)
        {
            if (playerManager.canDoCombo)
            {
                comboDoing = true;
                playerAttacker.HandleWeaponCombo(playerInventory.handWeapon);
                comboDoing = false;
            }
            else 
            {
                if (playerManager.isInteracting) return;
                if (playerManager.canDoCombo) return;
                playerAttacker.HandleLightAttack(playerInventory.handWeapon);
            }

            laInput = false;
        }

        //HA input handles right hand light attacks
        if (haInput)
        {
            if (playerManager.isInteracting)
                return;
            playerAttacker.HandleHeavyAttack(playerInventory.handWeapon);
            haInput = false;
        }
    }

}
