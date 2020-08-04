using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test2 : MonoBehaviour{
    [SerializeField] private CuePlayer cuePlayer;
    float value = 0f;

    private void Reset(){
        //soundManager = FindObjectOfType<>();
        //cueManager = FindObjectOfType<CueManager>();
        //atom = FindObjectOfType<CriAtom>();
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.W)){
            cuePlayer.Play("Jump");
        }
        else if(Input.GetKeyDown(KeyCode.S)){
            SceneManager.LoadScene("10yen");
        }
    }
}
