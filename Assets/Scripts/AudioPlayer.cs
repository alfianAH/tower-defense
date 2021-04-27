using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // Singleton
    private static AudioPlayer instance;

    public static AudioPlayer Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<AudioPlayer>();

            return instance;
        }
    }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioClips;

    public void PlaySFX(string name)
    {
        AudioClip sfx = audioClips.Find(s => s.name == name);
        
        if(sfx == null) return;
        
        audioSource.PlayOneShot(sfx);
    }
}
