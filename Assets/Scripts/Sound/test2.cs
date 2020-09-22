using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test2 : MonoBehaviour{
    [SerializeField] private CuePlayer cuePlayer;
    float value = 0f;

    private void Reset(){
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.W)){
            cuePlayer.Stop("Running");
        }
    }
}
