using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[assembly:AssemblyIsEditorAssembly]
[CustomEditor(typeof(CueManager))]
public class CueManagerEditor : Editor{
    private CueManager cueManager = null;
    //private CriAtom criAtom = null;

    //パラメーター
    private bool isOpen_ExCueInfoList = true;
    private bool isOpen_CuePlayer = true;

    private void OnEnable(){
        cueManager = (CueManager)base.target;
    }

    public override void OnInspectorGUI(){
        Undo.RecordObject(target, null);
        GUI.changed = false;

        if(cueManager.CriAtom != null){
            GUI.enabled = false;
        }
        cueManager.CriAtom = (CriAtom)EditorGUILayout.ObjectField("Cri Atom", cueManager.CriAtom, typeof(CriAtom), true);

        GUI.enabled = true;
        isOpen_CuePlayer = EditorGUILayout.Foldout(isOpen_CuePlayer, "Cue Player in Scene");
        if(isOpen_CuePlayer){
            EditorGUI.indentLevel++;
                if(cueManager.CuePlayers != null){
                    for(int i = 0; i < cueManager.CuePlayers.Length; i++){
                        /*cueManager.CuePlayer[i] = (CuePlayer)*/EditorGUILayout.ObjectField(i.ToString(), cueManager.CuePlayers[i], typeof(CuePlayer), true);
                    }
                }
            EditorGUI.indentLevel--;
        }

        isOpen_ExCueInfoList = EditorGUILayout.Foldout(isOpen_ExCueInfoList, "Ex Cue Info");
        if(isOpen_ExCueInfoList){
            EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("No", GUILayout.MaxWidth(40));
                    EditorGUILayout.LabelField("Cue Sheet");
                    EditorGUILayout.LabelField("Cue");
                EditorGUILayout.EndHorizontal();
                if((cueManager.ExCueInfoList != null) && (cueManager.ExCueInfoList.Count > 0)){
                    string recentCueSheetName = "";
                    for(int i = 0; i < cueManager.ExCueInfoList.Count; i++){
                        if(!recentCueSheetName.Equals(cueManager.ExCueInfoList[i].CueSheetName)){
                            EditorGUILayoutEx.Separator(true);
                        }

                        EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField(i.ToString(), GUILayout.MaxWidth(40));
                            EditorGUILayout.LabelField(cueManager.ExCueInfoList[i].CueSheetName);
                            EditorGUILayout.LabelField(cueManager.ExCueInfoList[i].CueName);
                        EditorGUILayout.EndHorizontal();

                        recentCueSheetName = cueManager.ExCueInfoList[i].CueSheetName;
                    }
                }
            EditorGUI.indentLevel--;
        }

        GUILayout.BeginHorizontal();
            if(GUILayout.Button("Preparation for Play")){
                cueManager.LoadCuePlayer();
                cueManager.SetCueSheet();
            }
            if(GUILayout.Button("Get Ex Cue Info")){
                cueManager.SetCueNameList();
            }
        GUILayout.EndHorizontal();

        if(EditorWindow.GetWindow<CriAtomWindow>(false, null, false).acfInfoData != null){
            cueManager.AcbInfos = EditorWindow.GetWindow<CriAtomWindow>(false, null, false).acfInfoData.GetAcbInfoList(false, Application.streamingAssetsPath);
        }

		if (GUI.changed) {
			EditorUtility.SetDirty(cueManager);
		}
    }
}
