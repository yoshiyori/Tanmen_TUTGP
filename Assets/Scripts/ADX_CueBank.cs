using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoundSystem{
    //このスクリプトをゲームオブジェクトに適用すると、CriAtomSourceも適用される
    [RequireComponent(typeof(CriAtomSource))]
    public class ADX_CueBank : MonoBehaviour{
        //コンポーネント
        public CriAtomSource criAtomSource;
        //キューシート名
        public string cueSheetName;
        public List<string> cueNameList = new List<string>();

        public void play(string cueName){
            if(cueNameList.FirstOrDefault(cue => cue == cueName) == null){
                Debug.Log(cueName + " Not Found");
            }
            else{
                criAtomSource.cueSheet = cueSheetName;
                criAtomSource.cueName = cueName;
                criAtomSource.Play();
            }
        }

        //ADX_CueBank初期化時のCriAtomSource初期化処理
        public void Reset(){
            criAtomSource = GetComponent<CriAtomSource>();
            criAtomSource.playOnStart = false;
        }
    }
}
