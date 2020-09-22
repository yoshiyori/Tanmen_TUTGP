using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CuePlayer2D : MonoBehaviour{
    //コンポーネント
    [SerializeField] private CriAtomExPlayer player;
    //[SerializeField] private CueManager cueManager;
    //[SerializeField] private CriAtom criAtom;
    //[SerializeField] private CriWareInitializer criWareInitializer;
    //[SerializeField] private CriWareErrorHandler criWareErrorHandler;

    //キュー名
    [SerializeField] internal List<string> cueNameList = new List<string>();

    //パラメーター
    [SerializeField] private bool playOnStart = false;
    [SerializeField] internal string playCueNameOnStart = "";

    [SerializeField] private CriAtomExPlayback[] criAtomExPlaybacks;

    //コルーチン
    private IEnumerator DestroyAfterPlay(GameObject gameObject, string cueName){
        while(!JudgeCueStatus(cueName, CriAtomExPlayback.Status.Playing)){
            yield return null;
        }
        Destroy(gameObject);
    }

    private IEnumerator StopFadeoutCore(string cueName, string aisacControlName, float fadeTime){
        fadeTime = fadeTime < 0.1f ? 0.1f : fadeTime;
        for(float t = 0f; t < fadeTime; t += Time.deltaTime){
            player.SetAisacControl(aisacControlName, Mathf.Lerp(1f, 0f, Mathf.Clamp01(t / fadeTime)));
            //UpdateCue(cueName);
            player.UpdateAll();
            yield return null;
        }
        Stop(cueName);
    }

    /**
     * <summary>指定した名前のキューを再生</summary>
     * <param name = "cueName">再生したいキューの名前</param>
     * <param name = "gameVariable">ゲーム変数による変化を設定してる場合はここで値を指定</param>
     * <param name = "selectorLabel">セレクタラベルの指定</param>
     */
    public void Play(string cueName, float gameVariable = 0f, string selectorName = "", string selectorLabel = ""){
        //ゲーム変数の設定
        var cue = CueManager.singleton.GetCueSheetName(cueName);
        if(!cue.gameVariableName.Equals("")){
            CriAtomEx.SetGameVariable(cue.gameVariableName, gameVariable);
        }

        //セレクターの設定
        if(!selectorLabel.Equals("")){
            this.player.SetSelectorLabel(selectorName, selectorLabel);
        }

        //キューシートの設定
        player.SetCue(CriAtom.GetAcb(cue.cueSheetName), cue.cueName);

        //再生とCriAtomExPlaybackの設定
        var index = cueNameList.IndexOf(cueName);
        if(index >= 0){
            if(index < criAtomExPlaybacks.Length){
                criAtomExPlaybacks[index] = player.Start();
            }
        }
        else{
            Debug.LogWarning(cueName + " is not found.");
        }
    }

    public void PlayWithFadeSetting(string cueName, int fadeTime, float gameVariable = 0f, string selectorName = "", string selectorLabel = ""){
        player.AttachFader();
        player.SetFadeOutTime(fadeTime);
        Play(cueName, gameVariable, selectorName, selectorLabel);
    }

    /**
     *<summary>シーンを移動する際にキューを再生させる<summary>
     * <param name = "cueName">再生したいキューの名前</param>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceがある場合はここで番号を指定</param>
     * <param name = "gameVariable">ゲーム変数による変化を設定してる場合はここで値を指定</param>
     */
    public void PlayOnSceneSwitch(string cueName, float gameVariable = 0f, string selectorName = "", string selectorLabel = ""){
        DontDestroyOnLoad(this.gameObject);
        Play(cueName, gameVariable, selectorName, selectorLabel);

        //音の再生が終了したら破壊
        StartCoroutine(DestroyAfterPlay(this.gameObject, cueName));
    }

    /**
     * <summary>ランダムなAISACコントロールの値を設定<summary>
     * <params name = "aisacControlName">値を設定したいAISACコントロールの名前<params>
     * <params name = "aisacControlValue">AISACコントロールの値<params>
     */
    public void SetAisacControl(string aisacControlName, float aisacControlValue){
        player.SetAisacControl(aisacControlName, aisacControlValue);
    }

    /**
     * <summary>ランダムなAISACコントロールの値を設定<summary>
     * <params name = "aisacControlName">値を設定したいAISACコントロールの名前<params>
     */
    public void SetRandomAisacControl(string aisacControlName){
        player.SetAisacControl(aisacControlName, UnityEngine.Random.value);
    }

    public void UpdateCue(string cueName){
        //再生とCriAtomExPlaybackの設定
        var index = cueNameList.IndexOf(cueName);
        if(index >= 0){
            if(index < criAtomExPlaybacks.Length){
                player.Update(criAtomExPlaybacks[index]);
            }
        }
        else{
            Debug.LogWarning(cueName + " is not found.");
        }
    }

    public void UpdatePlayer(){
        player.UpdateAll();
    }

    /**
     * <summary>キューの再生状態を取得<summary>
     * <param name = "cueName">再生状態を取得したいキューの名前</param>
     * <returns>キューの再生状態<returns>
     */
    public CriAtomExPlayback.Status GetCueStatus(string cueName){
        var index = cueNameList.IndexOf(cueName);
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

    /**
     *<summary>再生しているキューの一時停止<summary>
     * <param name = "cueName">一時停止したいキューの名前</param>
     */
    public void Pause(string cueName){
        var index = cueNameList.IndexOf(cueName);
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
        var index = cueNameList.IndexOf(cueName);
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
        var index = cueNameList.IndexOf(cueName);
        if(index >= 0){
            if(index < criAtomExPlaybacks.Length){
                criAtomExPlaybacks[index].Stop();
            }
        }
        else{
            Debug.LogWarning(cueName + " is not found.");
        }
    }

    public void StopFadeout(){
        //StartCoroutine(StopFadeoutCore(cueName, aisacControlName, fadeTime));
        player.Stop(false);
        player.DetachFader();
    }

    void Reset(){
        //cueManager = GetComponent<CueManager>();
        //criAtom = GetComponent<CriAtom>();
        //criWareErrorHandler = GetComponent<CriWareErrorHandler>();
        //criWareInitializer = GetComponent<CriWareInitializer>();
    }

    private void Awake(){
        player = new CriAtomExPlayer();
        criAtomExPlaybacks = new CriAtomExPlayback[cueNameList.Count];
    }

    //PlayOnStartの実行
    private void Start(){
        if(playOnStart){
            Play(playCueNameOnStart);
        }
    }
}
