using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CuePlayer2D))]
public class CuePlayer2DEditor : Editor{
    //コンポーネント
    private CuePlayer2D cuePlayer2D = null;
    //private CriAtomSource criAtomSource = null;
    private CueManager cueManager = null;
    //private CriAtom criAtom = null;
    //private CriWareInitializer criWareInitializer = null;
    //private CriWareErrorHandler criWareErrorHandler = null;

    //拡張エディターインスタンス
    //private CriAtomSourceEditor criAtomSourceEditor = null;
    
    //折り畳み表示の開閉フラグ
    //private bool isOpen_AtomSource = true;
    private bool isOpen_Components = false;
    private bool isOpen_CueName = true;

    //ポップアップメニューの選択インデックス
    private int selectedCueIndex_PlayOnStart = 0;
    private List<int> selectedCueIndex_CueName = new List<int>();

    //チェックボックスのフラグ
    private bool playFlag = false;

    private void OnEnable(){
        cuePlayer2D = (CuePlayer2D)base.target;
        //criAtomSource = (CriAtomSource)FindObjectOfType(typeof(CriAtomSource));
        cueManager = (CueManager)FindObjectOfType(typeof(CueManager));
        //criAtom = (CriAtom)FindObjectOfType(typeof(CriAtom));
        //criWareInitializer = (CriWareInitializer)FindObjectOfType(typeof(CriWareInitializer));
        //criWareErrorHandler = (CriWareErrorHandler)FindObjectOfType(typeof(CriWareErrorHandler));
        
        foreach(var cueName in cuePlayer2D.CueNameList){
            selectedCueIndex_CueName.Add(0);
        }
    }

    public override void OnInspectorGUI(){
        if(cuePlayer2D == null){
            return;
        }
        Undo.RecordObject(target, null);

        GUI.changed = false;{
            EditorGUI.indentLevel++;

            //自身のCriAtomSource
            //自動で取得
            /*isOpen_AtomSource = EditorGUILayout.Foldout(isOpen_AtomSource, "Cri Atom Source");
            if(isOpen_AtomSource){
                GUI.enabled = false;
                EditorGUI.indentLevel++;
                    for(int i = 0; i < cuePlayer2D.CriAtomSourceList.Count; i++){
                        criAtomSource = (CriAtomSource)EditorGUILayout.ObjectField(i.ToString(), cuePlayer2D.CriAtomSourceList[i], typeof(CriAtomSource), true);
                    }
                EditorGUI.indentLevel--;
                GUI.enabled = true;
            }*/

            isOpen_Components = EditorGUILayout.Foldout(isOpen_Components, "SoundManager Components");
            if(isOpen_Components){
                //シーン内のCueManager
                //基本的には自動で取得
                if(cueManager != null){
                    GUI.enabled = false;
                }
                cueManager = (CueManager)EditorGUILayout.ObjectField("Sound Manager", cuePlayer2D.CueManager, typeof(CueManager), true);
                GUI.enabled = true;

                //シーン内のcriAtom
                //基本的には自動で取得
                if(cuePlayer2D.CriAtom != null){
                    GUI.enabled = false;
                }
                cuePlayer2D.CriAtom = (CriAtom)EditorGUILayout.ObjectField("Sound Manager", cuePlayer2D.CriAtom, typeof(CriAtom), true);
                GUI.enabled = true;

                //シーン内のCueManager
                //基本的には自動で取得
                if(cuePlayer2D.CriWareInitializer != null){
                    GUI.enabled = false;
                }
                cuePlayer2D.CriWareInitializer = (CriWareInitializer)EditorGUILayout.ObjectField("Sound Manager", cuePlayer2D.CriWareInitializer, typeof(CriWareInitializer), true);
                GUI.enabled = true;

                //シーン内のCueManager
                //基本的には自動で取得
                if(cuePlayer2D.CriWareErrorHandler != null){
                    GUI.enabled = false;
                }
                cuePlayer2D.CriWareErrorHandler = (CriWareErrorHandler)EditorGUILayout.ObjectField("Sound Manager", cuePlayer2D.CriWareErrorHandler, typeof(CriWareErrorHandler), true);
                GUI.enabled = true;
            }

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
                    cuePlayer2D.PlayOnStart = false;
                }
                cuePlayer2D.PlayOnStart = EditorGUILayout.Toggle("Play On Start", cuePlayer2D.PlayOnStart);
                GUI.enabled = true;

                //シーン開始時に鳴らすキューを選択するポップアップメメニュー
                if(!cuePlayer2D.PlayOnStart || cueNames == null){
                    GUI.enabled = false;
                }

                /*selectedCueIndex_PlayOnStart = cueNames.FindIndex(n => n.Equals(cuePlayer2D.PlayCueNameOnStart));

                selectedCueIndex_PlayOnStart = EditorGUILayout.Popup(selectedCueIndex_PlayOnStart, cueNames.ToArray());
                if((selectedCueIndex_PlayOnStart >= cueNames.Count) && (cueNames.Count > 0)){
                    selectedCueIndex_PlayOnStart = cueNames.Count - 1;
                }

                if(cuePlayer2D.PlayOnStart && (cueNames.Count > 0) && (selectedCueIndex_PlayOnStart >= 0)){
                    cuePlayer2D.PlayCueNameOnStart = cueNames[selectedCueIndex_PlayOnStart];
                }
                else{
                    cuePlayer2D.PlayCueNameOnStart = "";
                }*/
                cuePlayer2D.PlayCueNameOnStart = EditorGUILayout.TextField(cuePlayer2D.PlayCueNameOnStart);
            EditorGUILayout.EndHorizontal();

            //使用するキューの一覧
            GUI.enabled = true;
            isOpen_CueName = EditorGUILayout.Foldout(isOpen_CueName, "Names of Cue to use");
            if(isOpen_CueName && cueNames != null){
                EditorGUI.indentLevel++;
                for(int i = 0; i < cuePlayer2D.CueNameList.Count; i++){
                    EditorGUILayout.BeginHorizontal();
                        cuePlayer2D.CueNameList[i] = EditorGUILayout.TextField(cuePlayer2D.CueNameList[i]);
                        /*selectedCueIndex_CueName[i] = cueNames.FindIndex(n => n.Equals(cuePlayer2D.CueNameList[i]));

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
                            cuePlayer2D.CueNameList[i] = cueNames[selectedCueIndex_CueName[i]];
                        }
                        else{
                            cuePlayer2D.CueNameList[i] = "";
                            GUI.enabled = false;
                        }*/

                        //プレビュー処理
                        /*if(!playFlag){
                            if(GUILayout.Button("Play", GUILayout.MaxWidth(60))){
                                cueManager.LoadCuePlayer();
                                cueManager.SetCueSheet();
                                PlayPreview(cuePlayer2D.CueNameList[i]);
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
                        }*/
                        if(GUILayout.Button("Delete", GUILayout.MaxWidth(60))){
                            cuePlayer2D.CueNameList.RemoveAt(i);
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
                    cuePlayer2D.CueNameList.Add("");
                    selectedCueIndex_CueName.Add(0);
                }
                GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        if(GUI.changed){
            EditorUtility.SetDirty(this.cuePlayer2D);
        }
    }

    //CriAtomSourceEditorからプレビュー再生用メソッドを呼び出す
    /*private void PlayPreview(string cueName){
        criAtomSource = cuePlayer2D.gameObject.AddComponent<CriAtomSource>();
        criAtomSourceEditor = (CriAtomSourceEditor)Resources.FindObjectsOfTypeAll(typeof(CriAtomSourceEditor))[0];
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
        Destroy(criAtomSource);
    }*/
}