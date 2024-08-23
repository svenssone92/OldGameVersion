using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    GameObject unit;
    [SerializeField] int maxHp = 100;
    [SerializeField] private float pushbackForce =2;
    private int currentHp;

    void Start()
    {
        currentHp = maxHp;
        unit = gameObject;
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage, Vector2 attackDirection)
    {
        currentHp -= damage;


        if (currentHp <= 0)
        {
            Die();
        }

        // Apply pushback force
        Rigidbody2D enemyRb = GetComponent<Rigidbody2D>();
        if (enemyRb != null)
        {
            enemyRb.velocity = attackDirection.normalized * pushbackForce;
        }
    }

    private void Die()
    {
        Debug.Log("Target is Dead");
        unit.SetActive(false);
    }
}
