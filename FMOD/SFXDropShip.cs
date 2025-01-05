using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXDropShip : MonoBehaviour
{

    EventInstance dropShipSFX;
    // Start is called before the first frame update
    void Start()
    {
        dropShipSFX = RuntimeManager.CreateInstance("event:/SFX/Enviorment/DropShip");
        RuntimeManager.AttachInstanceToGameObject(dropShipSFX, transform);

        dropShipSFX.start();
        dropShipSFX.release();
    }

   
}
