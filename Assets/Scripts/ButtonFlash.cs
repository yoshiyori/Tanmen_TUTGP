using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFlash : MonoBehaviour
{
    private float time = 0f;
    private float stopTimer;
    [SerializeField] private float stopTime;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private float katamukiNum;
    
    [SerializeField] Handle hd;
    private int selectNum;
    private bool selectStopFlag;

    void Start()
    {
        selectNum = 2;
        selectStopFlag = false;
    }
    void Update()
    {
        time += Time.deltaTime;


        if (time > 0.5f)
        {
            time = 0.0f;
            buttons[selectNum].SetActive(!buttons[selectNum].activeInHierarchy);
        }

        if (selectStopFlag == true)
        {
            stopTimer += Time.deltaTime;
            if (stopTimer > stopTime)
            {
                selectStopFlag = false;
                stopTimer = 0.0f;
            }
        }

        if (hd.GetControlllerAccel(1) < -katamukiNum && selectStopFlag == false)//right
        {
            selectStopFlag = true;
            if (buttons[selectNum].activeInHierarchy == false) buttons[selectNum].SetActive(true);
            if (selectNum < 4)
            {
                selectNum++;
                hd.JoyconRumble(0, 160, 320, 0.3f, 100);//第一引数が0で左コントローラー、他はSetRumble()の引数と同様
            }
            //Debug.Log(selectNum);
        }
        if (hd.GetControlllerAccel(1) > katamukiNum && selectStopFlag == false)//left
        {
            selectStopFlag = true;
            if (buttons[selectNum].activeInHierarchy == false) buttons[selectNum].SetActive(true);
            if (selectNum > 0)
            {
                selectNum--;
                hd.JoyconRumble(1, 160, 320, 0.3f, 100);//第一引数が1で右コントローラー、他はSetRumble()の引数と同様
            }
            //Debug.Log(selectNum);
        }

    }
}
