using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

/**
 * <summary>キューの再生を簡易化するコンポーネント</summary>
 */
[AddComponentMenu("SoundSystem/Cue Player2")]
public class CuePlayer2 : CriAtomSource{
    //コンポーネント
    [SerializeField] private CueManager cueManager;

    //キュー関連
    [SerializeField] private string[] cueNames;
    [SerializeField] private CriAtomExPlayback[] criAtomExPlaybacks;

    //パラメーター
    [HideInInspector] public float loopTime;

    //コルーチン
    private IEnumerator DestroyAfterPlay(GameObject gameObject, string cueName){
        while(!JudgeCueStatus(cueName, CriAtomExPlayback.Status.Playing)){
            yield return null;
        }
        Destroy(gameObject);
    }

    private IEnumerator PlayStrechLoopCore(string cueName, float gameVariable = 0f, string selectorName = "", string selectorLabel = ""){
        while(loop){
            //多重再生防止
            //一回の再生が終わるたびにタイムストレッチの値が適用され、次の再生が始まる
            if(!JudgeCueStatus(cueName, CriAtomExPlayback.Status.Playing)){
                this.player.SetDspTimeStretchRatio(loopTime);
                //Debug.Log(loopTime);
                Play(cueName, gameVariable, selectorName, selectorLabel);
            }
            yield return null;
        }
    }

    /**
     * <summary>指定した名前のキューを再生</summary>
     * <param name = "cueName">再生したいキューの名前</param>
     * <param name = "gameVariable">ゲーム変数による変化を設定してる場合はここで値を指定</param>
     * <param name = "selectorLabel">セレクタラベルの指定</param>
     */
    public void Play(string cueName, float gameVariable = 0f, string selectorName = "", string selectorLabel = ""){
        //ゲーム変数の設定
        var cue = cueManager.GetCueSheetName(cueName);
        if(!cue.gameVariableName.Equals("")){
            CriAtomEx.SetGameVariable(cue.gameVariableName, gameVariable);
        }

        //セレクターの設定
        if(!selectorLabel.Equals("")){
            this.player.SetSelectorLabel(selectorName, selectorLabel);
        }

        //キューシートの設定
        this.cueSheet = cue.cueSheetName;

        //再生とCriAtomExPlaybackの設定
        var index = Array.IndexOf(cueNames, cueName);
        if(index >= 0){
            if(index < criAtomExPlaybacks.Length){
                criAtomExPlaybacks[index] = base.Play(cueName);
            }
        }
        else{
            Debug.LogWarning(cueName + " is not found.");
        }
    }

    /**
     * <summary>破壊されるオブジェクトで指定した名前のキューを再生</summary>
     * <param name = "cueName">再生したいキューの名前</param>
     * <param name = "mesh">音源となるオブジェクトのMeshFilter</param>
     * <param name = "Collider">音源となるオブジェクトのCollider</param>
     * <param name = "gameVariable">ゲーム変数による変化を設定してる場合はここで値を指定</param>
     * <param name = "selectorLabel">セレクタラベルの指定</param>
     */
    public void PlayAndDestroy(string cueName, ref MeshFilter mesh, ref Collider collider, float gameVariable = 0f, string selectorName = "", string selectorLabel = ""){
        Play(cueName, gameVariable, selectorName, selectorLabel);

        //見かけ上のみオブジェクトを破壊
        if(mesh != null){
            Destroy(mesh);
        }
        if(collider != null){
            Destroy(collider);
        }

        //音の再生が終了してから破壊
        StartCoroutine(DestroyAfterPlay(this.gameObject, cueName));
    }

    /**
     * <summary>タイムストレッチを用いてキューをループ再生(CuePayerのフィールド)</summary>
     * <param name = "cueName">再生したいキューの名前</param>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceが複数必要な場合はここで番号を指定する(追加のCriAtomSourceは自動で適用される)</param>
     * <param name = "gameVariable">ゲーム変数による変化を設定してる場合はここで値を指定</param>
     * <param name = "selectorLabel">セレクタラベルの指定</param>
     */
    public void PlayStrechLoop(string cueName, float gameVariable = 0f, string selectorName = "", string selectorLabel = ""){
        if(!loop){
            this.player.SetVoicePoolIdentifier(CueManager.TIMESTRECH_VOICEPOOL);
            loop = true;
            StartCoroutine(PlayStrechLoopCore(cueName, gameVariable, selectorName, selectorLabel));
        }
    }

    /**
     * <summary>ランダムなAISACコントロールの値を設定<summary>
     * <params name = "aisacControlName">値を設定したいAISACコントロールの名前<params>
     * <params name = "value">AISACコントロールの値<params>
     */
    public void SetRandomAisacControl(string aisacControlName, int atomSourceNum = 0){
        this.SetAisacControl(aisacControlName, UnityEngine.Random.value);
    }

    /**
     *<summary>再生しているキューの一時停止<summary>
     * <param name = "cueName">一時停止したいキューの名前</param>
     */
    public void Pause(string cueName){
        var index = Array.IndexOf(cueNames, cueName);
        if(index >= 0){
            if(index < criAtomExPlaybacks.Length){
                if(!criAtomExPlaybacks[index].IsPaused()){
                    criAtomExPlaybacks[index].Pause();
                }
                else{
                    Debug.Log(cueName + " is paused");
                }
            }
        }
        else{
            Debug.LogWarning(cueName + " is not found.");
        }
    }

    /**
     *<summary>一時停止しているキューの再開<summary>
     * <param name = "cueName">再開したいキューの名前</param>
     */
    public void Restart(string cueName){
        var index = Array.IndexOf(cueNames, cueName);
        if(index >= 0){
            if(index < criAtomExPlaybacks.Length){
                if(criAtomExPlaybacks[index].IsPaused()){
                    criAtomExPlaybacks[index].Resume(CriAtomEx.ResumeMode.PausedPlayback);
                }
                else{
                    Debug.Log(cueName + " is not paused");
                }
            }
        }
        else{
            Debug.LogWarning(cueName + " is not found.");
        }
    }

    /**
     * <summary>再生しているキューの停止<summary>
     * <param name = "cueName">停止したいキューの名前</param>
     */
    public void Stop(string cueName){
        var index = Array.IndexOf(cueNames, cueName);
        if(index >= 0){
            if(index < criAtomExPlaybacks.Length){
                criAtomExPlaybacks[index].Stop();
            }
        }
        else{
            Debug.LogWarning(cueName + " is not found.");
        }
    }

    /**
     * <summary>CriAtomSourceの再生状態を取得<summary>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceがある場合はここで番号を指定</param>
     * <returns>CriAtomSourceの再生状態<returns>
     */
    public CriAtomExPlayback.Status GetCueStatus(string cueName){
        var index = Array.IndexOf(cueNames, cueName);
        if(index >= 0 && index < criAtomExPlaybacks.Length){
            return criAtomExPlaybacks[index].GetStatus();
        }
        else{
            Debug.LogWarning(cueName + " is not found.");
            return CriAtomExPlayback.Status.Removed;
        }
    }

    /**
     * <summary>キューの再生状態を判定<summary>
     * <params name = "status">再生状態の名称がこの引数の値と同じかどうかを判定<params>
     * <return>再生状態と引数が一致するかどうか</return>
     */
    public bool JudgeCueStatus(string cueName, CriAtomExPlayback.Status status){
        return GetCueStatus(cueName).ToString().Equals(status);
    }

    void Awake(){
        this.InternalInitialize();
        criAtomExPlaybacks = new CriAtomExPlayback[cueNames.Length];
        
    }
}