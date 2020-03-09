
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*---------------------------------------------------------------
ViewOnlyAttribute:フィールドをインスペクターから編集不可能にする属性
    参考:https://kandycodings.jp/2019/03/24/unidev-noneditable/
---------------------------------------------------------------*/

public sealed class NonEditableAttribute : PropertyAttribute{}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(NonEditableAttribute))]
public sealed class NonEditableAttributeDrawer : PropertyDrawer{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif