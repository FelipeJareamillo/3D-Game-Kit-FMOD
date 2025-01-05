using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using Gamekit3D;

public class SFXHealthBox : MonoBehaviour
{
    [SerializeField] EventReference healthLoopPath;
    [SerializeField] EventReference healthOpenPath;

    EventInstance healthLoop;
    EventInstance healthOpen;

    PlayerController playerController;

    [ParamRef]
    [SerializeField]
    string paramPath;

    // Start is called before the first frame update
    void Start()
    {
        healthLoop = RuntimeManager.CreateInstance(healthLoopPath);
        healthOpen = RuntimeManager.CreateInstance(healthOpenPath);

        playerController = FindObjectOfType<PlayerController>();

        RuntimeManager.AttachInstanceToGameObject(healthLoop, transform);
        healthLoop.start();
        healthLoop.release();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Ellen")
        {
            RuntimeManager.AttachInstanceToGameObject(healthOpen, transform);
            healthOpen.start();
            healthOpen.release();
            healthLoop.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            playerController.healthAudioChange = 5;
            RuntimeManager.StudioSystem.setParameterByName(paramPath, playerController.healthAudioChange);
        }
    }
}
