using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using System.Runtime.InteropServices;
using System;
using FMOD;
using Unity.VisualScripting;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public delegate void BeatEventDelegate(int beat);
    public delegate void MarkerEventDelegate(string markerName);

    public static event BeatEventDelegate BeatUpdate;
    public static event MarkerEventDelegate MarkerUpdate;

    int lastBeat = 0;
    string lastMarker;

    //Path del evento
    [SerializeField] EventReference battleMusicPath;

    //Path del parametro global
    [ParamRef]
    [SerializeField]
    string battleMusicParam;

    //Id del parametro global
    PARAMETER_ID paramID;

    //Aqui guardaremos la descripcion del parametro global del parametro global
    PARAMETER_DESCRIPTION paramDes;

    //Donde guardamos el evento
    EventInstance battleMusic;

    //le dira a unity que esta clase tendra una secuencia predecible e inmovible en la memoria
    [StructLayout(LayoutKind.Sequential)]
    //clase donde vamos a manejar los callbacks del battleMusic
    class TimeLineInfo
    {
        //El pulso actual
        public int currenBeat = 0;
        //El compas Actual
        public int currentBar = 0;
        //La posicion Actua
        public int currentPosition = 0;
        //El tempo
        public float currentBPM = 0;
        //Cuantos pulsos por compas
        public int currentUperTimeSignature = 0;
        //La sub divicion por compas
        public int currentLoweTimeSignature = 0;
        //Nos da el nombre del ultimo marker del evento
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
        //El numero del marker
        public int lastMarkerPosition = 0;

    }

    //Nuestra clase
    TimeLineInfo timeLineInfo;

    //Pinea nuestra clase a una seccion de memoria libre
    GCHandle timeLineHandle;

    //Guarda nuestros callbacks
    EVENT_CALLBACK beatCallBack;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //Buscamos el ID del parametro global usando el StudioSystem.gerParameterDescriptionName, le damos el nmbre y una variable que contendra la informacion
        RuntimeManager.StudioSystem.getParameterDescriptionByName(battleMusicParam, out paramDes);

        //Guardamos el ID del parametro global
        paramID = paramDes.id;

        timeLineInfo = new TimeLineInfo();

        beatCallBack = new EVENT_CALLBACK(BeatEventCallBack);
        //Pinea el objeto gestionado (timeLineInfo) en elGCHandle llamado timeLineHandle 
        timeLineHandle = GCHandle.Alloc(timeLineInfo, GCHandleType.Pinned);

        //Le decimos que evetno vamos a guardar en la instancia
        battleMusic = RuntimeManager.CreateInstance(battleMusicPath);

        //Convertimos el timeLineHandle en un puntero y almacenaremos la informacion de ese puntero en el user data del evento de la pelea final
        //Permite que la informacion gestionada en nuestro GCHandle timeLineHndle este disponible dentro de FMOD
        battleMusic.setUserData(GCHandle.ToIntPtr(timeLineHandle));

        //configura un callback que se ejecutará cuando la música en battleMusic llegue a un beat o a un marcador específico en su línea de tiempo.
        battleMusic.setCallback(beatCallBack, EVENT_CALLBACK_TYPE.TIMELINE_BEAT | EVENT_CALLBACK_TYPE.TIMELINE_MARKER);

    }

    private void Update()
    {
        //Si mi int no tiene el mismo valor que mi beat actualizalo y haz que tengan el mismo valor
        if (lastBeat != timeLineInfo.currenBeat)
        {
            lastBeat = timeLineInfo.currenBeat;
            //Si mi delegado no es nulo
            if (BeatUpdate != null)
            {
                //Entonces colocale mi beat actual
                BeatUpdate(timeLineInfo.currenBeat);
            }

        }
        //Si mi int no tiene el mismo valor que mi beat actualizalo y guarda el valor del beat
        if (lastMarker != timeLineInfo.lastMarker)
        {
            lastMarker = timeLineInfo.lastMarker;
            //Si mi delegado no es nulo entonces actualizamelo con el valor del ultimo marker
            if (MarkerUpdate != null)
            {
                MarkerUpdate(timeLineInfo.lastMarker);
            }

        }

    }

    private void OnDestroy()
    {
        //Libera la informacion gestionada
        battleMusic.setUserData(IntPtr.Zero);
        battleMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        battleMusic.release();
        //Liberamos el GChandle de la memoria gestionada
        timeLineHandle.Free();
    }

    public void SetGlobalMusicParameter(float value)
    {
        //Si el valor del parametro es 1 entonces que empiece la musica
        if(value == 1)
        {
            StartBattleMusic();
        }
        RuntimeManager.StudioSystem.setParameterByID(paramID, value);
    }

    void StartBattleMusic()
    {
        //Guardamos el estado de la musica en una variable
        battleMusic.getPlaybackState(out PLAYBACK_STATE pb);

        //Si la musica no esta sonanado o esta parando entonces que empiece la musica
        if(pb == PLAYBACK_STATE.STOPPED || pb == PLAYBACK_STATE.STOPPING)
        {
            battleMusic.start();
        }
    }

    static FMOD.RESULT BeatEventCallBack(EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        //Creo un nuevo event isntance que contenga el puntero "isntancePtr"
        EventInstance instance = new EventInstance(instancePtr);

        //vamos a acceder a los datos adicionales de esa instancia y los guardaremos en el "timeLineInfoPtr"
        FMOD.RESULT result = instance.getUserData(out IntPtr timeLineInfoPtr);

        //revision de segurdad
        //Si el resultado de el data de la instancia es diferente a "FMOD.RESULT.OK" entonces mandame un error
        if (result != FMOD.RESULT.OK)
        {
            //Esto manda un error en consola.
            //Usamos el UnityEngine debido a que hay que diferenciarlo del de FMOD
            UnityEngine.Debug.LogError("Timeline Callback Error: " + result);
        }
        //si todo esta bien entonces verifica que el valor de nuestro timeLineInfoPtr sea diferente a cero
        //Es el equivalente a "!= null" en IntPtr
        else if (timeLineInfoPtr != IntPtr.Zero)
        {
            //Creamos un GCHandle a partir de un puntero y lo guardamos en otro llamado "timeLineHandle"
            GCHandle timeLineHandle = GCHandle.FromIntPtr(timeLineInfoPtr);

            //el Target permite acceder al objeto que esta siendo gestionado por el GCHandle en este caso "timeLineInfo"
            //Este objeto gestionado lo convertimos en tipo "TimeLineInfo" con los parentesis
            //Y lo guardamos en una variable de ese mismo tipo
            TimeLineInfo timeLineInfo = (TimeLineInfo)timeLineHandle.Target;

            //La declaracion "switch" se usa para escoger uno de varios bloques que se ejecutaran
            switch (type)
            {
                //El valor de la variable "type" sera comparada con el valor del caso ("Case")
                case EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        //Guardaremos la informacion del beat |                        Marshal.PtrToStructure:Convierte nuestro puntero "parameterPtr" en un Objeto de memoria gestionada
                        //                                                             typof: Devuelve el tipo (Type) que contiene informacion sobre el tipo especificado, como propiedades, metodos...
                        TIMELINE_BEAT_PROPERTIES parameter = (TIMELINE_BEAT_PROPERTIES) Marshal.PtrToStructure(parameterPtr, typeof(TIMELINE_BEAT_PROPERTIES));

                        //Guardamos el beat en nuestro int dentro de la clase
                        timeLineInfo.currenBeat = parameter.beat;
                        //Guardamos el compas en el que estamos actualemte
                        timeLineInfo.currentBar = parameter.bar;
                        //guardamos el valor del tempo
                        timeLineInfo.currentBPM = parameter.tempo;
                        //Guardaamos la posicion del evento
                        timeLineInfo.currentPosition = parameter.position;
                        //Guardamos los pulsos por compas
                        timeLineInfo.currentUperTimeSignature = parameter.timesignatureupper;
                        //Guardamos la Subdivision por compas
                        timeLineInfo.currentLoweTimeSignature = parameter.timesignaturelower;
                    }
                    //Para la ejecucion del codigo
                    break;
                //El valor de la variable "type" sera comparada con el valor del caso ("Case")
                case EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                    //Guardaremos la informacion de los Markers |                     Marshal.PtrToStructure: Convierte nuestro puntero "parameterPtr" en un Objeto de memoria gestionada
                   //                                                                 typof: Devuelve el tipo(Type) que contiene informacion sobre el tipo especificado, como propiedades, metodos...
                        TIMELINE_MARKER_PROPERTIES parameter = (TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(TIMELINE_MARKER_PROPERTIES));

                        //Guardamos nuestro marker en nuestro StringWrapper de la clase
                        timeLineInfo.lastMarker = parameter.name;
                        //El numero del marker
                        timeLineInfo.lastMarkerPosition = parameter.position;
                    }
                    //Para la ejecucion del codigo
                    break;
            }
            
        }
        //Si todo salio bien retornamos que el sesultado salio bien
        return RESULT.OK;
    }

    //Solo ejecuta esta seccion de codigo dentro del editor de unity y no cuando este se exporte
#if UNITY_EDITOR
    //Sirve para monitorear los parametros en la ui de unity
    private void OnGUI()
    {
        //Crea una caja que se pondra en la esquina superior izquierda y muestra la informacion que queramos
        GUILayout.Box(String.Format("CurrentBeat = {0}, lastMarker = {1}", timeLineInfo.currenBeat, (String)timeLineInfo.lastMarker));
        GUILayout.Box(String.Format("CurrentBPM = {0}, CurrentTempo = {1}/{2}", timeLineInfo.currentBPM, timeLineInfo.currentUperTimeSignature, timeLineInfo.currentLoweTimeSignature));
        GUILayout.Box(String.Format("CurrentPosition = {0}, currentBar = {1}", timeLineInfo.currentPosition, timeLineInfo.currentBar));
        GUILayout.Box(String.Format("MarkerPosition = {0}", timeLineInfo.lastMarkerPosition));
    }
#endif
}
