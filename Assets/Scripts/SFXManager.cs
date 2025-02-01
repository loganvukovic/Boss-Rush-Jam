using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{


    public AudioSource audioPlayer;
    public AudioClip[] sfxList;

    public void playSFX(int sfxIDNUM)
    {

        audioPlayer.clip = sfxList[sfxIDNUM];
        audioPlayer.Play();
    }
}
