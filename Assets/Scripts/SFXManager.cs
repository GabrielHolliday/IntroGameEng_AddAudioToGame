using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;

public class SFXManager : MonoBehaviour
{
    public GameManager gameManager;
    public AudioClip playerShoot;
    public AudioClip asteroidExplosion;
    public AudioClip playerDamage;
    public AudioClip playerExplosion;
    public AudioClip BgMusicGameplay;
    public AudioClip BgMusicTitleScreen;
    public AudioClip playerMilestone;

    private AudioSource SFXaudioSource;

    private AudioSource BgMusicAudioSource1;
    private AudioSource BgMusicAudioSource2;
    private AudioSource shooting;

    

    public void Awake()
    {
        SFXaudioSource = GetComponent<AudioSource>();
        shooting = transform.Find("Shooting").GetComponent<AudioSource>();
        //GameObject child = this.transform.Find("BgMusic").gameObject;

        BgMusicAudioSource1 = gameObject.transform.Find("BgMusic1").gameObject.GetComponent<AudioSource>(); // the extra one is for music looping while keeping reverb
        BgMusicAudioSource2 = gameObject.transform.Find("BgMusic2").gameObject.GetComponent<AudioSource>();
        

        //BgMusicAudioSource.GetComponent<AudioSource>().Play();       
        BgMusicAudioSource1.volume = 0.25f;
        BgMusicAudioSource2.volume = 0.25f;

    }

    public void Start()
    {
        startingVal = SFXaudioSource.pitch;
        MusicLoop(gameManager.cancelationTokenSource.Token);
    }



    //called in the PlayerController Script
    public void PlayerShoot()
    {
        shooting.pitch = Random.Range(startingVal - 0.12f, startingVal + 0.12f);
        shooting.PlayOneShot(playerShoot);
    }

    //called in the PlayerController Script
    public void PlayerDamage()
    {
        SFXaudioSource.PlayOneShot(playerDamage);
    }

    public void PlayerMilestone()
    {
        SFXaudioSource.PlayOneShot(playerMilestone);
    }

    //called in the PlayerController Script
    private float startingVal;
    public void PlayerExplosion()
    {
 
        SFXaudioSource.PlayOneShot(playerExplosion);
    }

    //called in the AsteroidDestroy script
    public void AsteroidExplosion()
    {

        SFXaudioSource.PlayOneShot(asteroidExplosion);
    }



    private double remainder = 6;
    private double songLength;
    //private AudioSource curSource;
    private double targTime = 1;
    private bool flipFlop = false;
    public float tempo = 1;

    //private AudioClip QueUp;

    private async void MusicLoop(CancellationToken token)
    {
        songLength = BgMusicTitleScreen.length;


        while (true)
        {

            targTime = AudioSettings.dspTime;
            targTime += (songLength - remainder);
            if (flipFlop)
            {
                //Debug.Log("Playing Track 1");
                BgMusicAudioSource1.time = 0;
                BgMusicAudioSource1.Play();
                while (BgMusicAudioSource1.time < (songLength - remainder)) await Task.Delay(1);
                if (token.IsCancellationRequested) return;
            }
            else
            {
                //Debug.Log("Playing Track 2");
                BgMusicAudioSource2.time = 0;
                BgMusicAudioSource2.Play();
                while (BgMusicAudioSource2.time < (songLength - remainder)) await Task.Delay(1);
                if (token.IsCancellationRequested) return;
            }
            BgMusicAudioSource1.pitch = tempo;
            BgMusicAudioSource2.pitch = tempo;

            flipFlop = !flipFlop;

        }
    }
    
    public void IncreaseTempo(float amoutn)
    {
        tempo += amoutn;
        BgMusicAudioSource1.pitch = tempo;
        BgMusicAudioSource2.pitch = tempo;

    }
    
    public void BGMusicMainMenu()
    {
        BgMusicAudioSource1.clip = BgMusicTitleScreen;
        BgMusicAudioSource2.clip = BgMusicTitleScreen;
        if (flipFlop) BgMusicAudioSource1.Play();
        else BgMusicAudioSource2.Play();
        BgMusicAudioSource1.time = (float)(songLength  - remainder) - (float)(targTime - AudioSettings.dspTime);
        BgMusicAudioSource2.time = (float)(songLength - remainder) - (float)(targTime - AudioSettings.dspTime);
        tempo = 1;
        BgMusicAudioSource1.pitch = tempo;
        BgMusicAudioSource2.pitch = tempo;
        remainder = 6.25;
    }

    public void BGMusicGameplay()
    {
        BgMusicAudioSource1.clip = BgMusicGameplay;
        BgMusicAudioSource2.clip = BgMusicGameplay;
        if (flipFlop) BgMusicAudioSource1.Play();
        else BgMusicAudioSource2.Play();
        BgMusicAudioSource1.time = (float)(songLength  - remainder) - (float)(targTime - AudioSettings.dspTime);
        BgMusicAudioSource2.time = (float)(songLength - remainder) - (float)(targTime - AudioSettings.dspTime);
        remainder = 6.25;

    }
}
