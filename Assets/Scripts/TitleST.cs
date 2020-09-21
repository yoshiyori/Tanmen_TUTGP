using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleST : MonoBehaviour
{
    [SerializeField] private GameObject flashObject;
    [SerializeField] private float flashInterval;
    [SerializeField] Handle hd;
    [SerializeField] FadeController fc;

    private float time;
    private bool isTransition;
    [SerializeField] private bool isGoRanking;

    [SerializeField] private GameObject titleCanvas;
    [SerializeField] private GameObject modeSelectCanvas;
    [SerializeField] private GameObject rankingCanvas;

    [SerializeField] private CuePlayer2D soundManager;                      //サウンド追加分 1/2

    [SerializeField] private int selectNum;
    private float stopTimer;                        //時間計測
    [SerializeField] private float stopTime;        //コントローラーを傾けっぱなしの場合、一気に端までいかないために一つ一つの選択項目に留めておく時間
    [SerializeField] private float katamukiNum;     //コントローラーをどこまで傾けたら横入力判定されるか
    private bool selectStopFlag;


    private void Start()
    {
        time = 0.0f;
        isTransition = false;
        isGoRanking = false;

        //サウンド追加分
        if(!soundManager.JudgeAtomSourceStatus("Playing", 1)){
            soundManager.Play("TitleBGM", 1);
        }
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > flashInterval)
        {
            time = 0.0f;
            flashObject.SetActive(!flashObject.activeInHierarchy);
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


        if ((hd.GetRightBrakeDown() == true || hd.GetLeftBrakeDown() == true) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            //fc.isFadeOut = true;
            isTransition = true;
        }


        if ( (Input.GetKeyDown(KeyCode.LeftArrow) ||
                (hd.GetControlllerAccel(0.2f, 1) > katamukiNum && selectStopFlag == false)
                ) ||
             (Input.GetKeyDown(KeyCode.RightArrow) ||
                (hd.GetControlllerAccel(0.2f, 1) < -katamukiNum && selectStopFlag == false)
                )
           )
        {
            isGoRanking = true;
            isTransition = true;
        }


        if (isTransition == true && fc.isFadeOut == false && isGoRanking == false)
        {
            isTransition = false;
            modeSelectCanvas.SetActive(!modeSelectCanvas.activeInHierarchy);
            titleCanvas.SetActive(!titleCanvas.activeInHierarchy);

            //サウンド追加分
            soundManager.Play("Decision");
            soundManager.Stop(1);
            soundManager.Play("MenuBGM", 1);
        }
        if (isTransition == true && fc.isFadeOut == false && isGoRanking == true)
        {
            isTransition = false;
            isGoRanking = false;
            rankingCanvas.SetActive(!rankingCanvas.activeInHierarchy);
            titleCanvas.SetActive(!titleCanvas.activeInHierarchy);
            
        }

    }
}
