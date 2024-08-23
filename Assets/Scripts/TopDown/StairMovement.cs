using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairMovement : MonoBehaviour
{
    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sareko"))
        {
            OnStairs();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Sareko"))
        {
            OffStairs();
        }
    }

    public void OnStairs()
    {
        SarekoTopDownMovement.onStairs = true;
    }

    public void OffStairs()
    {
        SarekoTopDownMovement.onStairs = false;
    }
}
