using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeBreakSound : MonoBehaviour{
    private bool coneDestroyed = false;
    [SerializeField] private CriAtomSource coneSound;

    void Update(){
        if(coneDestroyed && coneSound.status.ToString().Equals("PlayEnd")){
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
