using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void ChomperEvent(string chomperSound)
    {
        FMODUnity.RuntimeManager.PlayOneShot(chomperSound, transform.position);
    }
}
