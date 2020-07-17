using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectST : MonoBehaviour
{
    [SerializeField] private int selectNum;
    private float time = 0f;                        
    private float stopTimer;                        //時間計測
    [SerializeField] private float stopTime;        //コントローラーを傾けっぱなしの場合、一気に端までいかないために一つ一つの選択項目に留めておく時間
    [SerializeField] private GameObject[] frames;
    [SerializeField] private float katamukiNum;     //コントローラーをどこまで傾けたら横入力判定されるか
    [SerializeField] Handle hd;
    private bool selectStopFlag;
    [SerializeField] private bool isConnectJoycon;

    //ポーズ用の処理。変数系はポーズの時だけ使う
    [SerializeField] bool inPause;
    private float lastRealTime;
    private float lastStopTime;

    [SerializeField] private CuePlayer2D soundManager;         //サウンド追加分 1/5

    void Start()
    {
        selectNum = 0;
        selectStopFlag = false;
        if (stopTime == 0) stopTime = 0.6f;
        if (katamukiNum == 0) katamukiNum = 0.5f;
        if (hd.isConnectHandle == true) isConnectJoycon = true;
        lastRealTime = 0.0f;
        lastStopTime = 0.0f;
    }

    void Update()
    {
        if(AlertSet.alertFlag == true)
        {
            frames[selectNum].SetActive(false);
        }
        else
        {
            frames[selectNum].SetActive(true);

            if (inPause == true)
            {
                time = Time.realtimeSinceStartup - lastRealTime;

                if (time > 0.5f)
                {
                    var frame = frames[selectNum].GetComponent<Image>();
                    time = 0.0f;
                    lastRealTime = Time.realtimeSinceStartup;
                    frame.enabled = !frame.enabled;
                }

                if (selectStopFlag == true)
                {
                    stopTimer = Time.realtimeSinceStartup - lastStopTime;
                    if (stopTimer > stopTime)
                    {
                        selectStopFlag = false;
                        stopTimer = 0.0f;
                        lastStopTime = Time.realtimeSinceStartup;
                    }
                }
            }
            else
            {
                time += Time.deltaTime;

                if (time > 0.5f)
                {
                    time = 0.0f;
                    var frame = frames[selectNum].GetComponent<Image>();
                    frame.enabled = !frame.enabled;
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
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) ||
                (hd.GetControlllerAccel(0.2f, 1) > katamukiNum && selectStopFlag == false)
                )
            {
                if (selectNum >= 0)
                {
                    frames[selectNum].SetActive(false);
                    selectNum--;
                    if(selectNum == -1)
                    {
                        selectNum = frames.Length - 1;
                    }
                    frames[selectNum].SetActive(true);
                    soundManager.Play("Select",1);                                                            //サウンド追加分 4/5
                                                                                                            //if (isConnectJoycon) hd.JoyconRumble(1, 160, 320, 0.3f, 100);//第一引数が1で右コントローラー、他はSetRumble()の引数と同様（元の仕様変更が必要なため一時オミット）
                }
                selectStopFlag = true;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) ||
                (hd.GetControlllerAccel(0.2f, 1) < -katamukiNum && selectStopFlag == false)
                )
            {
                if ((selectNum < 2 && selectNum >= 0))
                {
                    frames[selectNum].SetActive(false);
                    selectNum++;
                    if(selectNum == frames.Length)
                    {
                        selectNum = 0;
                    }
                    frames[selectNum].SetActive(true);
                    soundManager.Play("Select",1);                                                            //サウンド追加分 5/5
                                                                                                            //if (isConnectJoycon) hd.JoyconRumble(0, 160, 320, 0.3f, 100);//第一引数が0で左コントローラー、他はSetRumble()の引数と同様（元の仕様変更が必要なため一時オミット）
                }
                selectStopFlag = true;
            }
        }
        
    }

    private void OnDisable()
    {
        if(frames[selectNum].GetComponent<CanvasActive>() == null)
        {
            frames[selectNum].SetActive(false);
            selectNum = 0;
            frames[selectNum].SetActive(true);
        }
    }
}
