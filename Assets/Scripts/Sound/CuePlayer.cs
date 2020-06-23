using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/**
 * <summary>キューの再生を簡易化するコンポーネント</summary>
 */
[RequireComponent(typeof(CriAtomSource)), AddComponentMenu("SoundSystem/Cue Player")]
public class CuePlayer : MonoBehaviour{
    //コンポーネント
    [SerializeField] private List<CriAtomSource> criAtomSourceList = new List<CriAtomSource>();
    [SerializeField] private CueManager cueManager;

    //キュー名
    [SerializeField] internal List<string> cueNameList = new List<string>();

    //パラメーター
    [SerializeField] private bool playOnStart = false;
    [SerializeField] internal string playCueNameOnStart = "";
    [HideInInspector] public bool loop = false;
    [HideInInspector] public float loopTime;

    //その他
    private CriAtomExStandardVoicePool voicePool;

#if UNITY_EDITOR
    //Editor以外からフィールドにアクセスすることを防ぐプロパティたち
    public List<CriAtomSource> CriAtomSourceList{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return criAtomSourceList;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            criAtomSourceList = value;
        }
    }
    
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

    /**
     * <summary>ゲームオブジェクトに付与されているCriAtomSourceの数(読み取り専用)</summary>
     */
    public int criAtomSourceNum{
        get{
            return criAtomSourceList.Count;
        }
    }
    
    //コルーチン
    private IEnumerator DestroyAfterPlay(GameObject gameObject, int atomSourceNum = 0){
        while(!JudgeAtomSourceStatus("PlayEnd", atomSourceNum)){
            yield return null;
        }
        Destroy(gameObject);
    }

    private IEnumerator PlayStrechLoopCore(string cueName, int atomSourceNum = 0, float gameVariable = 0f, string selectorLabel = ""){
        while(loop){
            if(!JudgeAtomSourceStatus("Playing", atomSourceNum)){
                criAtomSourceList[atomSourceNum].player.SetDspTimeStretchRatio(loopTime);
                Play(cueName, atomSourceNum, gameVariable, selectorLabel);
            }
            yield return null;
        }
    }

    private IEnumerator DetachInsertionDSP(int atomSourceNum = 0){
        while(true){
            if(!JudgeAtomSourceStatus("Playing", atomSourceNum)){
                voicePool.DetachDsp();
                break;
            }
            yield return null;
        }
    }

    private IEnumerator DisposeStandardVoicePoolCore(int atomSourceNum = 0){
        while(true){
            if(!JudgeAtomSourceStatus("Playing", atomSourceNum)){
                voicePool.Dispose();
                break;
            }
            yield return null;
        }
    }

    /**
     * <summary>指定した名前のキューを再生</summary>
     * <param name = "cueName">再生したいキューの名前</param>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceが複数必要な場合はここで番号を指定する
     * (キューごとに異なるタイミングで再生を止めたりなどの操作をする場合はCriAtomSourceが複数必要になる。追加のCriAtomSourceは自動で適用される。)</param>
     * <param name = "gameVariable">ゲーム変数による変化を設定してる場合はここで値を指定</param>
     * <param name = "selectorLabel">セレクタラベルの指定</param>
     */
    public void Play(string cueName, int atomSourceNum = 0, float gameVariable = 0f, string selectorLabel = ""){
        //指定された番号のAtomSourceがない場合は追加
        if(criAtomSourceList.Count <= atomSourceNum){
            criAtomSourceList.Add(InitializeAtomSource());
        }

        //ゲーム変数の設定
        var cue = cueManager.GetCueSheetName(cueName);
        if(!cue.gameVariableName.Equals("")){
            CriAtomEx.SetGameVariable(cue.gameVariableName, gameVariable);
        }

        //セレクターの設定
        if(!selectorLabel.Equals("")){
            criAtomSourceList[atomSourceNum].player.SetSelectorLabel(cueName, selectorLabel);
        }

        //キューシートの設定と再生
        criAtomSourceList[atomSourceNum].cueSheet = cue.cueSheetName;
        criAtomSourceList[atomSourceNum].Play(cueName);
    }

    /**
     * <summary>破壊されるオブジェクトで指定した名前のキューを再生</summary>
     * <param name = "cueName">再生したいキューの名前</param>
     * <param name = "mesh">音源となるオブジェクトのMeshFilter</param>
     * <param name = "Collider">音源となるオブジェクトのCollider</param>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceが複数必要な場合はここで番号を指定する(追加のCriAtomSourceは自動で適用される)</param>
     * <param name = "gameVariable">ゲーム変数による変化を設定してる場合はここで値を指定</param>
     * <param name = "selectorLabel">セレクタラベルの指定</param>
     */
    public void PlayAndDestroy(string cueName, ref MeshFilter mesh, ref Collider collider, int atomSourceNum = 0, float gameVariable = 0f, string selectorLabel = ""){
        Play(cueName, atomSourceNum, gameVariable, selectorLabel);

        //見かけ上のみオブジェクトを破壊
        if(mesh != null){
            Destroy(mesh);
        }
        if(collider != null){
            Destroy(collider);
        }

        //音の再生が終了してから破壊
        StartCoroutine(DestroyAfterPlay(this.gameObject));
    }

    /**
     * <summary>タイムストレッチを用いてキューをループ再生(CuePayerのフィールド)</summary>
     * <param name = "cueName">再生したいキューの名前</param>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceが複数必要な場合はここで番号を指定する(追加のCriAtomSourceは自動で適用される)</param>
     * <param name = "gameVariable">ゲーム変数による変化を設定してる場合はここで値を指定</param>
     * <param name = "selectorLabel">セレクタラベルの指定</param>
     */
    public void PlayStrechLoop(string cueName, int atomSourceNum = 0, float gameVariable = 0f, string selectorLabel = ""){
        //指定された番号のAtomSourceがない場合は追加
        if(criAtomSourceList.Count <= atomSourceNum){
            criAtomSourceList.Add(InitializeAtomSource());
        }

        if(!loop){
            //タイムストレッチ準備
            if(voicePool == null){
                voicePool = new CriAtomExStandardVoicePool(2, 2, 48000, false, 2);
                voicePool.AttachDspTimeStretch();
                criAtomSourceList[atomSourceNum].player.SetVoicePoolIdentifier(2);
            }

            loop = true;
            StartCoroutine(PlayStrechLoopCore(cueName, atomSourceNum, gameVariable, selectorLabel));
        }
    }

    /**
     * <summary>指定したAISACコントロールの値を設定<summary>
     * <params name = "aisacControlName">値を設定したいAISACコントロールの名前<params>
     * <params name = "value">AISACコントロールの値<params>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceがある場合はここで番号を指定</param>
     */
    public void SetAisacControl(string aisacControlName, float value, int atomSourceNum = 0){
        //指定された番号のAtomSourceがない場合は追加
        if(criAtomSourceList.Count <= atomSourceNum){
            criAtomSourceList.Add(InitializeAtomSource());
        }

        criAtomSourceList[atomSourceNum].SetAisacControl(aisacControlName, value);
    }

    /**
     * <summary>指定したAtomSourceの保持するキューのセレクタラベルを変更<summary>
     * <params name = "cueName">セレクタラベルを変更するキューの名前<params>
     * <params name = "selectorLabel">指定するセレクタラベル<params>
     * <param name = "atomSourceNum">指定したキューを保持するAtomSourceの番号を指定</param>
     */
    public void SwitchSelector(string cueName, string selectorLabel, int atomSourceNum = 0){
        criAtomSourceList[atomSourceNum].player.SetSelectorLabel(cueName, selectorLabel);
    }

    /**
     * <summary>指定したAtomSourceのインサーションDSP設定を初期化<summary>
     * <param name = "atomSourceNum">指定したキューを保持するAtomSourceの番号を指定</param>
     */
    public void ResetInsertionDSP(int atomSourceNum = 0){
        //指定された番号のAtomSourceがない場合は追加
        if(criAtomSourceList.Count <= atomSourceNum){
            criAtomSourceList.Add(InitializeAtomSource());
        }

        criAtomSourceList[atomSourceNum].player.SetDspTimeStretchRatio(1f);
        criAtomSourceList[atomSourceNum].player.SetDspPitchShifterPitch(1f);
    }

    /**
     *<summary>再生しているキューの一時停止(CriAtomSourceごとの停止)<summary>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceがある場合はここで番号を指定</param>
     */
    public void Pause(int atomSourceNum = 0){
        if(criAtomSourceList.Count <= atomSourceNum){
            criAtomSourceList.Add(InitializeAtomSource());
        }
        criAtomSourceList[atomSourceNum].Pause(true);
    }


    /**
     *<summary>一時停止しているキューの再開(CriAtomSourceごとの再開)<summary>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceがある場合はここで番号を指定</param>
     */
    public void Restart(int atomSourceNum = 0){
        if(criAtomSourceList.Count <= atomSourceNum){
            criAtomSourceList.Add(InitializeAtomSource());
        }
        criAtomSourceList[atomSourceNum].Pause(false);
    }

    /**
     * <summary>再生しているキューの停止(CriAtomSourceごとの停止)<summary>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceがある場合はここで番号を指定</param>
     */
    public void Stop(int atomSourceNum = 0){
        if(criAtomSourceList.Count <= atomSourceNum){
            criAtomSourceList.Add(InitializeAtomSource());
        }
        criAtomSourceList[atomSourceNum].Stop();
    }

    /**
     * <summary>タイムストレッチを利用したループ再生の終了(CriAtomSourceごとの停止)<summary>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceがある場合はここで番号を指定</param>
     * <param name = "detachInsertionDSP">インサーション</param>
     */
    public void StopStrechLoop(int atomSourceNum = 0, bool detachInsertionDSP = true){
        if(loop){
            loop = false;
            if(detachInsertionDSP){
                StartCoroutine(DetachInsertionDSP(atomSourceNum));
            }
        }
    }

    /**
     * <summary>指定したAtomSourceが使用しているボイスプールを破棄<summary>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceがある場合はここで番号を指定</param>
     */
    public void DisposeStandardVoicePool(int atomSourceNum = 0){
        StartCoroutine(DisposeStandardVoicePoolCore(atomSourceNum));
    }

    /**
     * <summary>CriAtomSourceの再生状態を取得<summary>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceがある場合はここで番号を指定</param>
     * <returns>CriAtomSourceの再生状態<returns>
     */
    public CriAtomSource.Status GetAtomSourceStatus(int atomSourceNum = 0){
        if(criAtomSourceList.Count <= atomSourceNum){
            criAtomSourceList.Add(InitializeAtomSource());
        }
        return criAtomSourceList[atomSourceNum].status;
    }

    /**
     * <summary>CriAtomSourceの再生状態を判定<summary>
     * <params name = "status">再生状態の名称がこの引数の値と同じかどうかを判定<params>
     * <return>再生状態と引数が一致するかどうか</return>
     */
    public bool JudgeAtomSourceStatus(string status, int atomSourceNum = 0){
        return GetAtomSourceStatus(atomSourceNum).ToString().Equals(status);
    }

    /**
     * <summary>指定したAISACコントロールを初期化<summary>
     * <params name = "aisacControlName">値を設定したいAISACコントロールの名前<params>
     */
    public void InitializeAisacControl(string aisacControlName){
        foreach(CriAtomSource source in criAtomSourceList){
            source.SetAisacControl(aisacControlName, 0f);
        }
    }
    
    //CriAtomSourceの追加
    private CriAtomSource InitializeAtomSource(){
        var atomSource = this.gameObject.AddComponent<CriAtomSource>();
        atomSource.use3dPositioning = true;
        atomSource.playOnStart = false;
        return atomSource;
    }

    //ADX_CueBank初期化時のCriAtomSource初期化処理
    private void Reset(){
        criAtomSourceList.Clear();
        criAtomSourceList.Add(GetComponent<CriAtomSource>());
        criAtomSourceList[0].cueSheet = "";
        criAtomSourceList[0].cueName = "";
        criAtomSourceList[0].playOnStart = false;
        cueManager = GameObject.FindObjectOfType<CueManager>().GetComponent<CueManager>();
    }

    private void Awake(){
        if(cueManager == null){
            cueManager = (CueManager)FindObjectOfType(typeof(CueManager));
        }
    }

    //PlayOnStartの実行
    private void Start(){
        if(playOnStart){
            //Play(PlayCueNameOnStart);
            criAtomSourceList[0].cueSheet = cueManager.GetCueSheetName(playCueNameOnStart).cueSheetName;
            criAtomSourceList[0].Play(playCueNameOnStart);
        }
    }
}
