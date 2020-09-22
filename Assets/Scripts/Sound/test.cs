using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour{
    [SerializeField] private CuePlayer2D cuePlayer2D;
    private bool isLoop = false;
    public float loopTime = 1f;

    private void Reset(){
    }

    private void Start(){
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.A)){
            cuePlayer2D.PlayWithFadeSetting("TitleBGM", 5000);
        }
        if(Input.GetKeyDown(KeyCode.S)){
            cuePlayer2D.StopFadeout();
        }
        if(Input.GetKeyDown(KeyCode.D)){
            cuePlayer2D.SetAisacControl("GameBGMFade", 0f);
            //cuePlayer2D.UpdateCue("TitleBGM");
            cuePlayer2D.UpdatePlayer();
        }
        if(Input.GetKeyDown(KeyCode.W)){
            cuePlayer2D.SetAisacControl("GameBGMFade", 0.1f);
            cuePlayer2D.Play("TitleBGM");
        }
    }
}
