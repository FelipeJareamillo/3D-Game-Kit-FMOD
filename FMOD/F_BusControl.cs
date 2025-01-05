using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.UI;

public class F_BusControl : MonoBehaviour
{
  

    Slider slider;

    //Referencia al VCA
    VCA vca;

    //Contiene el nombre de los VCA
    [SerializeField] string vcaPath;

    //Con este vamos a guardar el nivel del VCA
    float vcaVolume;


   
    // Start is called before the first frame update
    void Start()
    {
        vca = RuntimeManager.GetVCA("vca:/" + vcaPath);
        vca.getVolume(out vcaVolume);
        slider = GetComponent<Slider>();

        //Le decimos que el valor inicial del slider sea el mismo que de los buses de FMOD
        slider.value = vcaVolume;
    }

    /// <summary>
    /// Va a cambiar el volumen de los buses, esta publico para que aparesca en el on value change de los sliders
    /// </summary>
    /// <param name="sliderValue"></param>
    public void VolumeLevel(float sliderValue)
    {
        vca.setVolume(sliderValue);
    }

}
