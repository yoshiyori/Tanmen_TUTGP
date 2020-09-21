using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour{
    [SerializeField] private CuePlayer cuePlayer;
    float value = 0f;

    private void Reset(){
        //soundManager = FindObjectOfType<>();
        //cueManager = FindObjectOfType<CueManager>();
        //atom = FindObjectOfType<CriAtom>();
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.W)){
            cuePlayer.Play("Increase");
        }
    }
}
