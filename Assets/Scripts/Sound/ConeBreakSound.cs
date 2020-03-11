using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeBreakSound : MonoBehaviour{
    private bool coneDestroyed = false;
    [SerializeField] private CriAtomSource coneSound;
    [SerializeField] private GameObject parent;

    void Update(){
        if(coneDestroyed && coneSound.status.ToString().Equals("PlayEnd") && parent == null){
            //Debug.Log(coneSound.status);
            Destroy(this.gameObject);
        }
    }

    public void PlayAndDestroyed(){
        this.transform.parent = null;
        coneSound.Play();
        coneDestroyed = true;
    }
}
