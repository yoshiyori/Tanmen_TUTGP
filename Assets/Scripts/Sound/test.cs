using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour{
    [SerializeField] private CuePlayer cuePlayer;
    private bool isLoop = false;
    public float loopTime = 1f;

    private IEnumerator PlayStrechLoopCore(string cueName, float gameVariable = 0f, string selectorName = "", string selectorLabel = ""){
        while(isLoop){
            //多重再生防止
            //一回の再生が終わるたびにタイムストレッチの値が適用され、次の再生が始まる
            //if(!cuePlayer.JudgeCueStatus(cueName, CriAtomExPlayback.Status.Playing)){
                cuePlayer.player.SetDspTimeStretchRatio(loopTime);
                cuePlayer.player.UpdateAll();
                //Debug.Log(loopTime);
                cuePlayer.Play(cueName, gameVariable, selectorName, selectorLabel);
            //}
            yield return null;
        }
    }

    private void Reset(){
    }

    private void Start(){
        CueManager.singleton.AddTimeStrechVoicePool();
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            //cuePlayer.PlayStrechLoop("SwingBoost", 0f, "SwingBoost", "Increase");
            if(!isLoop){
                cuePlayer.player.SetVoicePoolIdentifier(CueManager.TIMESTRECH_VOICEPOOL);
                isLoop = true;
                StartCoroutine(PlayStrechLoopCore("SwingBoost", 0f, "SwingBoost", "Increase"));
            }
        }
        if(Input.GetKeyDown(KeyCode.Z)){
            isLoop = false;
        }
        if(Input.GetKeyDown(KeyCode.A)){
            cuePlayer.Play("SwingBoost", 0f, "SwingBoost", "Increase");
        }
        if(Input.GetKeyDown(KeyCode.S)){
            cuePlayer.PlayStrechLoop("SwingBoost", 0f, "SwingBoost", "Increase");
        }
    }
}
