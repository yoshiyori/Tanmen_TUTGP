using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using SoundSystem;

[CustomEditor(typeof(CuePlayer))]
public class CuePlayerEditor : Editor{
    //コンポーネント
    private CuePlayer cuePlayer = null;
    private CriAtomSource criAtomSource = null;
    private CueNameList cueNameList = null;
    
    //パラメーター
    private bool isOpen = true;
    private int selectedCueIndex = 0;
    bool isCueNameListInitialized = false;
    int lastSelectedCueIndex = -1;

    //SelectedCueIndexに負の数が入ることを防止
    private int SelectedCueIndex{
        set{
            if(value < 0){
                selectedCueIndex = 0;
            }
            else{
                selectedCueIndex = value;
            }
        }
        get{
            return selectedCueIndex;
        }
    }

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
            GUI.enabled = true;
            cueNameList = (CueNameList)EditorGUILayout.ObjectField("Cue Name List", cuePlayer.cueInfo, typeof(CueNameList), false);

            EditorGUILayout.BeginHorizontal();
                if(cuePlayer.cueNameList.Count <= 0){
                    GUI.enabled = false;
                }
                cuePlayer.playOnStart = EditorGUILayout.Toggle("Play On Start", cuePlayer.playOnStart);

                //シーン開始時に鳴らすキューを選択するプルダウンメニュー
                if(!cuePlayer.playOnStart){
                    GUI.enabled = false;
                }
                if(isCueNameListInitialized){
                    lastSelectedCueIndex = SelectedCueIndex;
                }
                else{
                   isCueNameListInitialized = true;
                }
                SelectedCueIndex = EditorGUILayout.Popup(SelectedCueIndex, cuePlayer.cueNameList.ToArray());
                if(SelectedCueIndex >= cuePlayer.cueNameList.Count){
                        SelectedCueIndex = cuePlayer.cueNameList.Count - 1;
                }
                if(cuePlayer.playOnStart){
                    cuePlayer.playCueOnStart = cuePlayer.cueNameList[SelectedCueIndex];
                }
                else{
                    cuePlayer.playCueOnStart = "";
                }
            EditorGUILayout.EndHorizontal();

            //CueNameList
            GUI.enabled = true;
            isOpen = EditorGUILayout.Foldout(isOpen, "CueName");
            if(isOpen){
                EditorGUI.indentLevel++;
                for(int i = 0; i < cuePlayer.cueNameList.Count; i++){
                    EditorGUILayout.BeginHorizontal();
                        cuePlayer.cueNameList[i] = EditorGUILayout.TextField(i.ToString(), cuePlayer.cueNameList[i]);

                        if(cuePlayer.cueNameList[i].Equals("")){
                            GUI.enabled = false;
                        }
                        if(GUILayout.Button("Play", GUILayout.MaxWidth(60))){
                            cuePlayer.Play(cuePlayer.cueNameList[i]);
                        }

                        GUI.enabled = true;
                        if(GUILayout.Button("Delete", GUILayout.MaxWidth(60))){
                            cuePlayer.cueNameList.RemoveAt(i);
                            if(cuePlayer.cueNameList.Count <= 0){
                                cuePlayer.playOnStart = false;
                            }
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