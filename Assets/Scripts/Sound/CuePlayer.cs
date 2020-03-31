using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoundSystem{
    //このスクリプトをゲームオブジェクトに適用すると、CriAtomSourceも適用される
    [RequireComponent(typeof(CriAtomSource))]
    public class CuePlayer : MonoBehaviour{
        //コンポーネント
        public List<CriAtomSource> criAtomSourceList = new List<CriAtomSource>();
        public CueNameList cueInfo;

        //キュー名
        public List<string> cueNameList = new List<string>();

        //パラメーター
        public bool playOnStart = false;
        public string playCueOnStart = "";

        //キューの再生
        //複数種類の音を鳴らすオブジェクトで、一部のみを止めたりといった処理をする際はCriAtomSourceが複数必要
        public void Play(string cueName, int atomSourceNum = 0){
            if(criAtomSourceList.Count <= atomSourceNum){
                criAtomSourceList.Add(InitializeAtomSource());
            }

            criAtomSourceList[atomSourceNum].cueSheet = cueInfo.GetCueNameInfo(cueName).cueSheetName;
            criAtomSourceList[atomSourceNum].Play(cueName);
        }

        //再生しているキューの停止
        public void Stop(int atomSourceNum = 0){
            if(criAtomSourceList.Count <= atomSourceNum){
                criAtomSourceList.Add(InitializeAtomSource());
            }
            criAtomSourceList[atomSourceNum].Stop();
        }

        //キューの再生状態の取得
        public CriAtomSource.Status GetAtomSourceStatus(int atomSourceNum = 0){
            if(criAtomSourceList.Count <= atomSourceNum){
                criAtomSourceList.Add(InitializeAtomSource());
            }
            return criAtomSourceList[atomSourceNum].status;
        }

        //public void Play(string cueName){}

        //CriAtomSourceの追加
        private CriAtomSource InitializeAtomSource(bool use3dPositioning = true, bool playOnStart = false){
            var atomSource = this.gameObject.AddComponent<CriAtomSource>();

            atomSource.use3dPositioning = use3dPositioning;
            atomSource.playOnStart = playOnStart;

            return atomSource;
        }

        //PlayOnStartの実行
        private void Start(){
            if(playOnStart){
                //Play(playCueOnStart);
                criAtomSourceList[0].cueSheet = cueInfo.GetCueNameInfo(playCueOnStart).cueSheetName;
                criAtomSourceList[0].Play(playCueOnStart);
            }
        }

        //ADX_CueBank初期化時のCriAtomSource初期化処理
        private void Reset(){
            criAtomSourceList.Clear();

            criAtomSourceList.Add(GetComponent<CriAtomSource>());
            criAtomSourceList[0].cueSheet = "";
            criAtomSourceList[0].cueName = "";
            criAtomSourceList[0].playOnStart = false;

            cueInfo = GameObject.FindObjectOfType<CueNameList>().GetComponent<CueNameList>();
        }
    }
}
