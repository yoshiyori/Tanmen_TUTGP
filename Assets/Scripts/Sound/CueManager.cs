using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

/**
 * <summary>キューに関する情報の収集・管理・提供を行うコンポーネント</summary>
 */
[RequireComponent(typeof(CriAtom))]
public class CueManager : MonoBehaviour{

    //コンポーネント
    [SerializeField, NonEditable] private CriAtom criAtom;
    [SerializeField] private CuePlayer[] cuePlayers;
    [SerializeField] private CuePlayer2D[] cuePlayer2Ds;

    //データ
    [SerializeField] private List<ExCueInfo> exCueInfoList = new List<ExCueInfo>();
    [SerializeField] private CriAtomAcfInfo.AcbInfo[] acbInfos;

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

    public CuePlayer2D[] CuePlayer2Ds{
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

    public CriAtomAcfInfo.AcbInfo[] AcbInfos{
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
    }

    //Editor以外からアクセスできないメソッドたち
    //現在プロジェクト内に存在するキューの名前と、そのキューが属するキューシートの名前を取得し、リストに格納
    public void SetCueNameList(bool checkCueInfos = false){
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
        if((cuePlayer2Ds != null) && (cuePlayer2Ds.Length > 0)){
            Array.Clear(cuePlayer2Ds, 0, cuePlayer2Ds.Length);
            Array.Resize(ref cuePlayer2Ds, 0);
        }
        
        cuePlayers = FindObjectsOfType<CuePlayer>();
        cuePlayer2Ds = FindObjectsOfType<CuePlayer2D>();
    }

    //CuePlayerから使用するキューの名前を取得し、それに併せてCriAtomにキューシートを登録する
    public void SetCueSheet(){
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

        foreach(var cuePlayer2D in cuePlayer2Ds){
            foreach(var cueName in cuePlayer2D.cueNameList){
                var usedCueSheet = GetCueSheetName(cueName).cueSheetName;
                if(!usedCueSheetList.Contains(usedCueSheet)){
                    usedCueSheetList.Add(usedCueSheet);
                }
            }
            if(!cuePlayer2D.playCueNameOnStart.Equals("")){
                var usedCueSheet = GetCueSheetName(cuePlayer2D.playCueNameOnStart).cueSheetName;
                if(!usedCueSheetList.Contains(usedCueSheet)){
                    usedCueSheetList.Add(usedCueSheet);
                }
            }
        }

        foreach(var usedCueSheet in usedCueSheetList){
            criAtom.AddCueSheetInternal(usedCueSheet, usedCueSheet + ".acb", "", null);
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
                
                if(criAtom.cueSheets[j].acb.GetCueInfo(exCueInfoList[i].cueName, out var cueInfo)){
                    exCueInfoList[i].gameVariableName = cueInfo.gameVariableInfo.name;
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
        foreach(var cuePlayer2D in cuePlayers){
            foreach(var matchData in matchDatas){
                if(cuePlayer2D.cueNameList.Exists(cueName => cueName.Equals(matchData.cueName)) || cuePlayer2D.playCueNameOnStart.Equals(matchData.cueName)){
                    for(int i = 0; i < cuePlayer2D.criAtomSourceNum; i++){
                        cuePlayer2D.Pause(i);
                    }
                }
            }
        }
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
        foreach(var cuePlayer2D in cuePlayers){
            foreach(var matchData in matchDatas){
                if(cuePlayer2D.cueNameList.Exists(cueName => cueName.Equals(matchData.cueName)) || cuePlayer2D.playCueNameOnStart.Equals(matchData.cueName)){
                    for(int i = 0; i < cuePlayer2D.criAtomSourceNum; i++){
                        cuePlayer2D.Restart(i);
                    }
                }
            }
        }
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
    }

    private void Awake(){
        SetGameVariableName();
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
