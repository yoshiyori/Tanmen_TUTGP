using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using SoundSystem;

[CustomEditor(typeof(ADX_CueBank))]
public class ADX_CueBankEditor : Editor{
    private ADX_CueBank adx_CueBank = null;
    private CriAtomSource criAtomSource = null;
    
    //折り畳みUIの状態
    private bool isOpen = true;

    private void OnEnable(){
        adx_CueBank = (ADX_CueBank)base.target;
        criAtomSource = (CriAtomSource)FindObjectOfType(typeof(CriAtomSource));
    }

    public override void OnInspectorGUI(){
        //base.OnInspectorGUI();
        if(adx_CueBank == null){
            return;
        }
        Undo.RecordObject(target, null);

        GUI.changed = false;
        {
            EditorGUI.indentLevel++;
            criAtomSource = (CriAtomSource)EditorGUILayout.ObjectField("Cri Atom Source", adx_CueBank.criAtomSource, typeof(CriAtomSource), false);
            adx_CueBank.cueSheetName = EditorGUILayout.TextField("Cue Sheet Name", adx_CueBank.cueSheetName);

            //CueNameList
            EditorGUILayout.BeginHorizontal();
                isOpen = EditorGUILayout.Foldout(isOpen, "CueName");
                if(GUILayout.Button("Add", GUILayout.MaxWidth(60))){
                    adx_CueBank.cueNameList.Add("");
                }
                GUILayout.Space(190);
            EditorGUILayout.EndHorizontal();
            if(isOpen){
                EditorGUI.indentLevel++;
                for(int i = 0; i < adx_CueBank.cueNameList.Count; i++){
                    EditorGUILayout.BeginHorizontal();
                        adx_CueBank.cueNameList[i] = EditorGUILayout.TextField(i.ToString(), adx_CueBank.cueNameList[i]);
                        if(GUILayout.Button("Delete", GUILayout.MaxWidth(60))){
                            adx_CueBank.cueNameList.RemoveAt(i);
                        }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}
