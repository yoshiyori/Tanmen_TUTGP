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

    private void Start(){
        cuePlayer.Play("SwingBoost", 0, 0f, "Decrease");
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.W)){
            value += 0.1f;
        }
        else if(Input.GetKeyDown(KeyCode.S)){
            value -= 0.1f;
        }

        Debug.Log(value);
        cuePlayer.SetAisacControl("SwingBoost", value);
    }
}
