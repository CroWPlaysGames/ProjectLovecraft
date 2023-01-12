using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
        }
    }

    void Start()
    {
        StartCoroutine(WaitToPlay());
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        sound.source.outputAudioMixerGroup = sound.mixer;
        sound.source.Play();
    }

    public void StopAudio()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }

    IEnumerator WaitToPlay()
    {
        yield return new WaitForSeconds(6);

        Play("Main Menu Music");
    }
}
