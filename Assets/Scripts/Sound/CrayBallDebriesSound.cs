using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrayBallDebriesSound : MonoBehaviour{
    private bool coneDestroyed = false;
    [SerializeField] private CriAtomSource crayBallDebriesSound;

    void Update(){
        if(coneDestroyed && crayBallDebriesSound.status.ToString().Equals("PlayEnd")){
            //Debug.Log(coneSound.status);
            Destroy(this.gameObject);
        }
    }

    public void PlayAndDestroyed(){
        this.transform.parent = null;
        crayBallDebriesSound.Play();
        coneDestroyed = true;
    }
}
