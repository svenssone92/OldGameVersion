using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownMusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgMusic;

    void Start()
    {
        bgMusic.Play();
    }

    void Update()
    {
        if (EnterBush.isInBush)
        {
            if (bgMusic.isPlaying)
            {
                bgMusic.Pause();
            }
        }
        else if(!bgMusic.isPlaying)
        {
            bgMusic.Play();
        }
        
    }
}
