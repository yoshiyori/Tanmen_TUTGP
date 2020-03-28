using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace SoundSystem{
    [RequireComponent(typeof(CriAtom))]
    public class CueNameList : MonoBehaviour{
        //コンポーネント
        [SerializeField, NonEditable] private CriAtom criAtom;
        [SerializeField] private CuePlayer[] audioSourceObject;

        //データ
        [HideInInspector] public List<CueNameInfo> cueNameInfos = new List<CueNameInfo>();
        //[SerializeField, HideInInspector] private List<string> cueNames = new List<string>();
        [HideInInspector] public string acfName;
        [HideInInspector] public CriAtomAcfInfo.AcbInfo[] acbInfos;

        //定数
        private const string dataFilePath = "Assets/Editor/10yen/CueNameInfo.txt";

        //プロパティ
        /*public List<string> CueNames{
            get{
                return cueNames;
            }
        }*/

        //CueNameList初期化時のCriAtom初期化処理
        public void Reset(){
            criAtom = GetComponent<CriAtom>();
            criAtom.acfFile = SearchExtention(Application.streamingAssetsPath, true, "acf")[0];
        }

        //CriAtomにacfファイルの名前を登録
        public void SetAcf(){
            criAtom.acfFile = acfName + ".acf";
        }

        //CriAtomAcfInfoクラスからキューシートとキューの名前を取得
        public void SetCueNameList(bool checkCueNames = false){
            cueNameInfos.Clear();
            //cueNames.Clear();

            foreach(var acbInfo in acbInfos){
                foreach(var cueInfo in acbInfo.cueInfoList){
                    cueNameInfos.Add(new CueNameInfo(acbInfo.name, cueInfo.name));
                    //cueNames.Add(cueInfo.name);
                }
            }

            if(checkCueNames){
                CheckCueNameInfos();
            }
            Debug.Log("Set Cue Name List");
        }

        //指定のキューのCueNameInfoを返す
        public CueNameInfo GetCueNameInfo(string cueName){
            var index = cueNameInfos.FindIndex(cueNameInfo => cueNameInfo.cueName.Equals(cueName));

            if(index == -1){
                Debug.LogWarning("Cue \"" + cueName + "\" Not Found");
                return new CueNameInfo("", "");
            }
            else{
                return cueNameInfos[index];
            }
        }

        //CuePlayerから使用するキューの名前を取得し、それに併せてCriAtomにキューシートを登録する
        public void SetCueSheet(){
            List<string> usedCueSheetList = new List<string>();

            Array.Clear(criAtom.cueSheets, 0, criAtom.cueSheets.Length);
            Array.Resize(ref criAtom.cueSheets, 0);

            //LoadCuePlayer();
            foreach(var cuePlayer in audioSourceObject){
                foreach(var cueName in cuePlayer.cueNameList){
                    var usedCueSheet = GetCueNameInfo(cueName).cueSheetName;
                    if(!usedCueSheetList.Contains(usedCueSheet)){
                        usedCueSheetList.Add(usedCueSheet);
                    }
                }
                if(!cuePlayer.playCueOnStart.Equals("")){
                    var usedCueSheet = GetCueNameInfo(cuePlayer.playCueOnStart).cueSheetName;
                    if(!usedCueSheetList.Contains(usedCueSheet)){
                        usedCueSheetList.Add(usedCueSheet);
                    }
                }
            }

            foreach(var usedCueSheet in usedCueSheetList){
                //CriAtom.AddCueSheet(usedCueSheet, usedCueSheet + ".acb", "");
                criAtom.AddCueSheetInternal(usedCueSheet, usedCueSheet + ".acb", "", null);
            }

            EditorUtility.SetDirty(criAtom);
            Debug.Log("Set Cue Sheet");
        }

        //シーン内にあるCuePlayerが適用されたオブジェクトを取得する
        public void LoadCuePlayer(){
            //古いデータの削除
            Array.Clear(audioSourceObject, 0, audioSourceObject.Length);
            Array.Resize(ref audioSourceObject, 0);
            
            audioSourceObject = FindObjectsOfType<CuePlayer>();
        }

        //現在のCueNameInfoをログに表示
        public void CheckCueNameInfos(){
            bool noData = true;

            foreach(var cueNameInfo in cueNameInfos){
                cueNameInfo.CheckCueNameInfo();
                noData = false;
            }

            if(noData){
                Debug.Log("No Data");
            }
        }

        //Jsonファイルからacfファイルの情報を読み出す
        //CriAtomWindow.csの丸パクリ
        /*private CriAtomAcfInfo.AcfInfo LoadAcfJson(){
		    string tmp_json;

		    if(File.Exists(acfJsonPath)) {
	    		tmp_json = File.ReadAllText(acfJsonPath);
    			return JsonUtility.FromJson<CriAtomAcfInfo.AcfInfo>(tmp_json);
		    }
            else{
		    	tmp_json = null;
                return null;
    		}
        }*/

        /*private void WriteCueNameInfoFile(){
            string contents = "";

            for(int i = 0; i < cueNameInfos.Count; i++){
                contents += i.ToString() + "\t" + cueNameInfos[i].cueSheetName + "\t" + cueNameInfos[i].cueName + "\n";
            }
            File.WriteAllText(dataFilePath, contents);
            Debug.Log("Edit CueNameInfo.txt");
            //Debug.Log(contents);
        }

        private void LoadCueNameInfoFile(){
            if(File.Exists(dataFilePath)){
                char[] spliter = {'\n', '\t'};
                string[] contents = File.ReadAllText(dataFilePath).Split(spliter);

                Debug.Log("Read CueNameInfo.txt");
                cueNameInfos.Clear();
                for(int i = 0; i < contents.Length - 2; i += 3){
                    cueNameInfos.Add(new CueNameInfo(contents[i + 1], contents[i+2]));
                }
            }
            else{
                Debug.Log(dataFilePath + " Not Found");
            }
        }*/
        
        //指定のフォルダから目当ての拡張子のファイルを探す
        //第二引数の拡張子に「.」や「*.」は書いても書かなくても良い
        //拡張子は複数指定できる
        //getExtentionは出力結果に拡張子を含むかどうかのフラグ
        public List<string> SearchExtention(string searchDirectoryPath, bool getExtention = true, params string[] extentions){
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
    }

    //キューの名前と、そのキューが属するキューシートの名前を保存
    [Serializable] public class CueNameInfo{
        public string cueSheetName;
        public string cueName;

        public CueNameInfo(string cueSheet, string cue){
            cueSheetName = cueSheet;
            cueName = cue;
        }

        public CueNameInfo(in CriAtom criAtom, int cueSheetID, int cueID){
            cueSheetName = criAtom.cueSheets[cueSheetID].name;
            cueName = criAtom.cueSheets[cueSheetID].acb.GetCueInfoList()[cueID].name;
        }

        public void CheckCueNameInfo(){
            Debug.Log("CueSheet:" + cueSheetName + " Cue:" + cueName);
        }
    }
}
