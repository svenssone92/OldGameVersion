using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SarekoTopDownMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] private float stairClimb = 0.8f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public static Vector2 facingDirection;
    public static Vector2 startingPosition = new Vector2(6f, -39f);
    public static bool canMove = true;
    public static bool onStairs = false;

    [Header("Outhers")]
    [SerializeField] private LayerMask Interactable;
    [SerializeField] private GameObject menu;

    [Header("Audios")]
    [SerializeField] private AudioSource bush;
    [SerializeField] private AudioSource bedroom;

    private Animator animator;



    // Animation state names
    private const string GoDown = "GoDown";
    private const string GoLeft = "GoLeft";
    private const string GoRight = "GoRight";
    private const string GoUp = "GoUp";
    private const string IdleDown = "IdleDown";
    private const string IdleLeft = "IdleLeft";
    private const string IdleRight = "IdleRight";
    private const string IdleUp = "IdleUp";

    // Current animation state
    private string currentState;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        EntryPosition();
        EntrySound();
    }

    void Update()
    {
        if (DialogManager.GetInstance().dialogIsPlaying)
        {
            return;
        }

        UpdateSarekosDirection();


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }

        if (canMove)
        {
            // Input
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            if (onStairs)
            {
                moveInput = new Vector2(horizontalInput, verticalInput);


                if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
                {
                    if (horizontalInput > 0)
                    {
                        moveInput = new Vector2(horizontalInput, -stairClimb);
                    }
                    else
                    {
                        moveInput = new Vector2(horizontalInput, stairClimb);
                    }
                }
                else
                {
                    moveInput = new Vector2(0f, verticalInput);
                }
            }
            else
            {
                moveInput = new Vector2(horizontalInput, verticalInput);


                if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
                {
                    moveInput.y = 0f;
                }
                else
                {
                    moveInput.x = 0f;
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                TryInteract();
            }
        }
    }

    void FixedUpdate()
    {

        if (!DialogManager.GetInstance().dialogIsPlaying)
        {

        Vector2 moveVelocity = moveInput.normalized * moveSpeed;
        rb.velocity = new Vector2(moveVelocity.x, moveVelocity.y*0.8f);

        }

        UpdateAnimator();
    }


    public void WalkBack()
    {
        StartCoroutine(WalkBackCoroutine());
    }

    public void LetMove()
    {
        canMove = true;
    }

    private IEnumerator WalkBackCoroutine()
    {
        canMove = false;

        Debug.Log("WalkBack coroutine started");

        moveInput = -facingDirection;

        yield return new WaitForSeconds(0.2f);

        // Reset movement input
        moveInput = Vector2.zero;

        Debug.Log("WalkBack coroutine ended");
    }

    private void ToggleMenu()
    {
        menu.SetActive(!menu.activeSelf);
    }

    void TryInteract()
    {
        Vector2 rayDirection = Vector2.zero;

        if (currentState == GoRight || currentState == IdleRight)
            rayDirection = Vector2.right;
        else if (currentState == GoLeft || currentState == IdleLeft)
            rayDirection = Vector2.left;
        else if (currentState == GoUp || currentState == IdleUp)
            rayDirection = Vector2.up;
        else if (currentState == GoDown || currentState == IdleDown)
            rayDirection = Vector2.down;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, 1f, Interactable);

        if (hit.collider != null)
        {
            Debug.Log("Interacted with: " + hit.collider.name);

            hit.collider.GetComponent<IInteractables>()?.Interact();
        }
    }


    void UpdateAnimator()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            if (rb.velocity.x > 0.1f)
                UpdateAnimationState(GoRight);
            else if (rb.velocity.x < -0.1f)
                UpdateAnimationState(GoLeft);
            else if (rb.velocity.y > 0.1f)
                UpdateAnimationState(GoUp);
            else if (rb.velocity.y < -0.1f)
                UpdateAnimationState(GoDown);
        }
        else
        {
            UpdateAnimationState(IdleStateForCurrentDirection());
        }
    }

    private void UpdateSarekosDirection()
    {
        if (currentState == GoRight || currentState == IdleRight)
            facingDirection = Vector2.right;
        else if (currentState == GoLeft || currentState == IdleLeft)
            facingDirection = Vector2.left;
        else if (currentState == GoUp || currentState == IdleUp)
            facingDirection = Vector2.up;
        else if (currentState == GoDown || currentState == IdleDown)
            facingDirection = Vector2.down;
    }

    private string IdleStateForCurrentDirection()
    {
        if (currentState == GoRight || currentState == IdleRight)
            return IdleRight;
        else if (currentState == GoLeft || currentState == IdleLeft)
            return IdleLeft;
        else if (currentState == GoUp || currentState == IdleUp)
            return IdleUp;
        else if (currentState == GoDown || currentState == IdleDown)
            return IdleDown;

        return IdleDown;
    }

    private void UpdateAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }

    private void EntrySound()
    {
        if (startingPosition == new Vector2(6f, -39f))
        {
            bedroom.Play();
        }
        else if (startingPosition == new Vector2(17f, 4f))
        {
            bush.Play();
        }

    }

    private void EntryPosition()
    {
        if (startingPosition == new Vector2(6f, -39f) || startingPosition == new Vector2(17f, 4f))
        {
            rb.position = startingPosition;
        }
        else { rb.position = new Vector2(14f, -1f); }
    }
}
