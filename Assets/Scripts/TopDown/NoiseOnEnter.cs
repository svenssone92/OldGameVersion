using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseOnEnter : MonoBehaviour
{

    [SerializeField] AudioSource noise;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sareko"))
        {
            noise.Play();
        }
    }
}
