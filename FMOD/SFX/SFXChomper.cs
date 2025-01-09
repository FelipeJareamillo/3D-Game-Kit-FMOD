using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SFXChomper : MonoBehaviour
{
    public void ChomperEvent(string chomperSound)
    {
       RuntimeManager.PlayOneShot(chomperSound, transform.position);
    }
}
