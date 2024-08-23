using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnTrigger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Sareko"))
        {
            HideSprite();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Sareko"))
        {
            ShowSprite();
        }
    }

    void HideSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }

    void ShowSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
    }
}
