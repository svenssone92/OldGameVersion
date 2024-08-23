using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool isActivated = false;

    [SerializeField] private AudioSource activateSound;
    [SerializeField] private Sprite activatedSprite;
    
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the CheckPoint GameObject.");
        }
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sareko") && !isActivated)
        {
            activateSound.Play();
            isActivated = true;
            if (spriteRenderer != null && activatedSprite != null)
            {
                spriteRenderer.sprite = activatedSprite;

                Color newColor = spriteRenderer.color;
                newColor.a = 0.8f;
                spriteRenderer.color = newColor;
            }

            Debug.Log("Hit CheckPoint");
            Movement.CheckPointPosition = transform.position;
        }
    }
}
