﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CuePlayer2D : MonoBehaviour{
    //コンポーネント
    [SerializeField] private List<CriAtomExPlayer> criAtomExPlayerList = new List<CriAtomExPlayer>();
    [SerializeField] private CueManager cueManager;
    [SerializeField] private CriAtom criAtom;
    [SerializeField] private CriWareInitializer criWareInitializer;
    [SerializeField] private CriWareErrorHandler criWareErrorHandler;

    //キュー名
    [SerializeField] internal List<string> cueNameList = new List<string>();

    //パラメーター
    [SerializeField] private bool playOnStart = false;
    [SerializeField] internal string playCueNameOnStart = "";

#if UNITY_EDITOR
    //Ediotr以外からのアクセスを防ぐプロパティたち
    public CueManager CueManager{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return cueManager;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            cueManager = value;
        }
    }
    
    public CriAtom CriAtom{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return criAtom;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            criAtom = value;
        }
    }
    
    public CriWareInitializer CriWareInitializer{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return criWareInitializer;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            criWareInitializer = value;
        }
    }
    
    public CriWareErrorHandler CriWareErrorHandler{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return criWareErrorHandler;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            criWareErrorHandler = value;
        }
    }
    public List<string> CueNameList{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return cueNameList;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            cueNameList = value;
        }
    }

    public bool PlayOnStart{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return playOnStart;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            playOnStart = value;
        }
    }

    public string PlayCueNameOnStart{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return playCueNameOnStart;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            playCueNameOnStart = value;
        }
    }
#endif

    //コルーチン
    private IEnumerator destroyAfterPlay;
    private IEnumerator DestroyAfterPlay(GameObject gameObject, int exPlayerNum = 0){
        while(!JudgeAtomSourceStatus("PlayEnd", exPlayerNum)){
            yield return null;
        }
        Destroy(gameObject);
    }

    /**
     * <summary>指定した名前のキューを再生</summary>
     * <param name = "cueName">再生したいキューの名前</param>
     * <param name = "exPlayerNum">1つのオブジェクトにCriAtomSourceが複数必要な場合はここで番号を指定する
     * (キューごとに異なるタイミングで再生を止めたりなどの操作をする場合はCriAtomSourceが複数必要になる。追加のCriAtomSourceは自動で適用される。)</param>
     * <param name = "gameVariable">ゲーム変数による変化を設定してる場合はここで値を指定</param>
     */
    public void Play(string cueName, int exPlayerNum = 0, float gameVariable = 0f){
        //指定された番号のAtomSourceがない場合は追加
        if(criAtomExPlayerList.Count <= exPlayerNum){
            criAtomExPlayerList.Add(InitializeAtomExPlayer());
        }

        //ゲーム変数の設定
        var cue = cueManager.GetCueSheetName(cueName);
        if(!cue.gameVariableName.Equals("")){
            CriAtomEx.SetGameVariable(cue.gameVariableName, gameVariable);
        }

        //キューシートの設定と再生
        criAtomExPlayerList[exPlayerNum].SetCue(CriAtom.GetAcb(cue.cueSheetName), cue.cueName);
        criAtomExPlayerList[exPlayerNum].Start();
    }

    /**
     *<summary>シーンを移動する際にキューを再生させる<summary>
     * <param name = "cueName">再生したいキューの名前</param>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceがある場合はここで番号を指定</param>
     * <param name = "gameVariable">ゲーム変数による変化を設定してる場合はここで値を指定</param>
     */
    public void PlayOnSceneSwitch(string cueName, int exPlayerNum = 0, float gameVariable = 0f){
        DontDestroyOnLoad(this.gameObject);
        Play(cueName, exPlayerNum, gameVariable);

        //不要なコンポーネントを削除
        Destroy(cueManager);
        //Destroy(criAtom);

        //音の再生が終了したら破壊
        destroyAfterPlay = DestroyAfterPlay(this.gameObject);
        StartCoroutine(destroyAfterPlay);
    }

    /**
     * <summary>CriAtomSourceの再生状態を取得<summary>
     * <param name = "exPlayerNum">1つのオブジェクトにCriAtomSourceがある場合はここで番号を指定</param>
     * <returns>CriAtomSourceの再生状態<returns>
     */
    public CriAtomExPlayer.Status GetAtomSourceStatus(int exPlayerNum = 0){
        if(criAtomExPlayerList.Count <= exPlayerNum){
            criAtomExPlayerList.Add(InitializeAtomExPlayer());
        }
        return criAtomExPlayerList[exPlayerNum].GetStatus();
    }

    /**
     * <summary>CriAtomSourceの再生状態を判定<summary>
     * <params status = "status">再生状態の名称がこの引数の値と同じかどうかを判定<params>
     * <return>再生状態と引数が一致するかどうか</return>
     */
    public bool JudgeAtomSourceStatus(string status, int exPlayerNum = 0){
        return GetAtomSourceStatus(exPlayerNum).ToString().Equals(status);
    }
    
    //CriAtomSourceの追加
    private CriAtomExPlayer InitializeAtomExPlayer(){
        return new CriAtomExPlayer();
    }

    void Reset(){
        cueManager = GetComponent<CueManager>();
        criAtom = GetComponent<CriAtom>();
        criWareErrorHandler = GetComponent<CriWareErrorHandler>();
        criWareInitializer = GetComponent<CriWareInitializer>();
    }

    private void Awake(){
        if(cueManager == null){
            cueManager = (CueManager)FindObjectOfType(typeof(CueManager));
        }
    }

    //PlayOnStartの実行
    private void Start(){
        if(playOnStart){
            if(criAtomExPlayerList.Count < 1){
                criAtomExPlayerList.Add(InitializeAtomExPlayer());
            }
            
            criAtomExPlayerList[0].SetCue(CriAtom.GetAcb(cueManager.GetCueSheetName(playCueNameOnStart).cueSheetName), playCueNameOnStart);
            criAtomExPlayerList[0].Start();
        }
    }
}