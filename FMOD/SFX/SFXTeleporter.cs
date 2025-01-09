using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXTeleporter : MonoBehaviour
{
    [SerializeField] FMODUnity.EventReference teleporterPath;
    

    FMOD.Studio.EventInstance sfxTeleporter;


    // Start is called before the first frame update
    void Start()
    {
        sfxTeleporter = FMODUnity.RuntimeManager.CreateInstance(teleporterPath);

        sfxTeleporter.start();

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sfxTeleporter, transform);

        sfxTeleporter.release();
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Ellen")
        {
            //"setParameterByName" es el metodo con el que podemos modificar parametros de unity a fmod
            //Requiere del nombre del parametro en un stru¿ing y el valor a modificar
            sfxTeleporter.setParameterByName("Trigger", 1);

            MusicPlayer.musicInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

  
}
