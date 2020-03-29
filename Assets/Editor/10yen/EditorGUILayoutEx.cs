using UnityEngine;
using UnityEditor;

public class EditorGUILayoutEx {

    /// <summary>
    /// インデントレベル設定を考慮した仕切り線.
    /// </summary>
    /// <param name="useIndentLevel">インデントレベルを考慮するか.</param>
    ///引用元:https://qiita.com/Gok/items/96e8747269bf4a2a9cc5
    public static void Separator(bool useIndentLevel = false)
    {
        EditorGUILayout.BeginHorizontal();
        if (useIndentLevel){
            GUILayout.Space(EditorGUI.indentLevel * 15);
        }
        GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
        EditorGUILayout.EndHorizontal();
    }
}