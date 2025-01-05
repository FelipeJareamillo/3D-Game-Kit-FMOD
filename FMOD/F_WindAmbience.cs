using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_WindAmbience : MonoBehaviour
{
    [SerializeField] GameObject ellen;
    EventInstance ambienceWind;
    // Start is called before the first frame update
    void Start()
    {
        ambienceWind = RuntimeManager.CreateInstance("event:/SFX/Enviorment/Ambience/GeneralAmbience");
        RuntimeManager.AttachInstanceToGameObject(ambienceWind,ellen.transform);
        ambienceWind.start();
        ambienceWind.release();
    }

    private void OnDestroy()
    {
        ambienceWind.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
