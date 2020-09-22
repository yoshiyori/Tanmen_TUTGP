using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour{
    [SerializeField] private CuePlayer cuePlayer;
    private bool isLoop = false;

    private void Reset(){
    }

    private void Start(){
        cuePlayer.loopTime = 1f;
        CueManager.singleton.AddTimeStrechVoicePool();
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.A)){
            cuePlayer.PlayStrechLoop("SwingBoost", 0f, "SwingBoost", "Increase");
        }
        if(Input.GetKeyDown(KeyCode.S)){
            cuePlayer.loopTime -= 0.1f;
        }
        if(Input.GetKeyDown(KeyCode.W)){
            cuePlayer.loopTime += 0.1f;
        }
        Debug.Log(cuePlayer.loopTime);
    }
}
