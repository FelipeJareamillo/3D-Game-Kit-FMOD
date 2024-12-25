using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.UI;

public class F_BusControl : MonoBehaviour
{
    //Referencia el objeto del bus en si
    Bus bus;

    //Contiene el nombre de los buses
    [SerializeField] string busPath;

    //Nos dice el volumen que se seteo en la API de FMOD dentrp del codigo, osea con el valor inicial que se le dio al setVolume
    float busVolume;

    //Este nos da el volumen tanto de FMOD como el que seteamos con setVolume
    float finalBusVolume;

    Slider slider;

    EventInstance levelTest;

    PLAYBACK_STATE pb;

    // Start is called before the first frame update
    void Start()
    {
        //Le damos que bus queremos referenciar
        bus = RuntimeManager.GetBus("bus:/" + busPath);
        bus.getVolume(out busVolume, out finalBusVolume);

        slider = GetComponent<Slider>();

        //Le decimos que el valor inicial del slider sea el mismo que de los buses de FMOD
        slider.value = busVolume;

        //Si el bus es el de Efectos etnonces creame una instancia del evento que quiero darle play
        if (busPath == "SFX")
        {
            levelTest = RuntimeManager.CreateInstance("event:/SFX/UI/Level Test");
        }
        else
            levelTest.release();
    }

    /// <summary>
    /// Va a cambiar el volumen de los buses, esta publico para que aparesca en el on value change de los sliders
    /// </summary>
    /// <param name="sliderValue"></param>
    public void VolumeLevel(float sliderValue)
    {
        bus.setVolume(sliderValue);

        //El bus que seleccione es el de efectos
        if(busPath == "SFX")
        {
            //Le pido que me de el playback en el que esta mi instancia "levelTest"
            levelTest.getPlaybackState(out pb);
            //Si el sonido no se esta reproduciendo entonces que se repordusca
            if(pb != PLAYBACK_STATE.PLAYING)
            {
                levelTest.start();
            }
        }
    }

    private void OnDestroy()
    {
        if(busPath == "SFX")
        {
            levelTest.release();
        }
    }
}
