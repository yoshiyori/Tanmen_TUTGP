using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExtarnalEditor : Editor{
    [MenuItem("GameObject/Sound Manager", false, 149)]
    public static void CreateSoundManager(){
        CueManager[] cueNameLists = FindObjectsOfType(typeof(CueManager)) as CueManager[];
        if(cueNameLists.Length > 0){
            Debug.LogError("SoundManager already exists.");

            //ヒエラルキー上でSoundManagerを選択
            Selection.activeGameObject = cueNameLists[0].gameObject;
        }
        else{
            GameObject soundManager = new GameObject("SoundManager");
            soundManager.AddComponent<CueManager>();
            soundManager.AddComponent<CuePlayer2D>();
            soundManager.AddComponent<CriWareInitializer>();
            soundManager.AddComponent<CriWareErrorHandler>();

            //ヒエラルキー上でSoundManagerを選択
            Selection.activeGameObject = soundManager;
        }
    }
}
