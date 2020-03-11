using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrayBallDebriesSound : MonoBehaviour{
    //private bool crayBallDebriesDestroyed = false;
    [SerializeField] private CriAtomSource crayBallDebriesSound;
    [SerializeField] private GameObject parent;

    void Update(){
        if(/*crayBallDebriesDestroyed && */crayBallDebriesSound.status.ToString().Equals("PlayEnd") && parent == null){
            //Debug.Log("Destroy" + this.gameObject.name);
            Destroy(this.gameObject);
        }
    }

    public void PlayAndDestroyed(){
        this.transform.parent = null;
        crayBallDebriesSound.Play();
        //crayBallDebriesDestroyed = true;
    }
}
