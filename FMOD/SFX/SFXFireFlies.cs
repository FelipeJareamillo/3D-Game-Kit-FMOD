using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXFireFlies : MonoBehaviour
{

    //Una variable convierte los FMOD Paths en Strings 
    [SerializeField] FMODUnity.EventReference fireFliesPath;

    // Crea una copia de nuestro evento en FMOD
    FMOD.Studio.EventInstance sfxFireFlies;

    // Start is called before the first frame update
    void Start()
    {
        /*Aqui le decimos que evento queremos crear una copia
         * En el metodo de "CreateInstance" dentro del parentesis hay que tener dos cosas:
         * El evento que colocamos como "event:/"
         * Y el nombre de la carpeta donde esta guardada el evento y el nombre del evento en si "Music(carpeta)/GamePlay Music(Nombre del evento)"  
        */
        sfxFireFlies = FMODUnity.RuntimeManager.CreateInstance(fireFliesPath);

        /*El AttachInstanceToGameObject es una alternativa al set3DAttributes en la que FMOD nos va a actualizar la posicion, rotacion, etc del objeto cada frame
         * Tiene 3 variables: la primera es el Instance que es "sfxFireFlies", un transform y un rigidbody, este ultimo es opcional
         */
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sfxFireFlies, transform);

        sfxFireFlies.start();
        sfxFireFlies.release();
    }

    // Update is called once per frame
    void Update()
    {
        /*Set3DAttributes lo que hace es decir en que posicion esta el evento y se pone en el update para que se actualice cada frame
         * Este tiene un comando (FMODUnity.RuntimeUtils.To3DAttributes()) con el cual le diremos cada frame donde esta el evento
         * Hay dos formas de darle las coordenadas de donde queremos que el evento se emita, la primera es con el "Transform.Position" y la segunda con el "RigidBody"
        */

       // sfxFireFlies.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));

    }

    private void OnDestroy()
    {
        sfxFireFlies.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
