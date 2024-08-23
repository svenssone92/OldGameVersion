using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterBush : MonoBehaviour
{
    [SerializeField] AudioSource bushNoise;

    static public bool isInBush = false;

    private BoxCollider2D bushCollider;

    [Header("LayerMasks")]
    [SerializeField] private LayerMask playerLayer;

    void Start()
    {
        bushCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        isInBush = IsInBush();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sareko"))
        {
            bushNoise.Play();
        }
    }

    private bool IsInBush()
    {
        RaycastHit2D hit = Physics2D.BoxCast(bushCollider.bounds.center, bushCollider.bounds.size, 0f, Vector2.down, 0f, playerLayer);

        return hit.collider != null;
    }

}
