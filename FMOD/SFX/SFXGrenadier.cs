using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXGrenadier : MonoBehaviour
{
    
    public void GrenadierSFX(string grenadierPath)
    {
        RuntimeManager.PlayOneShot(grenadierPath, transform.position);
    }
}
