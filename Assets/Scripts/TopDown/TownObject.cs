using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownObject : MonoBehaviour, IInteractables
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    public void Interact()
    {
        DialogManager.GetInstance().EnterDialogMode(inkJSON);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
