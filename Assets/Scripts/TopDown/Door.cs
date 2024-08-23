using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractables
{

    private SpriteRenderer spriteRenderer;

    [Header("Collider")]
    [SerializeField] private BoxCollider2D doorCollider;

    [Header("Audios")]
    [SerializeField] private AudioSource openDoorSound;
    [SerializeField] private AudioSource closeDoorSound;

    private bool isDoorOpen;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        
    }

    public void Interact()
    {

        if(!isDoorOpen)
        {
        openDoorSound.Play();
        doorCollider.isTrigger = !doorCollider.isTrigger;

        HideSprite();
        isDoorOpen = true;
        }
    }

    void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Sareko"))
        {
            closeDoorSound.Play();
            ShowSprite();


            doorCollider.isTrigger = !doorCollider.isTrigger;
            isDoorOpen=false;
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
