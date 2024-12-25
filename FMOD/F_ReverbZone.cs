using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class F_ReverbZone : MonoBehaviour
{

    EventInstance reverb;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        reverb = RuntimeManager.CreateInstance("snapshot:/Reverb Zone 1");

        player = GameObject.Find("Ellen");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            reverb.start();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            reverb.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    private void OnDestroy()
    {
        reverb.release();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
