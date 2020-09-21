using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectST : MonoBehaviour
{
    [SerializeField] private string[] modeName;
    private int selectNum;
    private int frameNum;
    private float time = 0f;
    private float stopTimer;                        //時間計測
    [SerializeField] private float stopTime;        //コントローラーを傾けっぱなしの場合、一気に端までいかないために一つ一つの選択項目に留めておく時間
    [SerializeField] private GameObject[] frames;   //selectNum == -2はsceneName[3]、selectNum == -1はsceneName[4]に該当
    [SerializeField] private float katamukiNum;     //コントローラーをどこまで傾けたら横入力判定されるか

    [SerializeField] private GameObject modeSelectCanvas;
    [SerializeField] private GameObject courseSelectCanvas;
    [SerializeField] private GameObject configCanvas;
    [SerializeField] private GameObject titleCanvas;

    [SerializeField] Handle hd;
    //[SerializeField] FadeController fc;
    private bool selectStopFlag;
    private bool isTransition;
    [SerializeField] private bool isConnectJoycon;

    [SerializeField]　private CuePlayer2D soundManager;         //サウンド追加分 1/5

    void Start()
    {
        if (modeName == null)
        {
            modeName[0] = "main";//コース１
            modeName[1] = "main";//コース２
            modeName[2] = "main";//コース３
        }
        selectNum = 0;
        selectStopFlag = false;
        isTransition = false;
        //if (modeSelectCanvas.activeInHierarchy == false) modeSelectCanvas.SetActive(true);
        if (stopTime == 0) stopTime = 0.6f;
        if (katamukiNum == 0) katamukiNum = 0.5f;
        if (hd.isConnectHandle) isConnectJoycon = true;
    }


    void Update()
    {
        time += Time.deltaTime;

        if (time > 0.5f)
        {
            time = 0.0f;
            frames[selectNum].SetActive(!frames[selectNum].activeInHierarchy);
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

        if ((hd.GetRightBrakeDown() == true) ||
            Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            isTransition = true;
            //Debug.Log("ModeselectPushSpace");
            //fc.isFadeOut = true;
        }

        if (hd.GetLeftBrakeDown() == true || Input.GetKeyDown(KeyCode.Backspace))
        {
            if (frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
            isTransition = true;
            selectNum = 3;
        }

        if (isTransition == true/* && fc.isFadeOut == false*/)
        {
            if (selectNum == 1)
            {
                if (frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
                modeSelectCanvas.SetActive(!modeSelectCanvas.activeInHierarchy);
                courseSelectCanvas.SetActive(!courseSelectCanvas.activeInHierarchy);
                soundManager.Play("Decision");                                                          //サウンド追加分 2/5
            }
            else if (selectNum == 2)
            {
                if (frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
                modeSelectCanvas.SetActive(!modeSelectCanvas.activeInHierarchy);
                configCanvas.SetActive(!configCanvas.activeInHierarchy);
                soundManager.Play("Decision");                                                          //サウンド追加分 3/5
            }
            else if (selectNum == 3)
            {
                modeSelectCanvas.SetActive(!modeSelectCanvas.activeInHierarchy);
                titleCanvas.SetActive(!titleCanvas.activeInHierarchy);
                soundManager.Play("Decision");
            }
            else
            {
                //soundManager.PlayOnSceneSwitch("Decision");
                SceneManager.LoadScene(modeName[selectNum], LoadSceneMode.Single);
            }
            selectNum = 0;
            isTransition = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            (hd.GetControlllerAccel(0.2f, 1) > katamukiNum && selectStopFlag == false)
            )
        {
            if (selectNum > 0)
            {
                if (frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
                selectNum--;
                soundManager.Play("Select");                                                            //サウンド追加分 4/5
                if (isConnectJoycon) hd.JoyconRumble(1, 160, 320, 0.3f, 100);//第一引数が1で右コントローラー、他はSetRumble()の引数と同様
            }
            selectStopFlag = true;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            (hd.GetControlllerAccel(0.2f, 1) < -katamukiNum && selectStopFlag == false)
            )
        {
            if ((selectNum < 2 && selectNum >= 0))
            {
                if (frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
                selectNum++;
                soundManager.Play("Select");                                                            //サウンド追加分 5/5
                if (isConnectJoycon) hd.JoyconRumble(0, 160, 320, 0.3f, 100);//第一引数が0で左コントローラー、他はSetRumble()の引数と同様
            }
            selectStopFlag = true;
        }


    }
}
