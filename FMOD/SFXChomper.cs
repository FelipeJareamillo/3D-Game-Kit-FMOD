using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SFXChomper : MonoBehaviour
{
  
    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChomperEvent(string chomperSound)
    {
       RuntimeManager.PlayOneShot(chomperSound, transform.position);
    }
}
