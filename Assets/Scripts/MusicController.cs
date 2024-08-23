using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static bool startSpinningAttack = false;
    private float bossSpinMusicTimer = 36f;
    private float tolerance = 0.1f;

    [SerializeField] private AudioSource bGMusic;
    [SerializeField] private AudioSource piratBossMusic;


    void Start()
    {
        
    }


    void Update()
    {

        if (piratBoss.bossActivated)
        {
            StartPiratBossMusic();
        }
        else
        {
            StartBGMusic();
        }
        if (Mathf.Abs(piratBossMusic.time - bossSpinMusicTimer) < tolerance && !startSpinningAttack)
        {
            startSpinningAttack =true;
        }

        if (startSpinningAttack && piratBossMusic.time > bossSpinMusicTimer)
        {
            Debug.Log("startSpinningAttack flag as false");
            startSpinningAttack = false;
        }

    }


    void StartPiratBossMusic()
    {
        if (!piratBossMusic.isPlaying)
        {
            bGMusic.Stop();
            piratBossMusic.Play();
        }
    }

    void StartBGMusic()
    {
        if (!bGMusic.isPlaying)
        {
            piratBossMusic.Stop();
            bGMusic.Play();
        }
    }
}
