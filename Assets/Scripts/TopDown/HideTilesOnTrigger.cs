using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HideTilesOnTrigger : MonoBehaviour
{
    private TilemapRenderer tileRenderer;

    [SerializeField] private TilemapRenderer secondTileRenderer;
    [SerializeField] private TilemapRenderer thirdTileRenderer;

    void Start()
    {
        tileRenderer = GetComponent<TilemapRenderer>();
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
        if (tileRenderer != null)
        {
            tileRenderer.enabled = false;
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
        if (tileRenderer != null)
        {
            tileRenderer.enabled = true;
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
