using System;
using System.Collections;
using System.Collections.Generic;
//using System.IO;
using System.Reflection;
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
    private CriAtomSourceEditor criAtomSourceEditor = null;
    
    //パラメーター
    private bool isOpen = true;
    private int selectedCueIndex_PlayOnStart = 0;
    bool isCueNameListInitialized = false;
    private int lastSelectedCueIndex_PlayOnStart = -1;
    private bool playFlag = false;

    //SelectedCueIndexに負の数が入ることを防止
    private int SelectedCueIndex_PlayOnStart{
        set{
            if(value < 0){
                selectedCueIndex_PlayOnStart = 0;
            }
            else{
                selectedCueIndex_PlayOnStart = value;
            }
        }
        get{
            return selectedCueIndex_PlayOnStart;
        }
    }

    private void OnEnable(){
        cuePlayer = (CuePlayer)base.target;
        criAtomSource = (CriAtomSource)FindObjectOfType(typeof(CriAtomSource));
        cueNameList = (CueNameList)FindObjectOfType(typeof(CueNameList));        
        criAtomSourceEditor = (CriAtomSourceEditor)Resources.FindObjectsOfTypeAll(typeof(CriAtomSourceEditor))[0];
        //criAtom = (CriAtom)FindObjectOfType(typeof(CriAtom));
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

            //自身のCriAtomSource
            //基本的には自動で取得
            if(criAtomSource != null){
                GUI.enabled = false;
            }
            criAtomSource = (CriAtomSource)EditorGUILayout.ObjectField("Cri Atom Source", cuePlayer.criAtomSource, typeof(CriAtomSource), true);
            GUI.enabled = true;

            //シーン内のCueNameList
            //基本的には自動で取得
            if(cueNameList != null){
                GUI.enabled = false;
            }
            cueNameList = (CueNameList)EditorGUILayout.ObjectField("Cue Name List", cuePlayer.cueInfo, typeof(CueNameList), false);
            GUI.enabled = true;

            EditorGUILayout.BeginHorizontal();
                List<string> cueNames = new List<string>();
                foreach(var cueNameInfo in cueNameList.cueNameInfos){
                    cueNames.Add(cueNameInfo.cueName);
                }

                //PlayOnStartの可否
                if(cueNames.Count <= 0){
                    GUI.enabled = false;
                    cuePlayer.playOnStart = false;
                }
                cuePlayer.playOnStart = EditorGUILayout.Toggle("Play On Start", cuePlayer.playOnStart);
                GUI.enabled = true;

                //シーン開始時に鳴らすキューを選択するプルダウンメニュー
                if(!cuePlayer.playOnStart){
                    GUI.enabled = false;
                }

                SelectedCueIndex_PlayOnStart = cueNames.FindIndex(n => n.Equals(cuePlayer.playCueOnStart));

                SelectedCueIndex_PlayOnStart = EditorGUILayout.Popup(SelectedCueIndex_PlayOnStart, cueNames.ToArray());
                if((SelectedCueIndex_PlayOnStart >= cueNames.Count) && (cueNames.Count > 0)){
                    SelectedCueIndex_PlayOnStart = cueNames.Count - 1;
                }

                if(cuePlayer.playOnStart && (cueNames.Count > 0)){
                    cuePlayer.playCueOnStart = cueNames[SelectedCueIndex_PlayOnStart];
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

                        if(!playFlag){
                            if(GUILayout.Button("Play", GUILayout.MaxWidth(60))){
                                PlayPreview(cuePlayer.cueNameList[i]);
                            }
                        }
                        if(playFlag){
                            if(GUILayout.Button("Stop", GUILayout.MaxWidth(60))){
                                StopPreview();
                            }
                        }

                        GUI.enabled = true;
                        if(playFlag){
                            GUI.enabled = false;
                        }
                        if(GUILayout.Button("Delete", GUILayout.MaxWidth(60))){
                            cuePlayer.cueNameList.RemoveAt(i);
                        }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
            GUI.enabled = true;
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

    private void PlayPreview(string cueName){
        Type type = criAtomSourceEditor.GetType();
        MethodInfo method = type.GetMethod("StartPreviewPlayer", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);

        criAtomSource.cueSheet = cueNameList.GetCueNameInfo(cueName).cueSheetName;
        criAtomSource.cueName = cueName;
        method.Invoke(criAtomSourceEditor, null);
        playFlag = true;
    }

    private void StopPreview(){
        Type type = criAtomSourceEditor.GetType();
        MethodInfo method = type.GetMethod("StopPreviewPlayer", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);

        criAtomSource.cueSheet = "";
        criAtomSource.cueName = "";
        method.Invoke(criAtomSourceEditor, null);
        playFlag = false;
    }
}