using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSample : MonoBehaviour{
    //コンポーネント
    [SerializeField] private CueManager cueManager;
    
    //フラグ
    private bool pauseNow = false;

    void Reset(){
        cueManager = (CueManager)FindObjectOfType(typeof(CueManager));
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.P) && !pauseNow){
            //キューシート「Game_SE」に属するキューの再生を全て一時停止する
            //一時停止中に再度、再生要求があると再生を再開するため注意 (後々修正するかも)
            cueManager.PauseCueSheet("GameSE");
            pauseNow = true;
        }
        else if(Input.GetKeyDown(KeyCode.P) && pauseNow){
            //キューシート「Game_SE」に属するキューの再生を全て再開するする
            cueManager.RestartCueSheet("GameSE");
            pauseNow = false;
        }
    }
}
