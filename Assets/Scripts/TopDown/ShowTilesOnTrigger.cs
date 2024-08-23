using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShowTilesOnTrigger : MonoBehaviour
{
    [SerializeField] private TilemapRenderer firstTileRenderer;
    [SerializeField] private TilemapRenderer secondTileRenderer;
    [SerializeField] private TilemapRenderer thirdTileRenderer;
     
    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sareko"))
        {
            ShowSprite();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Sareko"))
        {
            HideSprite();
        }
    }

    void HideSprite()
    {
        if (firstTileRenderer != null)
        {
            firstTileRenderer.enabled = false;
        }
        if (secondTileRenderer != null)
        {
            secondTileRenderer.enabled = false;
        }
        if (thirdTileRenderer != null)
        {
            thirdTileRenderer.enabled = false;
        }
    }

    void ShowSprite()
    {
        if (firstTileRenderer != null)
        {
            firstTileRenderer.enabled = true;
        }
        if (secondTileRenderer != null)
        {
            secondTileRenderer.enabled = true;
        }
        if (thirdTileRenderer != null)
        {
            thirdTileRenderer.enabled = true;
        }
    }
}
