using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

namespace SoundSystem{
    [RequireComponent(typeof(CriAtom))]
    public class CueNameList : MonoBehaviour{
        //コンポーネント
        [SerializeField, NonEditable] private CriAtom criAtom;
        [SerializeField] private CuePlayer[] audioSourceObject;

        //データ
        public static List<CueNameInfo> cueNameInfos = new List<CueNameInfo>();
        private CriAtomAcfInfo.AcfInfo acfInfo;

        //定数
        private const string acfJsonPath = "Assets/Editor/CriWare/CriAtom/saveAcfData.json";

        void Awake(){
            Reset();
        }

        //CueNameList初期化時のCriAtom初期化処理
        public void Reset(){
            criAtom = GetComponent<CriAtom>();
            acfInfo = LoadAcfJson();

            criAtom.acfFile = Path.GetFileName(acfInfo.acfFilePath);
            criAtom.dontDestroyOnLoad = false;
            //criAtom.dontRemoveExistsCueSheet = true;
            SetCueNameList(false);
        }

        //CriAtomクラスからキューの名前と、そのキューが属するキューシートの名前を取得
        public void SetCueNameList(bool checkCueNames = false){
            //古いCueNameListの破棄
            cueNameInfos.Clear();

            //キューシート情報の読みだし
            var acbInfos = acfInfo.GetAcbInfoList(false, Application.streamingAssetsPath);

            //CriAtomにキューシート情報を送り、そこからキュー情報を取得
            for(int i = 0; i<acbInfos.Length; i++){
                var acbInfo = acbInfos[i];
                criAtom.AddCueSheetInternal(acbInfo.name, acbInfo.name + ".acb", "", null);
                criAtom.Setup();
                for(int j = 0; j < criAtom.cueSheets[0].acb.GetCueInfoList().Length; j++){
                    cueNameInfos.Add(new CueNameInfo(in criAtom, 0, j));
                }
                //キュー情報を読みだしたらCriAtom上のキューシート情報を破棄
                criAtom.RemoveCueSheetInternal(acbInfo.name);
            }
            
            if(checkCueNames){
                CheckCueNameInfos();
            }
            else{
                Debug.Log("Set Cue Name List");
            }
        }

        //指定のキューのCueNameInfoを返す
        public CueNameInfo GetCueNameInfo(string cueName){
            var index = cueNameInfos.FindIndex(cueNameInfo => cueNameInfo.CueName.Equals(cueName));

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

            LoadCuePlayer();
            foreach(var cuePlayer in audioSourceObject){
                foreach(var cueName in cuePlayer.cueNameList){
                    var usedCueSheet = GetCueNameInfo(cueName).CueSheetName;
                    if(!usedCueSheetList.Contains(usedCueSheet)){
                        usedCueSheetList.Add(usedCueSheet);
                    }
                }
            }

            foreach(var usedCueSheet in usedCueSheetList){
                //CriAtom.AddCueSheet(usedCueSheet, usedCueSheet + ".acb", "");
                criAtom.AddCueSheetInternal(usedCueSheet, usedCueSheet + ".acb", "", null);
            }
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
        private CriAtomAcfInfo.AcfInfo LoadAcfJson(){
		    string tmp_json;

		    if(File.Exists(acfJsonPath)) {
	    		tmp_json = File.ReadAllText(acfJsonPath);
    			return JsonUtility.FromJson<CriAtomAcfInfo.AcfInfo>(tmp_json);
		    }
            else{
		    	tmp_json = null;
                return null;
    		}
        }
        
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
            return fileNames;
        }
    }

    //キューの名前と、そのキューが属するキューシートの名前を保存
    public class CueNameInfo{
        private string cueSheetName;
        private string cueName;

        public string CueSheetName{
            get{
                return cueSheetName;
            }
        }
        public string CueName{
            get{
                return cueName;
            }
        }

        public CueNameInfo(string cueSheet, string cue){
            cueSheetName = cueSheet;
            cueName = cue;
        }

        public CueNameInfo(in CriAtom criAtom, int cueSheetID, int cueID){
            cueSheetName = criAtom.cueSheets[cueSheetID].name;
            cueName = criAtom.cueSheets[cueSheetID].acb.GetCueInfoList()[cueID].name;
        }

        public void CheckCueNameInfo(){
            Debug.Log("CueSheet:" + CueSheetName + " Cue:" + CueName);
        }
    }
}
