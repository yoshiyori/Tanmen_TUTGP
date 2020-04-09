using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CuePlayer))]
public class CuePlayerEditor : Editor{
    //コンポーネント
    private CuePlayer cuePlayer = null;
    private CriAtomSource criAtomSource = null;
    private CueManager cueManager = null;

    //拡張エディターインスタンス
    private CriAtomSourceEditor criAtomSourceEditor = null;
    
    //折り畳み表示の開閉フラグ
    private bool isOpen_AtomSource = true;
    private bool isOpen_CueName = true;

    //ポップアップメニューの選択インデックス
    private int selectedCueIndex_PlayOnStart = 0;
    private List<int> selectedCueIndex_CueName = new List<int>();

    //チェックボックスのフラグ
    private bool playFlag = false;

    private void OnEnable(){
        cuePlayer = (CuePlayer)base.target;
        criAtomSource = (CriAtomSource)FindObjectOfType(typeof(CriAtomSource));
        cueManager = (CueManager)FindObjectOfType(typeof(CueManager));        
        criAtomSourceEditor = (CriAtomSourceEditor)Resources.FindObjectsOfTypeAll(typeof(CriAtomSourceEditor))[0];
        
        foreach(var cueName in cuePlayer.CueNameList){
            selectedCueIndex_CueName.Add(0);
        }
    }

    public override void OnInspectorGUI(){
        if(cuePlayer == null){
            return;
        }
        Undo.RecordObject(target, null);

        GUI.changed = false;{
            EditorGUI.indentLevel++;

            //自身のCriAtomSource
            //自動で取得
            isOpen_AtomSource = EditorGUILayout.Foldout(isOpen_AtomSource, "Cri Atom Source");
            if(isOpen_AtomSource){
                GUI.enabled = false;
                EditorGUI.indentLevel++;
                    for(int i = 0; i < cuePlayer.CriAtomSourceList.Count; i++){
                        criAtomSource = (CriAtomSource)EditorGUILayout.ObjectField(i.ToString(), cuePlayer.CriAtomSourceList[i], typeof(CriAtomSource), true);
                    }
                EditorGUI.indentLevel--;
                GUI.enabled = true;
            }

            //シーン内のCueManager
            //基本的には自動で取得
            if(cueManager != null){
                GUI.enabled = false;
            }
            cueManager = (CueManager)EditorGUILayout.ObjectField("Sound Manager", cuePlayer.CueManager, typeof(CueManager), false);
            GUI.enabled = true;

            EditorGUILayout.BeginHorizontal();
                List<string> cueNames = new List<string>();
                if(cueManager != null){
                    foreach(var cueNameInfo in cueManager.ExCueInfoList){
                        cueNames.Add(cueNameInfo.CueName);
                    }
                }

                //PlayOnStartの可否
                if(cueNames.Count <= 0){
                    GUI.enabled = false;
                    cuePlayer.PlayOnStart = false;
                }
                cuePlayer.PlayOnStart = EditorGUILayout.Toggle("Play On Start", cuePlayer.PlayOnStart);
                GUI.enabled = true;

                //シーン開始時に鳴らすキューを選択するポップアップメメニュー
                if(!cuePlayer.PlayOnStart || cueNames == null){
                    GUI.enabled = false;
                }

                selectedCueIndex_PlayOnStart = cueNames.FindIndex(n => n.Equals(cuePlayer.PlayCueNameOnStart));

                selectedCueIndex_PlayOnStart = EditorGUILayout.Popup(selectedCueIndex_PlayOnStart, cueNames.ToArray());
                if((selectedCueIndex_PlayOnStart >= cueNames.Count) && (cueNames.Count > 0)){
                    selectedCueIndex_PlayOnStart = cueNames.Count - 1;
                }

                if(cuePlayer.PlayOnStart && (cueNames.Count > 0) && (selectedCueIndex_PlayOnStart >= 0)){
                    cuePlayer.PlayCueNameOnStart = cueNames[selectedCueIndex_PlayOnStart];
                }
                else{
                    cuePlayer.PlayCueNameOnStart = "";
                }
            EditorGUILayout.EndHorizontal();

            //使用するキューの一覧
            GUI.enabled = true;
            isOpen_CueName = EditorGUILayout.Foldout(isOpen_CueName, "Names of Cue to use");
            if(isOpen_CueName && cueNames != null){
                EditorGUI.indentLevel++;
                for(int i = 0; i < cuePlayer.CueNameList.Count; i++){
                    EditorGUILayout.BeginHorizontal();
                        selectedCueIndex_CueName[i] = cueNames.FindIndex(n => n.Equals(cuePlayer.CueNameList[i]));

                        if(selectedCueIndex_CueName[i] < 0){
                            selectedCueIndex_CueName[i] = 0;
                        }
                        selectedCueIndex_CueName[i] = EditorGUILayout.Popup(selectedCueIndex_CueName[i], cueNames.ToArray());
                        
                        if(selectedCueIndex_CueName[i] >= cueNames.Count){
                            selectedCueIndex_CueName[i] = cueNames.Count - 1;
                        }
                        if(selectedCueIndex_CueName[i] < 0){
                            selectedCueIndex_CueName[i] = 0;
                        }

                        if(cueNames.Count > 0){
                            cuePlayer.CueNameList[i] = cueNames[selectedCueIndex_CueName[i]];
                        }
                        else{
                            cuePlayer.CueNameList[i] = "";
                            GUI.enabled = false;
                        }

                        //プレビュー処理
                        if(!playFlag){
                            if(GUILayout.Button("Play", GUILayout.MaxWidth(60))){
                                cueManager.LoadCuePlayer();
                                cueManager.SetCueSheet();
                                PlayPreview(cuePlayer.CueNameList[i]);
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
                            cuePlayer.CueNameList.RemoveAt(i);
                            selectedCueIndex_CueName.RemoveAt(i);
                        }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;
            }
            GUI.enabled = true;
            EditorGUILayout.BeginHorizontal();
                GUILayout.Space(15);
                if(GUILayout.Button("Add Cue Name", GUILayout.MaxWidth(120))){
                    cuePlayer.CueNameList.Add("");
                    selectedCueIndex_CueName.Add(0);
                }
                GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        if(GUI.changed){
            EditorUtility.SetDirty(this.cuePlayer);
        }
    }

    //CriAtomSourceEditorからプレビュー再生用メソッドを呼び出す
    private void PlayPreview(string cueName){
        Type type = criAtomSourceEditor.GetType();
        MethodInfo method = type.GetMethod("StartPreviewPlayer", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);

        criAtomSource.cueSheet = cueManager.GetCueSheetName(cueName).CueSheetName;
        criAtomSource.cueName = cueName;
        method.Invoke(criAtomSourceEditor, null);
        playFlag = true;
    }

    //CriAtomSourceEditorからプレビュー停止用メソッドを呼び出す
    private void StopPreview(){
        Type type = criAtomSourceEditor.GetType();
        MethodInfo method = type.GetMethod("StopPreviewPlayer", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance);

        criAtomSource.cueSheet = "";
        criAtomSource.cueName = "";
        method.Invoke(criAtomSourceEditor, null);
        playFlag = false;
    }
}