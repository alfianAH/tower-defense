using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : SingletonBaseClass<AudioPlayer>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioClips;

    public void PlaySFX(string name)
    {
        AudioClip sfx = audioClips.Find(s => s.name == name);
        
        if(sfx == null) return;
        
        audioSource.PlayOneShot(sfx);
    }
}
