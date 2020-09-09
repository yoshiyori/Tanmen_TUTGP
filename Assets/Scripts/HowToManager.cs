using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToManager : MonoBehaviour
{
    [SerializeField] GameObject[] pageObjects;
    [SerializeField] GameObject beforeCanvas;
    [SerializeField] GameObject howToCanvas;
    [SerializeField] private int pageNum;
    [SerializeField] private int lastPageNum;

    private float stopTimer;                        //時間計測
    [SerializeField] private float stopTime;        //コントローラーを傾けっぱなしの場合、一気に端までいかないために一つ一つの選択項目に留めておく時間
    [SerializeField] private float katamukiNum;     //コントローラーをどこまで傾けたら横入力判定されるか
    private bool selectStopFlag;

    [SerializeField] Handle hd;
    [SerializeField] string sceanName;



    void Start()
    {
        pageNum = 0;
        lastPageNum = pageObjects.Length - 1;
        if (stopTime == 0) stopTime = 0.6f;
        
        
        
    }

    void Update()
    {
        stopTimer += Time.deltaTime;

        if (selectStopFlag == true)
        {
            stopTimer += Time.deltaTime;
            if (stopTimer > stopTime)
            {
                selectStopFlag = false;
                stopTimer = 0.0f;
            }
        }


        if (Input.GetKeyDown(KeyCode.LeftArrow) || (hd.GetControlllerAccel(0.2f, 1) > katamukiNum && selectStopFlag == false))
        {
            selectStopFlag = true;
            if (pageNum == 0)
            {
                beforeCanvas.SetActive(true);
                howToCanvas.SetActive(false);
            }
            else
            {
                pageObjects[pageNum - 1].SetActive(true);
                pageObjects[pageNum].SetActive(false);
                pageNum--;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || (hd.GetControlllerAccel(0.2f, 1) < -katamukiNum && selectStopFlag == false))
        {
            selectStopFlag = true;
            if (pageNum == lastPageNum)
            {
                GameManeger.gameStartFlag = true;
                GameManeger.goalFlag = false;
                
                //サウンド追加分
                CueManager.singleton.AddTimeStrechVoicePool();
                
                SceneManager.LoadSceneAsync(sceanName);
            }
            else
            {
                pageObjects[pageNum].SetActive(false);
                pageObjects[pageNum + 1].SetActive(true);
                pageNum++;
            }
        }

    }
}
