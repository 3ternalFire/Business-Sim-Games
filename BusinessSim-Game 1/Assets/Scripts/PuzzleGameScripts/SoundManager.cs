using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;


    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    public void SetAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
