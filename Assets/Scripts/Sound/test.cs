using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour{
    [SerializeField] private CuePlayer2 cuePlayer;
    float value = 0f;

    private void Reset(){
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.W)){
            cuePlayer.Play("Wind");
        }
        if(Input.GetKeyDown(KeyCode.A)){
            cuePlayer.Play("Increase");
            cuePlayer.Stop("Wind");
        }
    }
}
