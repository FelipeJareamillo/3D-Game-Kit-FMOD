using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class SFXPressurePad : MonoBehaviour
{
    EventInstance pressurePadSFX;

    PARAMETER_ID paramID;

    [SerializeField] GameObject hugeDoor;

    // Start is called before the first frame update
    void Start()
    {
        pressurePadSFX = RuntimeManager.CreateInstance("event:/SFX/Interactable/PressurePads/PressurePad");

        paramID.data1 = 4123819339;
        paramID.data2 = 2507658778;

        RuntimeManager.AttachInstanceToGameObject(pressurePadSFX, transform);
        pressurePadSFX.start();
        pressurePadSFX.release();


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Ellen")
        {
            pressurePadSFX.setParameterByID(paramID, 1);
            RuntimeManager.PlayOneShotAttached("event:/SFX/Enviorment/HugeDoor3D",hugeDoor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
