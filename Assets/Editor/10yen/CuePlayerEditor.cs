using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using SoundSystem;

[CustomEditor(typeof(CuePlayer))]
public class CuePlayerEditor : Editor{
    private CuePlayer cuePlayer = null;
    private CriAtomSource criAtomSource = null;
    private CueNameList cueNameList = null;
    
    //折り畳みUIの状態
    private bool isOpen = true;

    private void OnEnable(){
        cuePlayer = (CuePlayer)base.target;
        criAtomSource = (CriAtomSource)FindObjectOfType(typeof(CriAtomSource));
        cueNameList = (CueNameList)FindObjectOfType(typeof(CueNameList));
    }

    public override void OnInspectorGUI(){
        //base.OnInspectorGUI();
        if(cuePlayer == null){
            return;
        }
        Undo.RecordObject(target, null);

        GUI.changed = false;
        {
            EditorGUI.indentLevel++;
            GUI.enabled = false;
                criAtomSource = (CriAtomSource)EditorGUILayout.ObjectField("Cri Atom Source", cuePlayer.criAtomSource, typeof(CriAtomSource), true);
                cueNameList = (CueNameList)EditorGUILayout.ObjectField("Cue Name List", cuePlayer.cueInfo, typeof(CueNameList), true);
            GUI.enabled = true;

            //CueNameList
            isOpen = EditorGUILayout.Foldout(isOpen, "CueName");
            if(isOpen){
                EditorGUI.indentLevel++;
                for(int i = 0; i < cuePlayer.cueNameList.Count; i++){
                    EditorGUILayout.BeginHorizontal();
                        cuePlayer.cueNameList[i] = EditorGUILayout.TextField(i.ToString(), cuePlayer.cueNameList[i]);
                        if(GUILayout.Button("Delete", GUILayout.MaxWidth(60))){
                            cuePlayer.cueNameList.RemoveAt(i);
                        }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                if(GUILayout.Button("Add Cue Name", GUILayout.MaxWidth(120))){
                    cuePlayer.cueNameList.Add("");
                }
                GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        if(GUI.changed){
            EditorUtility.SetDirty(this.cuePlayer);
        }
    }
}
