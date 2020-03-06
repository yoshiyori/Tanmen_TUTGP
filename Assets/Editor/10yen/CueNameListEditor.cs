using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SoundSystem;

[CustomEditor(typeof(CueNameList))]
public class CueNameListEditor : Editor{
    private CueNameList cueNameList = null;
    private void OnEnable(){
        cueNameList = (CueNameList)base.target;
    }

    public override void OnInspectorGUI(){
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
            if(GUILayout.Button("Get Audio Source Object")){
                cueNameList.LoadCuePlayer();
            }
            if(GUILayout.Button("Set Cue Sheet")){
                cueNameList.SetCueSheet();
            }
            if(GUILayout.Button("Check Cue Name List")){
                cueNameList.CheckCueNameInfos();
            }
        GUILayout.EndHorizontal();
    }
}
