using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer sprite;

    public static bool canMove = true;

    private bool isEmoteing;

    private static float dirX = 0;
    private float lockedTill;
    private static bool isFacingRight = true;
    public static Vector2 CheckPointPosition;

    private float gravityScale = 4;
    private float fallGravityMult = 1.2f;
    public float maxFastFallSpeed;

    private bool isJumping;
    private bool isFalling;
    private float jumpBufferTime = 0.1f;
    private float jumpBufferEndTime;

    private bool isWallSliding;
    private float wallSlidingSpeed = 0f;
    private float climbSpeed = 6f;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 20f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.1f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 17f);

    private bool isAttacking = false;
    private bool isHighAttacking = false;
    private bool canAttack = true;
    private Vector2 attackLBox = new Vector2(1.6f, 0.864f);
    private Vector2 highAttackHBox = new Vector2(0.64f, 1.28f);
    private Vector2 highAttackLBox = new Vector2(0.432f, 1.6f);

    private float attackAnimationTime = 0.35f;

    private Vector3 groundSheckSize = new Vector3(0.41f, 0.06f, 0f);


    //Checks
    private bool isGrounded;
    private bool isWalled;
    private bool hasEdge;
    public static bool isReSpawning;
    public static bool doAReSpawn =false;

    //Inputs
    private static bool isJumpPressed = false;
    private static bool isJumpReleased = false;
    private static bool isDashPressed = false;
    private static bool isAttackPressed = false;
    private static bool isHighAttackPressed = false;
    private static bool isSwapingPressed = false;
    private static bool isEmotePressed = false;
    public static bool isZoomPressed = false;

    private Vector2 sarekoColliderSize;
    private float catColliderOffset = -0.35f;

    [Header("Movement Parameters")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    [Header("Attack Parameters")]
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackSpeed = 0.5f;
    [SerializeField] private Transform attackHighPoint;
    [SerializeField] private Transform attackLowPoint;
    [SerializeField] private Transform highAttackHighPoint;
    [SerializeField] private Transform highAttackLowPoint;

    [Header("CheckPoints")]
    [SerializeField] private Transform wallCheckTop;
    [SerializeField] private Transform wallCheckHigh;
    [SerializeField] private Transform wallCheckLow;
    [SerializeField] private Transform groundCheckPoint;

    [Header("LayerMasks")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource jumpBreath;
    [SerializeField] private AudioSource attackBreath;
    [SerializeField] private AudioSource dashSound;
    [SerializeField] private AudioSource swapSound;
    [SerializeField] private AudioSource[] footSteps;


    [Header("Others")]
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject StaringPoint;

    #region Animation States
    //Animation States
    private int currentState;

    private static readonly int sarekoIdle = Animator.StringToHash("Sareko_Idle");
    private static readonly int sarekoRunning = Animator.StringToHash("Sareko_Running");
    private static readonly int sarekoJump = Animator.StringToHash("Sareko_Jump");
    private static readonly int sarekoFalling = Animator.StringToHash("Sareko_Falling");
    private static readonly int sarekoWallGlide = Animator.StringToHash("Sareko_WallGlide");
    private static readonly int sarekoKick = Animator.StringToHash("Sareko_Kick");
    private static readonly int sarekoHighKick = Animator.StringToHash("Sareko_HighKick");
    private static readonly int sarekoDash = Animator.StringToHash("Sareko_Dash");
    private static readonly int sarekoEmote = Animator.StringToHash("Sareko_Emote");
    private static readonly int sarekoWallJump = Animator.StringToHash("Sareko_WallJump");
    private static readonly int sarekoMidAir = Animator.StringToHash("Sareko_MidAir");
    private static readonly int sarekoReSpawn = Animator.StringToHash("Sareko_ReSpawn");

    private static readonly int playerSwap = Animator.StringToHash("Player_Swap");

    private static readonly int catIdle = Animator.StringToHash("Cat_Idle");
    private static readonly int catRunning = Animator.StringToHash("Cat_Running");
    private static readonly int catJump = Animator.StringToHash("Cat_Jump");
    private static readonly int catFalling = Animator.StringToHash("Cat_Falling");
    private static readonly int catWallGlide = Animator.StringToHash("Cat_WallGlide");
    private static readonly int catClimb = Animator.StringToHash("Cat_Climb");
    #endregion

    public enum SpiritState
    {
        Sareko,
        Cat
    }
    SpiritState spirit = SpiritState.Sareko;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        sarekoColliderSize = coll.size;
        CheckPointPosition = StaringPoint.transform.position;

        isFacingRight = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing || !canMove) return;
        GatherInput();
    }

    private void GatherInput()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Escape)) ToggleMenu();


        if (Input.GetButtonDown("SpiritSwap")) isSwapingPressed = true;

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferEndTime = Time.time + jumpBufferTime;
            isJumpPressed = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            isJumpReleased = true;
        }

        if (!isWallSliding)
        {
            if (Input.GetButtonDown("x") && Input.GetButton("ArrowUp") && !isHighAttacking && spirit == SpiritState.Sareko)
            {
                isHighAttackPressed = true;
            }
            else if (Input.GetButtonDown("x") && !Input.GetButton("ArrowUp") && !isAttacking && spirit == SpiritState.Sareko)
            {
                isAttackPressed = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.C)) isDashPressed = true;

        if (Input.GetKeyDown(KeyCode.E) && dirX == 0)
        {
            isEmotePressed = true;
        }

        if (Input.GetKeyDown(KeyCode.R) && dirX == 0)
        {
            if (isZoomPressed == true)
            {
                isZoomPressed = false;
            }
            else { isZoomPressed = true; }
        }
    }

    private void FixedUpdate()
    {
        if (doAReSpawn)
        {
            if(spirit == SpiritState.Cat)
            {
                SpiritSwap();
            }
            InputReset();
            doAReSpawn = false;
        }

        hasEdge = HasEdge();
        isGrounded = IsGrounded();
        isWalled = IsWalled();

        ClimbEdge();
        WallSlide();

        IsJumpingOrFalling();
        UpdateGravity();

       

        if (!isGrounded && !isWalled) isJumpPressed = false;

        if (!canDash || isWallSliding) isDashPressed = false;

        if (spirit == SpiritState.Sareko) WallJump();

        else WallClimb();

        if (isAttackPressed && canAttack)
        {
            Attack();
        }

        if (isHighAttackPressed && canAttack)
        {
            HighAttack();
        }

        if (isEmotePressed)
        {
            //Emote();
        }

        if (isDashPressed && spirit == SpiritState.Sareko) StartCoroutine(Dash());
        DashAnEdge();

        if (isSwapingPressed && !isAttacking && !isHighAttacking) SpiritSwap();

        if (Time.time < jumpBufferEndTime && isGrounded)
        {
            Jump();
        }

        if (!isWallJumping) Flip();

        if (!isWallJumping && !isDashing) rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        UpdateAnimation();
    }

    private void IsJumpingOrFalling()
    {
        if (isAttacking || isHighAttacking)
        {
            isAttackPressed = false;
            isHighAttackPressed = false;
        }

        if (isJumping && rb.velocity.y <= 0)
        {
            isJumping = false;
            isFalling = true;
        }
        if (isWalled || isGrounded)
        {
            isFalling = false;
            isJumpReleased = false;
        }
    }

    private void UpdateGravity()
    {
        if (isFalling && !isDashing && !isWallJumping)
        {
            isJumpReleased = false;
            rb.gravityScale = gravityScale * fallGravityMult;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFastFallSpeed));
        }
        else if (!isDashing)
        {
            rb.gravityScale = gravityScale;
        }
        if (isJumping && isJumpReleased && !isWallJumping)
        {
            rb.gravityScale = gravityScale * 5;
        }
    }
    #region Attack Logic
    private void Attack()
    {
        if (!isAttacking)
        {
            if (isAttackPressed)
            {
                canAttack = false;
                isAttacking = true;
                Collider2D[] hitHEnimies = Physics2D.OverlapCircleAll(attackHighPoint.position, 0.368f, enemyLayer);
                Collider2D[] hitLEnimies = Physics2D.OverlapBoxAll(attackLowPoint.position, highAttackLBox, 26.8f, enemyLayer);

                foreach (Collider2D enemy in hitHEnimies)
                {
                    Debug.Log("we hit " + enemy.name);

                    Vector2 attackDirection = enemy.transform.position - transform.position;
                    Health enemyHealth = enemy.GetComponent<Health>();
                    if (enemyHealth != null)
                    {
                        enemy.GetComponent<Health>().TakeDamage(attackDamage, attackDirection);
                    }
                }
                foreach (Collider2D enemy in hitLEnimies)
                {
                    if (!hitHEnimies.Contains(enemy))
                    {
                        Debug.Log("we hit " + enemy.name);

                        Vector2 attackDirection = enemy.transform.position - transform.position;
                        Health enemyHealth = enemy.GetComponent<Health>();
                        if (enemyHealth != null)
                        {
                            enemy.GetComponent<Health>().TakeDamage(attackDamage, attackDirection);
                        }
                    }
                }
                attackBreath.Play();
                StartCoroutine(AttackTime());
                StartCoroutine(AttackCoolDown());
            }
            isAttackPressed = false;
        }
    }

    private void HighAttack()
    {
        if (!isHighAttacking)
        {
            if (isHighAttackPressed)
            {
                canAttack = false;
                isHighAttacking = true;
                Collider2D[] hitHEnimies = Physics2D.OverlapCapsuleAll(highAttackHighPoint.position, highAttackHBox, CapsuleDirection2D.Vertical, 12f, enemyLayer);
                Collider2D[] hitLEnimies = Physics2D.OverlapBoxAll(highAttackLowPoint.position, highAttackLBox, 0f, enemyLayer);

                foreach (Collider2D enemy in hitHEnimies)
                {
                    Debug.Log("we hit " + enemy.name);

                    Vector2 attackDirection = enemy.transform.position - transform.position;
                    Health enemyHealth = enemy.GetComponent<Health>();
                    if(enemyHealth != null)
                    {
                        enemy.GetComponent<Health>().TakeDamage(attackDamage, attackDirection);
                    }
                }
                foreach (Collider2D enemy in hitLEnimies)
                {
                    if (!hitHEnimies.Contains(enemy))
                    {
                        Debug.Log("we hit " + enemy.name);

                        Vector2 attackDirection = enemy.transform.position - transform.position;
                        Health enemyHealth = enemy.GetComponent<Health>();
                        if (enemyHealth != null)
                        {
                            enemy.GetComponent<Health>().TakeDamage(attackDamage, attackDirection);
                        }
                    }
                }
                attackBreath.Play();
                StartCoroutine(HighAttackTime());
                StartCoroutine(AttackCoolDown());
            }
            isHighAttackPressed = false;
        }
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(attackSpeed);

        canAttack = true;
    }
    #endregion

    #region Animation Logic
    private void UpdateAnimation()
    {
        var state = GetState();

        isJumpPressed = false;
        isDashPressed = false;

        if (state == currentState) return;
        anim.CrossFade(state, 0, 0);
        currentState = GetState();
    }

    private int GetState()
    {
        // Order by prioritie
        if (spirit == SpiritState.Sareko)
        {
            if (isReSpawning) return sarekoReSpawn;
            if (isHighAttacking) return sarekoHighKick;
            if (isAttacking) return sarekoKick;
            if (isWallSliding) return sarekoWallGlide;
            if (isDashing) return sarekoDash;
            if (isWallJumping) return sarekoWallJump;
            if (isJumpPressed) return sarekoJump;
            if (isEmoteing) return sarekoEmote;
            if (isGrounded) return dirX == 0 ? sarekoIdle : sarekoRunning;
            if (rb.velocity.y < -0f) return sarekoFalling;
            return sarekoJump;
        }
        else
        {
            if (isWallSliding && rb.velocity.y > 0f) return catClimb;
            if (isWallSliding) return catWallGlide;
            if (isJumpPressed) return catJump;
            if (isGrounded) return dirX == 0 ? catIdle : catRunning;
            return rb.velocity.y > 0 ? catJump : catFalling;
        }
    }

    private void Emote() //For fun
    {
        if (!isEmoteing)
        {
            isEmotePressed = false;
            isEmoteing = true;
            StartCoroutine(EmoteTime());
        }
    }

    private IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(attackAnimationTime);

        isAttacking = false;
    }

    private IEnumerator HighAttackTime()
    {
        yield return new WaitForSeconds(attackAnimationTime);

        isHighAttacking = false;
    }

    private IEnumerator EmoteTime()
    {
        yield return new WaitForSeconds(3.5f);

        isEmoteing = false;
    }
    #endregion

    private void SpiritSwap() //Swaps between Sareko and the cat.
    {
        isSwapingPressed = false;
        swapSound.Play();
        spirit = (spirit == SpiritState.Sareko) ? SpiritState.Cat : SpiritState.Sareko;

        if (spirit == SpiritState.Cat)
        {
            coll.size = new Vector2(0.4f, 0.3f);
            coll.offset += new Vector2(0f, catColliderOffset);

            wallCheckHigh.localPosition = new Vector2(0.19f, -0.10f);
        }
        else
        {
            coll.size = sarekoColliderSize;
            coll.offset = Vector2.zero;

            wallCheckHigh.localPosition = new Vector2(0.19f, 0.54f);
        }
    }

    private void ToggleMenu()
    {
        menu.SetActive(!menu.activeSelf);
    }

    private void Flip()
    {
        if (isFacingRight && dirX < 0f || !isFacingRight && dirX > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void InputReset()
    {
        dirX = 0f;
        isJumpPressed = false;
        isJumpReleased = false;
        isDashPressed = false;
        isAttackPressed = false;
        isHighAttackPressed = false;
        isSwapingPressed = false;
        isEmotePressed = false;
        if (!isFacingRight)
        {
            isFacingRight = true;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void hasReSpawned()
    {
        isReSpawning = false;
        canMove = true;
    }

    #region Movement Logic

    private void Jump()
    {
        isJumping = true;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        if (spirit == SpiritState.Sareko) jumpBreath.Play();
    }
    private void ClimbEdge()
    {
        if (HasEdge() && !isDashing && dirX != 0)
        {
            if (spirit == SpiritState.Sareko)
            {
                rb.velocity = new Vector2(transform.localScale.x, 15f);
            }
            else
            {
                rb.velocity = new Vector2(transform.localScale.x, 10f);
            }
        }
    }

    private void DashAnEdge() //Sareko will get a push upward if hitting an edge while dashing
    {
        if (EdgeToDash() && isDashing)
        {
            rb.velocity = new Vector2(transform.localScale.x, 15f);
            canDash = true;
        }
    }

    private void WallSlide()
    {
        if (spirit == SpiritState.Sareko)
        {
            if (isWalled && !isGrounded && dirX != 0f)
            {
                isWallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }
            else
            {
                isWallSliding = false;
            }
        }
        else if (spirit == SpiritState.Cat)
        {
            if (isWalled && !isGrounded && dirX != 0f)
            {
                isWallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            else
            {
                isWallSliding = false;
            }
        }
    }

    private void WallClimb() //Will expand on
    {
        if (isWallSliding) rb.velocity = new Vector2(rb.velocity.x, Input.GetAxisRaw("Vertical") * climbSpeed);
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (isJumpPressed && wallJumpingCounter > 0f)
        {
            isJumpPressed = false;
            jumpBreath.Play();
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        dashSound.Play();
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    #endregion

    #region Check Surroundings
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheckHigh.position, 0.1f, groundLayer) && Physics2D.OverlapCircle(wallCheckLow.position, 0.1f, groundLayer) && !HasEdge();
    }

    private bool HasEdge()
    {
        if(spirit == SpiritState.Sareko)
        {
            return !Physics2D.OverlapCircle(wallCheckHigh.position, 0.1f, groundLayer) && Physics2D.OverlapCircle(wallCheckTop.position, 0.1f, groundLayer);
        }
        else
        {
            return Physics2D.OverlapCircle(wallCheckLow.position, 0.1f, groundLayer) && !Physics2D.OverlapCircle(wallCheckHigh.position, 0.1f, groundLayer);
        }
    }

    private bool EdgeToDash()
    {
        return !Physics2D.OverlapCircle(wallCheckHigh.position, 0.1f, groundLayer) && (Physics2D.OverlapCircle(wallCheckLow.position, 0.1f, groundLayer));
    }
    #endregion

    private void FootSteps()
    {
        int randomIndex = Random.Range(0, footSteps.Length);
        footSteps[randomIndex].Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheckTop.position, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wallCheckHigh.position, 0.1f);
        Gizmos.DrawWireSphere(wallCheckLow.position, 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheckPoint.position, groundSheckSize);
    }
}

