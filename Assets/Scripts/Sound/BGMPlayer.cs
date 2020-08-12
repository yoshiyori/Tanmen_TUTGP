using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour{
    [SerializeField] private CuePlayer2D cuePlayer2D;
    [SerializeField] private string playOnStart;
    void Start(){
        if(playOnStart != null && !playOnStart.Equals("")){
            cuePlayer2D.Play(playOnStart);
        }
    }

    void Update(){
        
    }
}
