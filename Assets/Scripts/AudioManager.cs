//for Sword Search, copyright Fancy Bus Games 2026

using System.Collections.Generic;
using UnityEngine;

public class AudioManager{

    static public List<AudioSource> audioSources;
    static public int audioSourceIndex = 0;
    static public AudioLibrary library;

    static public void PlaySound(SoundEffect soundEffect){
        AudioSource source = audioSources[audioSourceIndex];
        audioSourceIndex ++;
        if (audioSourceIndex >= audioSources.Count)
            audioSourceIndex = 0;
        source.clip = soundEffect.audioClip;
        source.volume = (float)(soundEffect.volumePercentage / 100.0) * (float)(StaticVariables.globalVolume / 100.0);
        source.Play();
    }
}

[System.Serializable]
public class SoundEffect{
    public AudioClip audioClip;
    [Range(0, 200)]
    public int volumePercentage = 100;
}