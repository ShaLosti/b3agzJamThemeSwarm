using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicController : MonoBehaviour
{
    [SerializeField]
    AudioSource doorClose;
    [SerializeField]
    AudioSource doorOpen;
    [SerializeField]
    AudioSource[] punchSounds;
    [SerializeField]
    AudioSource doorBreak;

    public static MusicController instance;
    
    private void Awake()
    {
        instance = this;
    }

    public void PlayDoorCloseSound()
    {
        if (doorClose.isPlaying)
            doorClose.Stop();
        doorClose.Play();
    }
    public void PlayDoorOpenSound()
    {
        if (doorOpen.isPlaying)
            doorOpen.Stop();
        doorOpen.Play();
    }
    public void PlayPunchSound()
    {
        AudioSource punch = punchSounds[Random.Range(0, punchSounds.Length)];
        if (punch.isPlaying)
            punch.Stop();
        punch.Play();
    }

    public void PlayDoorBreak()
    {
        if (doorBreak.isPlaying)
            doorBreak.Stop();
        doorBreak.Play();
    }
}
