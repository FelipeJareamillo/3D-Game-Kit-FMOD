using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicPlayer : MonoBehaviour
{
    //Una variable convierte los FMOD Paths en Strings 
    [SerializeField] EventReference eventPath;

  //[SerializeField] FMODUnity.EventReference eventPathTwo;

    // Crea una copia de nuestro evento en FMOD
    public static FMOD.Studio.EventInstance musicInst;
    //FMOD.Studio.EventInstance prueba;

    //Esta variable se puede usar para manipular los estados de reproduccion de los eventos
    PLAYBACK_STATE musicState;

    // Start is called before the first frame update
    void Start()
    {
        /*Aqui le decimos que evento queremos crear una copia
         * En el metodo de "CreateInstance" dentro del parentesis hay que tener dos cosas:
         * El evento que colocamos como "event:/"
         * Y el nombre de la carpeta donde esta guardada el evento y el nombre del evento en si "Music(carpeta)/GamePlay Music(Nombre del evento)"  
        */
        musicInst = RuntimeManager.CreateInstance(eventPath);
     //RuntimeManager.PlayOneShot(eventPathTwo);
        musicInst.start();
        musicInst.release();

        //Con esto le pedimos que nos de el estado en el que esta la musica, si es reproduciendoce, si espa loopeando, etc
        musicInst.getPlaybackState(out musicState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        musicInst.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
