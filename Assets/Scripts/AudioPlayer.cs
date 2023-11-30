using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx1;

    public void StartGame()
    {
        src.clip = sfx1;
    }
}
