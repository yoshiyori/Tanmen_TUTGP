using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

/**
 * <summary>キューに関する情報の収集・管理・提供を行うコンポーネント</summary>
 */
[RequireComponent(typeof(CriAtom))]
public class CueManager : MonoBehaviour{
    public static CueManager singleton;

    //コンポーネント
    [SerializeField, NonEditable] private CriAtom criAtom;
    [SerializeField] private CuePlayer[] cuePlayers;
    [SerializeField] private CuePlayer2D cuePlayer2Ds;

    //データ
    [SerializeField] public List<ExCueInfo> exCueInfoList = new List<ExCueInfo>();
    //[SerializeField] private CriAtomAcfInfo.AcbInfo[] acbInfos;
    //private List<CriAtomExAcb> acbs;
    //private string[] acbNames;

#if UNITY_EDITOR
    //Editor以外からフィールドにアクセスすることを防ぐプロパティたち
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

    public CuePlayer[] CuePlayers{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return cuePlayers;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            cuePlayers = value;
        }
    }

    public CuePlayer2D CuePlayer2Ds{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return cuePlayer2Ds;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            cuePlayer2Ds = value;
        }
    }

    public List<ExCueInfo> ExCueInfoList{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return exCueInfoList;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            exCueInfoList = value;
        }
    }

    /*public CriAtomAcfInfo.AcbInfo[] AcbInfos{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return acbInfos;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            acbInfos = value;
        }
    }*/

    //Editor以外からアクセスできないメソッドたち
    //現在プロジェクト内に存在するキューの名前と、そのキューが属するキューシートの名前を取得し、リストに格納
    public void SetCueNameList(){
        //Editor以外からのアクセスをはじく
        var method = new System.Diagnostics.StackFrame(1).GetMethod();
        if(!method.DeclaringType.Name.Equals("CueManager")){
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
        }

        //古いデータを消去
        exCueInfoList.Clear();

        //プロジェクト内のacbファイルの名前を全て取得
        var acbNames = SearchFileNameByExtention(Application.streamingAssetsPath, false, "acb");

        //全てのacbファイルを順にCriAtomに読み込ませる
        foreach(var acbName in acbNames){
            CriAtom.AddCueSheet(acbName, acbName + ".acb", "");
            var cueSheet = CriAtom.GetAcb(acbName);

            //CriAtomExAcbに0から順にキューの情報を請求し、名称データを取得
            for(int i = 0; ; i++){
                var result = cueSheet.GetCueInfoByIndex(i, out var cueInfo);

                //存在しないインデックスに到達した場合ループを終了
                if(!result){
                    break;
                }
                exCueInfoList.Add(new ExCueInfo(acbName, cueInfo.name));
            }
            CriAtom.RemoveCueSheet(acbName);
        }
    }

    //現在プロジェクト内に存在するキューの名前と、そのキューが属するキューシートの名前を取得し、リストに格納
    /*public void SetCueNameList(bool checkCueInfos = false){
        //Editor以外からのアクセスをはじく
        var method = new System.Diagnostics.StackFrame(1).GetMethod();
        if(!method.DeclaringType.Name.Equals("CueManager")){
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
        }
        
        exCueInfoList.Clear();
        foreach(var acbInfo in acbInfos){
            foreach(var cueName in acbInfo.cueInfoList){
                exCueInfoList.Add(new ExCueInfo(acbInfo.name, cueName.name));
            }
        }
        if(checkCueInfos){
            CheckExCueInfoList();
        }
        Debug.Log("Set Cue Name List");
    }*/

    public void LoadTxt(){
        //Editor以外からのアクセスをはじく
        var method = new System.Diagnostics.StackFrame(1).GetMethod();
        if(!method.DeclaringType.Name.Equals("CueManager")){
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
        }

        //テキストファイルのロード
        string data;
        using(var reader = new StreamReader(Application.dataPath + "/Temporary/CueInfo.txt")){
            data = reader.ReadToEnd();
        }

        //データの登録
        exCueInfoList.Clear();
        var lines = data.Split('\n');
        foreach(var line in lines){
            if(!line.Equals("")){
                var names = line.Split('\t');
                exCueInfoList.Add(new ExCueInfo(names[0], names[1]));
            }
        }
    }

    //シーン内にあるCuePlayerが適用されたオブジェクトを取得する
    public void LoadCuePlayer(){
        //Editor以外からのアクセスをはじく
        var method = new System.Diagnostics.StackFrame(1).GetMethod();
        if(!method.DeclaringType.Name.Equals("CueManager")){
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
        }
        //古いデータの削除
        if((cuePlayers != null) && (cuePlayers.Length > 0)){
            Array.Clear(cuePlayers, 0, cuePlayers.Length);
            Array.Resize(ref cuePlayers, 0);
        }
        /*if((cuePlayer2Ds != null) && (cuePlayer2Ds.Length > 0)){
            Array.Clear(cuePlayer2Ds, 0, cuePlayer2Ds.Length);
            Array.Resize(ref cuePlayer2Ds, 0);
        }*/
        
        cuePlayers = FindObjectsOfType<CuePlayer>();
        cuePlayer2Ds = FindObjectOfType<CuePlayer2D>();
    }

    //CuePlayerから使用するキューの名前を取得し、それに併せてCriAtomにキューシートを登録する
    private void SetCueSheet(){
        //Editor以外からのアクセスをはじく
        var method = new System.Diagnostics.StackFrame(1).GetMethod();
        if(!method.DeclaringType.Name.Equals("CueManager")){
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
        }

        List<string> usedCueSheetList = new List<string>();

        if(criAtom == null){
            criAtom = GetComponent<CriAtom>();
            if(criAtom == null){
                Debug.LogWarning("Cri Atom not found.");
            }
        }
        Array.Clear(criAtom.cueSheets, 0, criAtom.cueSheets.Length);
        Array.Resize(ref criAtom.cueSheets, 0);

        //Array.Clear(cuePlayers, 0, cuePlayers.Length);
        //Array.Resize(ref cuePlayers, 0);
        //Array.Clear(cuePlayer2Ds, 0, cuePlayer2Ds.Length);
        //Array.Resize(ref cuePlayer2Ds, 0);

        foreach(var cuePlayer in cuePlayers){
            foreach(var cueName in cuePlayer.cueNameList){
                var usedCueSheet = GetCueSheetName(cueName).cueSheetName;
                if(!usedCueSheetList.Contains(usedCueSheet)){
                    usedCueSheetList.Add(usedCueSheet);
                }
            }
            if(!cuePlayer.playCueNameOnStart.Equals("")){
                var usedCueSheet = GetCueSheetName(cuePlayer.playCueNameOnStart).cueSheetName;
                if(!usedCueSheetList.Contains(usedCueSheet)){
                    usedCueSheetList.Add(usedCueSheet);
                }
            }
        }

        //foreach(var cuePlayer2D in cuePlayer2Ds){
            foreach(var cueName in cuePlayer2Ds.cueNameList){
                var usedCueSheet = GetCueSheetName(cueName).cueSheetName;
                if(!usedCueSheetList.Contains(usedCueSheet)){
                    usedCueSheetList.Add(usedCueSheet);
                }
            }
            if(!cuePlayer2Ds.playCueNameOnStart.Equals("")){
                var usedCueSheet = GetCueSheetName(cuePlayer2Ds.playCueNameOnStart).cueSheetName;
                if(!usedCueSheetList.Contains(usedCueSheet)){
                    usedCueSheetList.Add(usedCueSheet);
                }
            }
        //}

        foreach(var usedCueSheet in usedCueSheetList){
            if(!usedCueSheet.Equals("")){
                CriAtom.AddCueSheet(usedCueSheet, usedCueSheet + ".acb", "", null);
            }
        }

        for(int i = 0; i < cuePlayers.Length; i++){
            if(cuePlayers[i] == null){
                Array.Resize(ref cuePlayers, 0);
            }
        }
        /*for(int i = 0; i < cuePlayer2Ds.Length; i++){
            if(cuePlayer2Ds[i] == null){
                Array.Resize(ref cuePlayer2Ds, 0);
            }
        }*/

        EditorUtility.SetDirty(criAtom);
        Debug.Log("Set Cue Sheet");
    }

    //CuePlayerから使用するキューの名前を取得し、それに併せてCriAtomにキューシートを登録する
    public void SetCueSheetInternal(){
        //Editor以外からのアクセスをはじく
        var method = new System.Diagnostics.StackFrame(1).GetMethod();
        if(!method.DeclaringType.Name.Equals("CueManager")){
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
        }

        List<string> usedCueSheetList = new List<string>();

        if(criAtom == null){
            criAtom = GetComponent<CriAtom>();
            if(criAtom == null){
                Debug.LogWarning("Cri Atom not found.");
            }
        }
        Array.Clear(criAtom.cueSheets, 0, criAtom.cueSheets.Length);
        Array.Resize(ref criAtom.cueSheets, 0);

        foreach(var cuePlayer in cuePlayers){
            foreach(var cueName in cuePlayer.cueNameList){
                var usedCueSheet = GetCueSheetName(cueName).cueSheetName;
                if(!usedCueSheetList.Contains(usedCueSheet)){
                    usedCueSheetList.Add(usedCueSheet);
                }
            }
            if(!cuePlayer.playCueNameOnStart.Equals("")){
                var usedCueSheet = GetCueSheetName(cuePlayer.playCueNameOnStart).cueSheetName;
                if(!usedCueSheetList.Contains(usedCueSheet)){
                    usedCueSheetList.Add(usedCueSheet);
                }
            }
        }

        //foreach(var cuePlayer2D in cuePlayer2Ds){
            foreach(var cueName in cuePlayer2Ds.cueNameList){
                var usedCueSheet = GetCueSheetName(cueName).cueSheetName;
                if(!usedCueSheetList.Contains(usedCueSheet)){
                    usedCueSheetList.Add(usedCueSheet);
                }
            }
            if(!cuePlayer2Ds.playCueNameOnStart.Equals("")){
                var usedCueSheet = GetCueSheetName(cuePlayer2Ds.playCueNameOnStart).cueSheetName;
                if(!usedCueSheetList.Contains(usedCueSheet)){
                    usedCueSheetList.Add(usedCueSheet);
                }
            }
        //}

        foreach(var usedCueSheet in usedCueSheetList){
            if(!usedCueSheet.Equals("")){
                criAtom.AddCueSheetInternal(usedCueSheet, usedCueSheet + ".acb", "", null);
            }
        }
        EditorUtility.SetDirty(criAtom);
        Debug.Log("Set Cue Sheet");
    }

    //CuePlayerから使用するキューの名前を取得し、それに併せてCriAtomにキューシートを登録する
    private void UnloadCueSheet(){
        List<string> usedCueSheetList = new List<string>();

        if(criAtom == null){
            criAtom = GetComponent<CriAtom>();
            if(criAtom == null){
                Debug.LogWarning("Cri Atom not found.");
            }
        }
        Array.Clear(criAtom.cueSheets, 0, criAtom.cueSheets.Length);
        Array.Resize(ref criAtom.cueSheets, 0);

        foreach(var cuePlayer in cuePlayers){
            foreach(var cueName in cuePlayer.cueNameList){
                var usedCueSheet = GetCueSheetName(cueName).cueSheetName;
                if(!usedCueSheetList.Contains(usedCueSheet)){
                    usedCueSheetList.Add(usedCueSheet);
                }
            }
            if(!cuePlayer.playCueNameOnStart.Equals("")){
                var usedCueSheet = GetCueSheetName(cuePlayer.playCueNameOnStart).cueSheetName;
                if(!usedCueSheetList.Contains(usedCueSheet)){
                    usedCueSheetList.Add(usedCueSheet);
                }
            }
        }

        //foreach(var cuePlayer2D in cuePlayer2Ds){
            foreach(var cueName in cuePlayer2Ds.cueNameList){
                var usedCueSheet = GetCueSheetName(cueName).cueSheetName;
                if(!usedCueSheetList.Contains(usedCueSheet)){
                    usedCueSheetList.Add(usedCueSheet);
                }
            }
            if(!cuePlayer2Ds.playCueNameOnStart.Equals("")){
                var usedCueSheet = GetCueSheetName(cuePlayer2Ds.playCueNameOnStart).cueSheetName;
                if(!usedCueSheetList.Contains(usedCueSheet)){
                    usedCueSheetList.Add(usedCueSheet);
                }
            }
        //}

        foreach(var usedCueSheet in usedCueSheetList){
            if(!usedCueSheet.Equals("")){
                CriAtom.RemoveCueSheet(usedCueSheet);
            }
        }
        EditorUtility.SetDirty(criAtom);
        Debug.Log("Set Cue Sheet");
    }
#endif

    //各キューのゲーム変数名を取得
    private void SetGameVariableName(){
        int j = 0;
        if(criAtom.cueSheets.Length > 0){
            for(int i = 0; i < exCueInfoList.Count; i++){
                if(!exCueInfoList[i].cueSheetName.Equals(criAtom.cueSheets[j].name)){
                    j++;
                }

                if(j >= criAtom.cueSheets.Length){
                    break;
                }
                
                if(criAtom.cueSheets[j].acb != null){
                    if(criAtom.cueSheets[j].acb.GetCueInfo(exCueInfoList[i].cueName, out var cueInfo)){
                        exCueInfoList[i].gameVariableName = cueInfo.gameVariableInfo.name;
                    }
                }
            }
        }
    }

    /**
     * <summary>指定したキューのExCueInfoを取得</summary>
     * <param name = "cueName">情報を取得したいキューの名前</param>
     * <returns> 指定したキューのExCueInfo </returns>
     */
    public ExCueInfo GetCueSheetName(string cueName){
        var index = exCueInfoList.FindIndex(exCueInfo => exCueInfo.cueName.Equals(cueName));
        if(index == -1){
            Debug.LogWarning("Cue \"" + cueName + "\" Not Found");
            return new ExCueInfo("", "");
        }
        else{
            return exCueInfoList[index];
        }
    }

    /**
     * <summary>現在のExCueInfoListの中身をログに表示</summary>
     */
    public void CheckExCueInfoList(){
        bool noData = true;
        foreach(var cueNameInfo in exCueInfoList){
            cueNameInfo.CheckExCueInfo();
            noData = false;
        }
        if(noData){
            Debug.Log("No Data");
        }
    }

    /**
     * <summary>指定したキューシートに属するキューの再生を全て一時停止する</summary>
     * <param name = "cueSheetName">再生を一時停止したいキューシート</param>
     */
    public void PauseCueSheet(string cueSheetName){
        var matchDatas = exCueInfoList.Where(exCueInfo => exCueInfo.cueSheetName.Equals(cueSheetName));
        if(matchDatas == null){
            Debug.LogWarning(cueSheetName + " not found.");
            return;
        }

        foreach(var cuePlayer in cuePlayers){
            foreach(var matchData in matchDatas){
                if(cuePlayer.cueNameList.Exists(cueName => cueName.Equals(matchData.cueName)) || cuePlayer.playCueNameOnStart.Equals(matchData.cueName)){
                    for(int i = 0; i < cuePlayer.criAtomSourceNum; i++){
                        cuePlayer.Pause(i);
                    }
                }
            }
        }
        //foreach(var cuePlayer2D in cuePlayer2Ds){
            foreach(var matchData in matchDatas){
                if(cuePlayer2Ds.cueNameList.Exists(cueName => cueName.Equals(matchData.cueName)) || cuePlayer2Ds.playCueNameOnStart.Equals(matchData.cueName)){
                    /*for(int i = 0; i < cuePlayer2D.criAtomSourceNum; i++){
                        cuePlayer2D.Pause(i);
                    }*/
                    cuePlayer2Ds.Pause(0);
                }
            }
        //}
    }

    /**
     * <summary>指定したキューシートに属するキューの再生を全て再開する</summary>
     * <param name = "cueSheetName">再生を再開したいキューシート</param>
     */

    public void RestartCueSheet(string cueSheetName){
        var matchDatas = exCueInfoList.Where(exCueInfo => exCueInfo.cueSheetName.Equals(cueSheetName));
        if(matchDatas == null){
            Debug.LogWarning(cueSheetName + " not found.");
            return;
        }

        foreach(var cuePlayer in cuePlayers){
            foreach(var matchData in matchDatas){
                if(cuePlayer.cueNameList.Exists(cueName => cueName.Equals(matchData.cueName)) || cuePlayer.playCueNameOnStart.Equals(matchData.cueName)){
                    for(int i = 0; i < cuePlayer.criAtomSourceNum; i++){
                        cuePlayer.Restart(i);
                    }
                }
            }
        }
        //foreach(var cuePlayer2D in cuePlayer2Ds){
            foreach(var matchData in matchDatas){
                if(cuePlayer2Ds.cueNameList.Exists(cueName => cueName.Equals(matchData.cueName)) || cuePlayer2Ds.playCueNameOnStart.Equals(matchData.cueName)){
                    for(int i = 0; i < cuePlayer2Ds.criAtomSourceNum; i++){
                        cuePlayer2Ds.Restart(i);
                    }
                }
            }
        //}
    }
    
    /**
     * <summary>指定のフォルダから目当ての拡張子のファイルを探す</summary>
     * <param name = "searchDirectoryPath">検索するディレクトリーのフルパス</param>
     * <param name = "getExtention">出力結果に拡張子を含む</param>
     * <param name = "extentions">検索したい拡張子(「.」や「*」は含んでいても、含んでいなくても良い)
     * <returns>指定した拡張子を持つファイルのファイル名(List)</returns>
     */
    public List<string> SearchFileNameByExtention(string searchDirectoryPath, bool getExtention = true, params string[] extentions){
        List<string> fileNames = new List<string>();
        foreach(var ext in extentions){
            //拡張を検索するための文字列に「*」「.」がない場合は追加
            var extention = ext;
            if(!extention.Contains(".")){
                extention = "." + extention;
            }
            if(!extention.Contains("*")){
                extention = "*" + extention;
            }
            var filePathes = Directory.GetFiles(searchDirectoryPath, extention);
            if(filePathes != null){
                foreach(var filePath in filePathes){
                    if(getExtention){
                        fileNames.Add(Path.GetFileName(filePath));
                    }
                    else{
                        fileNames.Add(Path.GetFileNameWithoutExtension(filePath));
                    }
                }
            }
        }
        if((fileNames == null) || (fileNames.Count <= 0)){
            Debug.Log("File not found.");
        }
        return fileNames;
    }

    //CueManager初期化時のCriAtom初期化処理
    private void Reset(){
        criAtom = GetComponent<CriAtom>();
        //acfファイルを検索して、そのファイル名をCriAtomに渡す
        criAtom.acfFile = SearchFileNameByExtention(Application.streamingAssetsPath, true, "acf")[0];
        criAtom.dontDestroyOnLoad = true;
    }

    void Awake(){
        Debug.Log("Awake");

        //前シーンからサウンドマネージャーが引き継がれた場合自身を削除
        if(singleton == null){
            singleton = this;
        }
        else{
            Destroy(this.gameObject);
        }

        cuePlayer2Ds = FindObjectOfType<CuePlayer2D>();
        this.SetGameVariableName();
    }

    void Start(){
        //SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnActiveSceneChanged( Scene prevScene, Scene nextScene ){
        Debug.Log("New Scene Active");
    }

    void OnSceneLoaded( Scene scene, LoadSceneMode mode ){
        Debug.Log ( scene.name + " scene loaded");
        this.UnloadCueSheet();
        this.LoadCuePlayer();
        this.SetCueSheet();
        this.SetGameVariableName();
    }
}

//キューの名前と、そのキューが属するキューシートの名前を保存
/**
 * <summary>キューの名前とそのキューに関する情報をまとめて保存するクラス
 * cueSheetName : キューが属するキューシートの名前
 * cueName : キューの名前
 * gameVariableName : キューが参照するゲーム変数の名前</summary>
 */
[Serializable] public class ExCueInfo{
    [SerializeField] internal string cueSheetName;
    [SerializeField] internal string cueName;
    [SerializeField] internal string gameVariableName;

#if UNITY_EDITOR
    //Editor以外からフィールドにアクセスすることを防ぐプロパティたち
    public string CueSheetName{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return cueSheetName;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            cueSheetName = value;
        }
    }
    public string CueName{
        get{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            return cueName;
        }
        set{
            var method = new System.Diagnostics.StackFrame(1).GetMethod();
            Assert.IsTrue(method.DeclaringType.Assembly.IsDefined(typeof(AssemblyIsEditorAssembly), false), "Invalid acssess from " + method.DeclaringType + "::" + method.Name);
            cueName = value;
        }
    }
#endif

    public ExCueInfo(string cueSheet, string cue, string gameVariable= ""){
        cueSheetName = cueSheet;
        cueName = cue;
        gameVariableName = gameVariable;
    }

    /*public ExCueInfo(in CriAtom criAtom, int cueSheetID, int cueID){
        cueSheetName = criAtom.cueSheets[cueSheetID].name;
        cueName = criAtom.cueSheets[cueSheetID].acb.GetCueInfoList()[cueID].name;
    }*/

    /**
     * <summary>ExCueInfoのフィールドをコンソールに出力</summary>
     */
    public void CheckExCueInfo(){
        Debug.Log("CueSheet:" + cueSheetName + " Cue:" + cueName);
    }
}
