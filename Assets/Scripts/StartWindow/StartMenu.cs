using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private float musicTimer = 1f;

    [SerializeField] private AudioSource bGMusic;
    [SerializeField] private AudioSource startSound;

    private bool inScreen = false;


    void Start()
    {
        bGMusic.Play();
    }
    void Update()
    {
        if (bGMusic.time > musicTimer && !inScreen)
        {
            inScreen = true;
            FadeController.FadeIn();
        }

    }

    //Gets called by buttons
    public void NewGame()
    {
        FadeController.FadeOut();
        bGMusic.Pause();
        startSound.Play();

        // Delay before loading the scene
        StartCoroutine(DelayedLoadScene("Home Town", 4f));
    }
    public void ExitGame()
    {
        // Close the game
        Application.Quit();
    }


    private IEnumerator DelayedLoadScene(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
