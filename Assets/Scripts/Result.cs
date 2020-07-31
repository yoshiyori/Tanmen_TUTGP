using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] GameObject[] resultSecTimeText;
    [SerializeField] GameObject resultTotalTimeText;
    [SerializeField] TimeManager timeManager;
    [SerializeField] Handle hd;
    [SerializeField] private CuePlayer2D soundManager;
    int minutes, seconds, mseconds;

    // Update is called once per frame
    void Update()
    {
        if(Goal.resultFlag == true)
        {
            for (int i = 0; i < resultSecTimeText.Length; i++)
            {
                minutes = Mathf.FloorToInt(timeManager.secTime[i] / 60f);
                seconds = Mathf.FloorToInt(timeManager.secTime[i] % 60f);
                mseconds = Mathf.FloorToInt((timeManager.secTime[i] % 60f - seconds) * 1000);
                resultSecTimeText[i].GetComponent<Text>().text = string.Format("sec{0:0}　{1:00}:{2:00}.{3:000}", i + 1, minutes, seconds, mseconds);
                resultSecTimeText[i].SetActive(true);
            }
            minutes = Mathf.FloorToInt(timeManager.totalTime / 60f);
            seconds = Mathf.FloorToInt(timeManager.totalTime % 60f);
            mseconds = Mathf.FloorToInt((timeManager.totalTime % 60f - seconds) * 1000);
            resultTotalTimeText.GetComponent<Text>().text = string.Format("Time　{0:00}:{1:00}.{2:000}", minutes, seconds, mseconds);
            resultTotalTimeText.SetActive(true);

            //リザルト画面からランキングに移動するときの処理
            if (hd.GetRightBrakeDown() == true
            || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                soundManager.Play("Decision", 1);
            }
        }
    }
}
