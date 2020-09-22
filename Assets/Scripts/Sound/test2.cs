using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test2 : MonoBehaviour{
    [SerializeField] private CuePlayer2D cuePlayer;
    float value = 0f;

    private void Reset(){
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.S)){
            Debug.Log(CueManager.singleton.exCueInfoList[33].CueName);
            cuePlayer.Play("Decision");
        }
    }
}
