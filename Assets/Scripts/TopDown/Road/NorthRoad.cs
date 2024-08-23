using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NorthRoad : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    void Start()
    {

    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sareko"))
        {

            SarekoTopDownMovement sarekoController = other.GetComponent<SarekoTopDownMovement>();

            StartCoroutine(SendBack(sarekoController));
        }
    }

    private IEnumerator SendBack(SarekoTopDownMovement sarekoController)
    {

        if (sarekoController != null)
        {
            sarekoController.WalkBack();
        }

        yield return new WaitForSeconds(0.5f);

        DialogManager.GetInstance().EnterDialogMode(inkJSON);

        sarekoController.LetMove();

    }
}
