using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SoundSystem;

public class ExtarnalEditor : Editor{
    [MenuItem("GameObject/Sound Manager", false, 149)]
    public static void CreateSoundManager(){
        CueNameList[] cueNameLists = FindObjectsOfType(typeof(CueNameList)) as CueNameList[];
        if(cueNameLists.Length > 0){
            Debug.LogError("SoundManager already exists.");

            //ヒエラルキー上でSoundManagerを選択
            Selection.activeGameObject = cueNameLists[0].gameObject;
        }
        else{
            GameObject soundManager = new GameObject("SoundManager");
            soundManager.AddComponent<CueNameList>();
            soundManager.AddComponent<CriWareInitializer>();
            soundManager.AddComponent<CriWareErrorHandler>();

            //ヒエラルキー上でSoundManagerを選択
            Selection.activeGameObject = soundManager;
        }

        //CriWareEditor.CreateCriwareLibraryInitalizer();
        //CriWareEditor.CreateCriwareErrorHandler();
    }
}
