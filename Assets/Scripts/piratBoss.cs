using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class piratBoss : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private SpriteRenderer sprite;

    public float activationDistance = 15f;
    private float initialDelay = 10f;

    public float gunCooldown = 10f;
    public float gunDistance = 10f;

    public bool hasStartedSpinAttack =false;

    private float distanceToPlayer;
    //private bool hasStartedSpinAttack = false;
    private bool canShoot = false;
    public static bool bossActivated = false;

    //private float bossMusicTimer = 32f;

    [SerializeField] private Transform sareko;

    //Audio
    [SerializeField] private AudioSource gunSound;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, sareko.position);

        if (!bossActivated && distanceToPlayer < activationDistance)
        {
            ActivateBoss();
        }

        if (distanceToPlayer > gunDistance && canShoot && !hasStartedSpinAttack)
        {
            Shoot();
        }

        //Trigger spinning attack based on music time
        if (MusicController.startSpinningAttack && !hasStartedSpinAttack)
        {
            Debug.Log("Start Spinning Attack");
            SpinAttack();
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void Shoot()
    {
        canShoot = false;
        anim.SetTrigger("Shoot");
        StartCoroutine(GunCooldown());
    }

    private IEnumerator GunCooldown()
    {
        yield return new WaitForSeconds(gunCooldown);
        canShoot = true;
    }

    private void SpinAttack()
    {
        anim.SetTrigger("StartSpinAttack");
        hasStartedSpinAttack = true;
    }

    //get called be animation trigger
    private void EndSpinAttack()
    {
        hasStartedSpinAttack = false;
    }

    //GunSound is called by an animation trigger to get the right timing
    private void GunSound()
    {
        gunSound.Play();
    }


    private IEnumerator InitialDelay()
    {
        yield return new WaitForSeconds(initialDelay);
        canShoot = true;
    }

    private void ActivateBoss()
    {
        // Starts bossfight
        bossActivated = true;
        StartCoroutine(InitialDelay());
    }

}
