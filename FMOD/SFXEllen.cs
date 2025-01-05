using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Gamekit3D;

public class SFXEllen : MonoBehaviour
{
    public EventInstance healthLow;

    float materialValue;

    //Nos devolvera con que colisiona el raycast
    RaycastHit hit;

    //La destancia en la que viaja el raycast
    float distance = 0.3f;

    //Los tipos de Layers con el que el raycast va a interactuar
    [SerializeField] LayerMask layer;

    PlayerController playerController;

    [ParamRef]
    [SerializeField]
    string paramRef;

    EventInstance ellenDialog;
    PLAYBACK_STATE pb;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();

        ellenDialog = RuntimeManager.CreateInstance("event:/SFX/Ellen/EllenTalk");
        ellenDialog.getPlaybackState(out pb);

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down * distance, Color.blue);
    }

    void PlayRunEvent(string runPath)
    {
        MaterialValue();
        //Aqui estamos declarando la funcion dentro de un metodo por lo que podemos crear una instancia y guardarla de una vez
        //Lo que no se va a poder hacer es que el evento "Run" solo podra ser usado dentro del metodo
        FMOD.Studio.EventInstance Run = FMODUnity.RuntimeManager.CreateInstance(runPath);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(Run, transform);
        Run.setParameterByName("Material", materialValue,false);
        Run.start();
        Run.release();
    }

    void MaterialValue()
    {
        //Si golpea un layer entonces identifica que objeto
        if(Physics.Raycast(transform.position, Vector3.down, out hit, distance, layer))
        {
            //Si el objeto tiene el tag de Earth entinces cambiame el valor del material
            if(hit.collider.CompareTag("Earth"))
            {
                materialValue = 0;
            }
            //Si el objeto tiene el tag de Grass entinces cambiame el valor del material
            else if (hit.collider.CompareTag("Grass"))
            {
                materialValue = 1;
            }
            //Si el objeto tiene el tag de Puddle entinces cambiame el valor del material
            else if (hit.collider.CompareTag("Puddle"))
            {
                materialValue = 2;
            }
            //Si el objeto tiene el tag de Stone entinces cambiame el valor del material
            else if (hit.collider.CompareTag("Stone"))
            {
                materialValue = 3;
            }
            //Si el raycasthit no colisiona con nada que tenga los anteriores tags entonces que ponga un valor predeterminado
            else
            {
                materialValue = 0;
            }
        }
        //Si el raycast no golpea con ningun layer entonces que cambie un valor predeterminado
        else
        {
            materialValue = 0;
        }
    }

     public void LowHealthSFX()
     {
        healthLow = RuntimeManager.CreateInstance("snapshot:/Low Health");
        RuntimeManager.StudioSystem.setParameterByName(paramRef, playerController.healthAudioChange); 
        healthLow.start();
     }

    public void DialogSFX()
    {
        if (pb != PLAYBACK_STATE.PLAYING)
        {
            ellenDialog.start();
        }
            
    }

    void LandingSound()
    {
        RuntimeManager.PlayOneShot("event:/SFX/Ellen/EllenLanding", transform.position);
    }

    private void OnDestroy()
    {
        healthLow.release();
        ellenDialog.release();
    }
}
