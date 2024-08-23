using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSortingOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Vector2 yOffSet;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on " + gameObject.name);
        }
    }

    void Update()
    {
        // Set sorting order based on the y-axis position
        spriteRenderer.sortingOrder = Mathf.RoundToInt((transform.position.y + yOffSet.y) * 100f) * -1;
    }
}