using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class F_ReverbZone2 : MonoBehaviour
{

    EventInstance reverb2;


    // Start is called before the first frame update
    void Start()
    {
        reverb2 = RuntimeManager.CreateInstance("snapshot:/Reverb Zone 2");

        RuntimeManager.AttachInstanceToGameObject(reverb2, transform);
        reverb2.start();

    }

    private void OnDestroy()
    {
        reverb2.release();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
