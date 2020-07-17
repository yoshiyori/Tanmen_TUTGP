using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] GameObject CountDownText;
    [SerializeField] float startCountDownTime;
    [SerializeField] GameObject startText;
    float countDownTime;
    int countDownSeconds;

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
        countDownTime = startCountDownTime;
        CountDownText.SetActive(true);
        startText.SetActive(false);
    }

    void Update()
    {
        if (GameManeger.gameStartFlag == true)
        {
            countDownTime -= Time.deltaTime;
            countDownSeconds = (int)countDownTime + 1;
            CountDownText.GetComponent<Text>().text = countDownSeconds.ToString();
            if(countDownTime <= 0)
            {
                CountDownText.SetActive(false);
                startText.SetActive(true);
                GameManeger.gameStartFlag = false;
            }
        }
        else if (GameManeger.gameStartFlag == false)
        {
            if(GameManeger.goalFlag == false)
            {
                totalTime += Time.deltaTime; //ここでタイム計測

                if(startText.activeSelf == true)
                {
                    if(totalTime >= 0.5f)
                    {
                        startText.SetActive(false);
                    }
                }

                //テキスト表示用処理
                minutes = Mathf.FloorToInt(totalTime / 60f);
                seconds = Mathf.FloorToInt(totalTime % 60f);
                mseconds = Mathf.FloorToInt((totalTime % 60f - seconds) * 1000);
                totalTimeText.text = string.Format("Time　{0:00}:{1:00}.{2:000}", minutes, seconds, mseconds);

            }
            if(GameManeger.goalFlag == true)
            {
                //ゴールした時の処理を入れる（現状何もなし）
            }
        }
    }

}
