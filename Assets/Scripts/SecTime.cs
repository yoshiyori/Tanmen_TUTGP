using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecTime : MonoBehaviour
{
    //旧仕様（今は使わない）

    //TimeManagerスクリプトから各種変数を参照する
    GameObject timeManager;
    TimeManager timeManagerScript;

    float secTime = 0f; //セクタータイム計算用変数

    //タイム表示関係
    int minutes, seconds, mseconds;
    public GameObject secTimeText; //セクタータイム表示テキスト

    private void Start()
    {
        //TimeMAnagerスクリプトから参照するための準備
        timeManager = GameObject.Find("TimeManager");
        timeManagerScript = timeManager.GetComponent<TimeManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {

            //テキスト表示用処理
            minutes = Mathf.FloorToInt(secTime / 60f);
            seconds = Mathf.FloorToInt(secTime % 60f);
            mseconds = Mathf.FloorToInt((secTime % 60f - seconds) * 1000);
            //secTimeText.GetComponent<Text>().text = string.Format("sec{0:0}　{1:00}:{2:00}.{3:000}", timeManagerScript.secNumber, minutes, seconds, mseconds);
            secTimeText.SetActive(!secTimeText.activeSelf);

            //セクタータイムを前のセクタータイムの合計値に足す
            //timeManagerScript.oldSecTime += secTime;
            //timeManagerScript.secNumber++; //セクターの数字が合うように1足す
        }
    }
}
