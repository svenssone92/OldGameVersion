using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDynamictSortingOrder : MonoBehaviour
{
    private TilemapRenderer tileRenderer;
    [SerializeField] private Vector2 yOffSet;

    void Start()
    {
        tileRenderer = GetComponent<TilemapRenderer>();
        if (tileRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on " + gameObject.name);
        }
    }

    void Update()
    {
        tileRenderer.sortingOrder = Mathf.RoundToInt((transform.position.y + yOffSet.y) * 100f) * -1;
    }
}
