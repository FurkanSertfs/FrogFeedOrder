using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    bool isSoundOn = true;

    [SerializeField]
    AudioClip collectibleCollected;

    [SerializeField]
    AudioClip hitGrape;

    [SerializeField]
    AudioClip wrongHit;

    [SerializeField]
    AudioSource audioSource;

    public enum Sound
    {
        CollectibleCollected,
        HitGrape,
        WrongHit
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        AudioListener.volume = isSoundOn ? 1 : 0;
    }

    public void PlaySound(AudioClip clip)
    {
        if (isSoundOn)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void PlaySound(Sound sound)
    {
        switch (sound)
        {
            case Sound.CollectibleCollected:
                PlaySound(collectibleCollected);
                break;
            case Sound.HitGrape:
                PlaySound(hitGrape);
                break;
            case Sound.WrongHit:
                PlaySound(wrongHit);
                break;
        }
    }
}
