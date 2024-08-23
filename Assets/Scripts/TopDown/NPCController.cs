using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, IInteractables
{
    private Animator animator;
    public static bool isInteractedWith = false;

    [SerializeField] private Vector2 defaultDirection;


    [Header("Idles")]
    [SerializeField] private string IdleRight;
    [SerializeField] private string IdleLeft;
    [SerializeField] private string IdleUp;
    [SerializeField] private string IdleDown;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInteractedWith)
        {
            SetIdleAnimation(-SarekoTopDownMovement.facingDirection);
        }
        else
        {
            SetIdleAnimation(defaultDirection);
        }

    }

    private void SetIdleAnimation(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
                PlayAnimation(IdleRight);
            else
                PlayAnimation(IdleLeft);
        }
        else
        {
            if (direction.y > 0)
                PlayAnimation(IdleUp);
            else
                PlayAnimation(IdleDown);
        }
    }

    private void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }

    public void Interact()
    {
        isInteractedWith = true;
        DialogManager.GetInstance().EnterDialogMode(inkJSON);
    }
}
