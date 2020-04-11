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
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Collider objectCollider;

    //キュー名
    [SerializeField] internal List<string> cueNameList = new List<string>();

    //パラメーター
    [SerializeField] private bool playOnStart = false;
    [SerializeField] internal string playCueNameOnStart = "";
    

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
    
    public Collider ObjectCollider{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return objectCollider;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            objectCollider = value;
        }
    }
    
    public MeshFilter MeshFilter{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return meshFilter;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            meshFilter = value;
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
    private IEnumerator destroyAfterPlay;
    private IEnumerator DestroyAfterPlay(GameObject gameObject, int atomSourceNum = 0){
        while(!JudgeAtomSourceStatus("PlayEnd", atomSourceNum)){
            yield return null;
        }
        Destroy(gameObject);
    }

    /**
     * <summary>指定した名前のキューを再生</summary>
     * <param name = "cueName">再生したいキューの名前</param>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceが複数必要な場合はここで番号を指定する
     * (キューごとに異なるタイミングで再生を止めたりなどの操作をする場合はCriAtomSourceが複数必要になる。追加のCriAtomSourceは自動で適用される。)</param>
     * <param name = "gameVariable">ゲーム変数による変化を設定してる場合はここで値を指定</param>
     */
    public void Play(string cueName, int atomSourceNum = 0, float gameVariable = 0f){
        if(criAtomSourceList.Count <= atomSourceNum){
            criAtomSourceList.Add(InitializeAtomSource());
        }
        var cue = cueManager.GetCueSheetName(cueName);
        if(!cue.gameVariableName.Equals("")){
            CriAtomEx.SetGameVariable(cue.gameVariableName, gameVariable);
        }
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
     */
    public void PlayAndDestroy(string cueName, int atomSourceNum = 0, float gameVariable = 0f){
        Play(cueName, atomSourceNum, gameVariable);
        if(meshFilter != null){
            Destroy(meshFilter);
        }
        if(objectCollider != null){
            Destroy(objectCollider);
        }
        destroyAfterPlay = DestroyAfterPlay(this.gameObject);
        StartCoroutine(destroyAfterPlay);
    }

    /**
     * <summary>CriAtomSourceの再生を一時停止する</summary>
     * <param name = "atomSourceNum">1つのオブジェクトにCriAtomSourceがある場合はここで番号を指定</param>
     */
    public void Pause(int atomSourceNum = 0){
        if(criAtomSourceList.Count <= atomSourceNum){
            criAtomSourceList.Add(InitializeAtomSource());
        }
        criAtomSourceList[atomSourceNum].Pause(true);
    }

    /**
     * <summary>CriAtomSourceの再生を再開する</summary>
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
     * <params status = "status">再生状態の名称がこの引数の値と同じかどうかを判定<params>
     * <return>再生状態と引数が一致するかどうか</return>
     */
    public bool JudgeAtomSourceStatus(string status, int atomSourceNum = 0){
        return GetAtomSourceStatus(atomSourceNum).ToString().Equals(status);
    }

    //CriAtomSourceの追加
    private CriAtomSource InitializeAtomSource(bool use3dPositioning = true, bool playOnStart = false){
        var atomSource = this.gameObject.AddComponent<CriAtomSource>();
        atomSource.use3dPositioning = use3dPositioning;
        atomSource.playOnStart = playOnStart;
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

        meshFilter = GetComponent<MeshFilter>();
        objectCollider = GetComponent<Collider>();
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
