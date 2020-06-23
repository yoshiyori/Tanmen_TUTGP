using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour{
    [SerializeField] private CuePlayer cuePlayer;
    [SerializeField] private CriAtomSource source;
    //[SerializeField] private CueManager soundManager;
    //[SerializeField] private CriAtom atom;
    //[SerializeField] private GameObject a;

    private CriAtomExStandardVoicePool voicePool;

    private void Reset(){
        //soundManager = FindObjectOfType<>();
        //cueManager = FindObjectOfType<CueManager>();
        //atom = FindObjectOfType<CriAtom>();
    }

    private void Start(){
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.A)){
            voicePool = new CriAtomExStandardVoicePool(1, 2, 48000, false, 2);
            Debug.Log("Made voice pool");
            voicePool.AttachDspTimeStretch();
            source.player.SetVoicePoolIdentifier(2);
            source.player.SetDspTimeStretchRatio(2f);
            source.player.UpdateAll();
        }
        if(Input.GetKeyDown(KeyCode.D)){
            voicePool.Dispose();
            Debug.Log("Dispose voice pool");
        }
        if(Input.GetKeyDown(KeyCode.P)){
            cuePlayer.Play("SwingBoost");
        }
    }
}
