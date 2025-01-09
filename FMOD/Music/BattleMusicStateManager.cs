using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMusicStateManager : MonoBehaviour
{
    [SerializeField] LayerMask layer;

    private void OnTriggerEnter(Collider other)
    {
        //La ecuacion nos va a dar cero si no hay coincidencias entre el layer del objeto y del objeto con el que colisiona por lo que si es el caso no va a hacer nada
        //Si si llega a coincidir con el layer mask el valor va a ser diferente a cero por lo que ejecutara el codico dentro del "if"
        if(0!=(layer.value & 1 << other.gameObject.layer))
        {
            MusicManager.instance.SetGlobalMusicParameter(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //La ecuacion nos va a dar cero si no hay coincidencias entre el layer del objeto y del objeto con el que colisiona por lo que si es el caso no va a hacer nada
        //Si si llega a coincidir con el layer mask el valor va a ser diferente a cero por lo que ejecutara el codico dentro del "if"
        if (0 != (layer.value & 1 << other.gameObject.layer))
        {
            MusicManager.instance.SetGlobalMusicParameter(0);
        }
    }
}
