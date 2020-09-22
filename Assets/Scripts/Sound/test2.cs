using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test2 : MonoBehaviour{
    //[SerializeField] private CuePlayer2D cuePlayer;
    private CriAtomExPlayer player;

    private void Reset(){
    }

    private void Start(){
        player = new CriAtomExPlayer();
        var cue = CueManager.singleton.GetCueSheetName("SelectBGM");
        player.SetCue(CriAtom.GetAcb(cue.cueSheetName), cue.cueName);
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.S)){
            player.Start();
        }
        if(Input.GetKeyDown(KeyCode.A)){
            //player.SetAisacControl()
        }
    }
}
