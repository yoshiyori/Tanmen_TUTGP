using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using SoundSystem;

[CustomEditor(typeof(CueNameList))]
public class CueNameListEditor : Editor{
    private CueNameList cueNameList = null;
    //private CriAtom criAtom = null;

    //パラメーター
    private bool isOpen = true;

    private void OnEnable(){
        cueNameList = (CueNameList)base.target;
        //criAtom = (CriAtom)FindObjectOfType(typeof(CriAtom));
    }

    public override void OnInspectorGUI(){
        Undo.RecordObject(target, null);
        GUI.changed = false;

        base.OnInspectorGUI();

        isOpen = EditorGUILayout.Foldout(isOpen, "Cue Name List");
        GUI.enabled = false;
            if(isOpen){
                EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("No", GUILayout.MaxWidth(40));
                        EditorGUILayout.LabelField("Cue Sheet");
                        EditorGUILayout.LabelField("Cue");
                    EditorGUILayout.EndHorizontal();
                    if((cueNameList.cueNameInfos != null) && (cueNameList.cueNameInfos.Count > 0)){
                        EditorGUILayoutEx.Separator(true);
                        for(int i = 0; i < cueNameList.cueNameInfos.Count; i++){
                            EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField(i.ToString(), GUILayout.MaxWidth(40));
                                cueNameList.cueNameInfos[i].cueSheetName = EditorGUILayout.TextField(cueNameList.cueNameInfos[i].cueSheetName);
                                cueNameList.cueNameInfos[i].cueName = EditorGUILayout.TextField(cueNameList.cueNameInfos[i].cueName);
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                EditorGUI.indentLevel--;
            }
        GUI.enabled = true;

        GUILayout.BeginHorizontal();
            if(GUILayout.Button("Get Audio Source Object")){
                cueNameList.LoadCuePlayer();
            }
            if(GUILayout.Button("Set Cue Sheet")){
                cueNameList.SetCueSheet();
            }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
            if(GUILayout.Button("Set Cue Name List")){
                cueNameList.SetCueNameList();
                //var criAtomEditor = (CriAtomEditor)ScriptableObject.FindObjectOfType(typeof(CriAtomEditor));
                //criAtomEditor. = true;
            }
            if(GUILayout.Button("Set Acf")){
                cueNameList.SetAcf();
            }
        GUILayout.EndHorizontal();

        cueNameList.acfName = EditorWindow.GetWindow<CriAtomWindow>(false, null, false).acfInfoData.name;
        cueNameList.acbInfos = EditorWindow.GetWindow<CriAtomWindow>(false, null, false).acfInfoData.GetAcbInfoList(false, Application.streamingAssetsPath);

		if (GUI.changed) {
			EditorUtility.SetDirty(cueNameList);
		}
    }
}
