using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AudioControl : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("Audio source for the home diner ambience.")]
    [SerializeField] private AudioSource homeDinerAudioSource;
    [Tooltip("Audio source for the ruin ambience.")]
    [SerializeField] private AudioSource ruinAudioSource;
    [Tooltip("Audio source for the boat ambience.")]
    [SerializeField] private AudioSource boatAudioSource;
    [Tooltip("Bool to control which audio is playing.")]
    [SerializeField] private bool homeDinerAudio = true;
    [SerializeField] private bool boatAudio = false;
    [SerializeField] private bool ruinAudio = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (homeDinerAudio)
        {
            homeDinerAudioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RuinAudioTrigger"))
        {
            // home audio will be off at this point
            // could be arrival or exit
            if (boatAudio && !ruinAudio)
            {
                // arrival
                StartCoroutine(FadeOut(boatAudioSource, 2f));
                StopCoroutine(FadeOut(boatAudioSource, 2f));
                boatAudio = false;
                ruinAudioSource.Play();
                ruinAudio = true;
            }
            
            else if (ruinAudio && !boatAudio)
            {
                // exit
                StartCoroutine(FadeOut(ruinAudioSource, 2f));
                StopCoroutine(FadeOut(ruinAudioSource, 2f));
                ruinAudio = false;
                boatAudioSource.Play();
                boatAudio = true;
            }
        }
        if (other.CompareTag("BoatAudioTrigger"))
        {
            // only on arrival
            if (homeDinerAudio)
            {
                StartCoroutine(FadeOut(homeDinerAudioSource, 2f));
                StopCoroutine(FadeOut(homeDinerAudioSource, 2f));
                homeDinerAudio = false;
            }
            if (!boatAudio)
            {
                boatAudioSource.Play();
                boatAudio = true;
            }
        }
    }
    // reference: https://discussions.unity.com/t/fade-out-audio-source/585912
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
