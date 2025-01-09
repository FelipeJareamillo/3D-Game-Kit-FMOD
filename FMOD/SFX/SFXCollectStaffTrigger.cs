using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXCollectStaffTrigger : MonoBehaviour
{
    private void OnTriggerEnter()
    {
        //Le decimos que le de play una vez a un evento cuando entre dentro de un trigger
        //El "PlayOneShot" requiere de el path del evento que se quiere dar play y si este es 3D un "Vector3"
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Interactable/Staff Collect", transform.position);

        //El "PlayOneShotAttached" requiere de el path del evento que se quiere dar play y si este es 3D un "gameObject"
        //FMODUnity.RuntimeManager.PlayOneShotAttached("event:/SFX/Interactable/Staff Collect", gameObject);
    }
}
