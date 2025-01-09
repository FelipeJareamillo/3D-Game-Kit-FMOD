using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MusicEventListener : MonoBehaviour
{

    [SerializeField] PostProcessVolume postVolume = null;
    float interpolationTime = 0f;
    Color defaultColor = new Color { r = 0.972549f, g = 0.86f, b = 0.85f, a = 1f };
    Color orange = new Color { r = 1f, g = 0.33f, b = 0f, a = 1f };
    ColorGrading colorGrading = null;
    Color targetColor = new Color{ r = 0.972549f, g = 0.86f, b = 0.85f, a = 1f };
    Color oldColor = new Color { r = 0.972549f, g = 0.86f, b = 0.85f, a = 1f };
    // Start is called before the first frame update
    void Start()
    {
        postVolume.profile.TryGetSettings(out colorGrading);
        //Añadimos el metodo en el delegado con el +=
        MusicManager.BeatUpdate += OnBeat;
        //Añadimos el metodo en el delegado con el +=
        MusicManager.MarkerUpdate += OnMarker;
    }

    private void Update()
    {
        //Transicion entre colores
        colorGrading.colorFilter.value = Color.Lerp(oldColor, targetColor, interpolationTime / 1.867f);
        interpolationTime += Time.deltaTime;
    }

    private void OnDestroy()
    {
        //Eliminamos el metodo del delegado con el -=
        MusicManager.BeatUpdate -= OnBeat;
        //Eliminamos el metodo del delegado con el -=
        MusicManager.MarkerUpdate -= OnMarker;
    }

    void OnBeat(int value)
    {
        Gamekit3D.CameraShake.Shake(.02f, .1f);
    }

    void OnMarker(string color)
    {
        oldColor = targetColor;
        //cambiamos el color dependiendo del nombre del marker en el evetno
        switch (color)
        {
            case "Red":
                targetColor = Color.red;
                break;
            case "Orange":
                targetColor = orange;
                break;
            case "Purple":
                targetColor = Color.magenta;
                break;
            case "Green":
                targetColor = Color.green;
                break;
            case "Blue":
                targetColor = Color.blue;
                break;
            case "Yellow":
                targetColor = Color.yellow;
                break;
            case "White":
                targetColor = Color.white;
                break;
            default:
                targetColor = defaultColor;
                break;
        }
        interpolationTime = 0;
    }
}
