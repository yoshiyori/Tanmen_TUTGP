using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CourseSelectST : MonoBehaviour         //ST == SceneTransition
{
    [SerializeField] private string[] sceneName;
    [SerializeField] private int selectNum;
    private int frameNum;
    private float time = 0f;
    private float stopTimer;                        //時間計測
    [SerializeField] private float stopTime;        //コントローラーを傾けっぱなしの場合、一気に端までいかないために一つ一つの選択項目に留めておく時間
    [SerializeField] private GameObject[] frames;   //selectNum == -2はsceneName[3]、selectNum == -1はsceneName[4]に該当
    [SerializeField] private float katamukiNum;     //コントローラーをどこまで傾けたら横入力判定されるか

    [SerializeField] Handle hd;
    [SerializeField] FadeController fc;
    private bool selectStopFlag;
    private bool isTransition;


    void Start()
    {
        if (sceneName == null)
        {
            sceneName[0] = "KbAndJcTestScene";//コース１
            sceneName[1] = "KbAndJcTestScene";//コース２
            sceneName[2] = "KbAndJcTestScene";//コース３
            sceneName[3] = "KbAndJcTestScene";//チュートリアル　遊び方
            sceneName[4] = "KbAndJcTestScene";//設定（オプション）
        }
        selectNum = 0;
        selectStopFlag = false;
        isTransition = false;
    }


    void Update()
    {
        time += Time.deltaTime;

        if (time > 0.5f)
        {
            time = 0.0f;
            if (selectNum >= 0) frames[selectNum].SetActive(!frames[selectNum].activeInHierarchy);
            else if (selectNum < 0)
            {
                if (selectNum == -2) frames[3].SetActive(!frames[3].activeInHierarchy);
                else frames[4].SetActive(!frames[4].activeInHierarchy);
            }
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

        if ((hd.GetRightBrake() == true || hd.GetLeftBrake() == true) ||
            Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            isTransition = true;
            fc.isFadeOut = true;
        }

        if (isTransition == true && fc.isFadeOut == false)
        {
            if (selectNum > 0) SceneManager.LoadScene(sceneName[selectNum]);
            else if (selectNum < 0)
            {
                if (selectNum == -2) SceneManager.LoadScene(sceneName[3]);
                else SceneManager.LoadScene(sceneName[4]);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            (hd.GetControlllerAccel(1) > katamukiNum && selectStopFlag == false)
            )
        {
            if(selectNum > 0 || selectNum == -1)
            {
                if (selectNum > 0 && frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
                if (selectNum == -1 && frames[4].activeInHierarchy == true) frames[4].SetActive(false);
                selectNum--;
                hd.JoyconRumble(1, 160, 320, 0.3f, 100);//第一引数が1で右コントローラー、他はSetRumble()の引数と同様
            }
            selectStopFlag = true;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || 
            (hd.GetControlllerAccel(1) < -katamukiNum && selectStopFlag == false)
            )
        {
            if ((selectNum < 2 && selectNum >= 0)  || selectNum == -2)
            {
                if (selectNum < 2 && selectNum >= 0 && frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
                if (selectNum == -2 && frames[3].activeInHierarchy == true) frames[3].SetActive(false);
                selectNum++;
                hd.JoyconRumble(0, 160, 320, 0.3f, 100);//第一引数が0で左コントローラー、他はSetRumble()の引数と同様
            }
            selectStopFlag = true;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || hd.GetControllerSwing() <= -8)
        {
            if (frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
            if (selectNum < 2)
            {
                selectNum = -2;
                hd.JoyconRumble(0, 160, 320, 0.1f, 100);
                hd.JoyconRumble(1, 160, 320, 0.1f, 100);
            }
            else
            {
                selectNum = -1;
                hd.JoyconRumble(0, 160, 320, 0.1f, 100);
                hd.JoyconRumble(1, 160, 320, 0.1f, 100);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || hd.GetControllerSwing() >= 8)
        {
            if (selectNum == -2)
            {
                if (frames[3].activeInHierarchy == true) frames[3].SetActive(false);
                selectNum = 0;
                hd.JoyconRumble(0, 160, 320, 0.1f, 100);
                hd.JoyconRumble(1, 160, 320, 0.1f, 100);
            }
            else if (selectNum == -1)
            {
                if (frames[4].activeInHierarchy == true) frames[4].SetActive(false);
                selectNum = 2;
                hd.JoyconRumble(0, 160, 320, 0.1f, 100);
                hd.JoyconRumble(1, 160, 320, 0.1f, 100);
            }
            
        }

    }

}
