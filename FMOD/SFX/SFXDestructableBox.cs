using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXDestructableBox : MonoBehaviour
{
    public void DestructableBoxEvent(string destructableBoxPath)
    {
        RuntimeManager.PlayOneShot(destructableBoxPath, transform.position);
    }
}
