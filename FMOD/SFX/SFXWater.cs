using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXWater : MonoBehaviour
{
    //Convierte un FMOD path a una variable string para darselo al "CreatInstance"
    [SerializeField] FMODUnity.EventReference waterPath;

    //Guarda una compia de un evento de FMOD
    FMOD.Studio.EventInstance sfxWater;

    // Start is called before the first frame update
    void Start()
    {
        //Le da al "EventIstne" el evento de FMOD que va a guardar
        sfxWater = FMODUnity.RuntimeManager.CreateInstance(waterPath);

        //Le decimos al evento 3D donde queremos que suene y este metodo lo actulizara cada frame
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(sfxWater, transform);

        //Le da play al evento
        sfxWater.start();

        //Elimina el evnto para que no ocupa espacio
        sfxWater.release();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        //Para el evento
        sfxWater.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
