using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BushTrigger : MonoBehaviour
{
    [SerializeField] private string level;
    [SerializeField] private Vector2 position;



    private void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sareko"))
        {
            if(position != null)
            {
                SarekoTopDownMovement.startingPosition = position;
            }

            SceneManager.LoadScene(level);
        }
    }
}