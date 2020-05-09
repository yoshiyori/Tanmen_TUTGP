using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{

    bool startFlag = true; //スタート前演出追加した時用。今はずっとtrue
    [System.NonSerialized] public bool goalFlag = false;　//ゴール判断用
    [System.NonSerialized] public float totalTime; //全体のタイム計測

    //タイム表示関係
    int minutes, seconds, mseconds;
    public Text totalTimeText; //全体のタイム表示テキスト

    [System.NonSerialized] public int secNumber; //セクター表記用変数
    [System.NonSerialized] public float oldSecTime; //セクター計算用変数

    void Start()
    {
        //変数系初期化
        totalTime = 0.0f;
        oldSecTime = 0.0f;
        secNumber = 1;
        minutes = 0;
        seconds = 0;
        mseconds = 0;
    }

    void Update()
    {
        if (startFlag == false)
        {
            //スタート前処理を入れる時用（たぶんここには入れないだろうけど念のため）
        }
        else if (startFlag == true)
        {
            if(goalFlag == false)
            {
                totalTime += Time.deltaTime; //ここでタイム計測

                //テキスト表示用処理
                minutes = Mathf.FloorToInt(totalTime / 60f);
                seconds = Mathf.FloorToInt(totalTime % 60f);
                mseconds = Mathf.FloorToInt((totalTime % 60f - seconds) * 1000);
                totalTimeText.text = string.Format("Time　{0:00}:{1:00}.{2:000}", minutes, seconds, mseconds);

            }
            if(goalFlag == true)
            {
                //ゴールした時の処理を入れる（現状何もなし）
            }
        }
    }

}
