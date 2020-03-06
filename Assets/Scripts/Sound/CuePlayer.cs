using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoundSystem{
    //このスクリプトをゲームオブジェクトに適用すると、CriAtomSourceも適用される
    [RequireComponent(typeof(CriAtomSource))]
    public class CuePlayer : MonoBehaviour{
        //コンポーネント
        public CriAtomSource criAtomSource;
        public CueNameList cueInfo;

        //キュー名
        public List<string> cueNameList = new List<string>();

        public void Play(string cueName){
            criAtomSource.cueSheet = cueInfo.GetCueNameInfo(cueName).CueSheetName;
            criAtomSource.Play(cueName);
        }

        //ADX_CueBank初期化時のCriAtomSource初期化処理
        public void Reset(){
            criAtomSource = GetComponent<CriAtomSource>();
            criAtomSource.playOnStart = false;

            cueInfo = GameObject.FindObjectOfType<CueNameList>().GetComponent<CueNameList>();
        }
    }
}
