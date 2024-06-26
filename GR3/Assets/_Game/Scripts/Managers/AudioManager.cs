using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("-------AudioSource-------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--------AudioClip--------")]
    public AudioClip background;//

    public AudioClip win;//
    public AudioClip die;//

    public AudioClip bulletShoot;//
    public AudioClip bulletHit;// 
    
    public AudioClip fly;//
 
    public AudioClip coin;//

    public AudioClip click;

    //public static AudioManager instance;


    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}