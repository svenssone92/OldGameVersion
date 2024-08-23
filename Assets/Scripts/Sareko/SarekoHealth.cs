using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SarekoHealth : MonoBehaviour
{
    GameObject unit;
    [SerializeField] int maxHp = 100;
    [SerializeField] private AudioSource reSpawnSound;
    private int currentHp;

    void Start()
    {
        currentHp = maxHp;
        unit = gameObject;
    }


    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;


        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

        currentHp = 100;
        StartCoroutine(ReturnToCheckPointWithDelay());

    }

    private IEnumerator ReturnToCheckPointWithDelay()
    {
        Movement.isReSpawning = true;
        Movement.canMove = false;
        Movement.doAReSpawn = true;

        yield return new WaitForSeconds(0.1f);

        transform.position = Movement.CheckPointPosition;

    }

    //Trigges by animation event
    private void ReSpawnSound()
    {
        reSpawnSound.Play();
    }
}
