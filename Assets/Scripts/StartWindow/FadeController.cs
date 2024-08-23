using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Movement;

public class FadeController : MonoBehaviour
{
    private Animator animator;

    public enum fadeState
    {
        blackOut,
        fadeIn,
        fadeOut
    }
    public static fadeState state = fadeState.blackOut;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetInteger("state", (int)state);
    }


    //gets called from StartMenu 
    public static void FadeOut()
    {
        state = fadeState.fadeOut;
    }
    public static void FadeIn()
    {
        state = fadeState.fadeIn;
    }
}
