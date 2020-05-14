using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfigManager : MonoBehaviour
{
    private int selectNum;
    private int frameNum;
    private float time = 0f;
    private float stopTimer;                        //時間計測
    [SerializeField] private float stopTime;        //コントローラーを傾けっぱなしの場合、一気に端までいかないために一つ一つの選択項目に留めておく時間
    [SerializeField] private GameObject[] frames;   //selectNum == -2はsceneName[3]、selectNum == -1はsceneName[4]に該当
    [SerializeField] private float katamukiNum;     //コントローラーをどこまで傾けたら横入力判定されるか

    [SerializeField] private bool isIngame; //ゲームしてる時Pauseで行くときはTrue,モード選択から行くときはFalse

    private GameObject Slider01;
    private GameObject Slider02;
    private GameObject Slider03;

    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;
    [SerializeField] private Slider handleSlider;

    private float bgmSliderDivisionNum; //BGMスライダーの分割数。偶数だと思って作ったので奇数にする際は連絡をお願いします。
    private float seSliderDivisionNum;　//SEスライダーの分割数。                     〃
    private float handleSliderDivisionNum;　//Handleスライダーの分割数。             〃 

    [SerializeField] private GameObject configCanvas;
    [SerializeField] private GameObject returnCanvas;//config入る前に使ってたキャンバス(キャンバス使ってなかったら違うのまた作る)
    [SerializeField] private GameObject configAlertYesPanel;
    [SerializeField] private GameObject configAlertNoPanel;

    [SerializeField] Handle hd;
    //[SerializeField] FadeController fc;
    private bool selectStopFlag;
    private bool isTransition;
    [SerializeField] private bool isAlertStandup;
    private bool isConnectJoycon;
    [SerializeField] private bool choosingModeFlag;
    private bool alertStandingFlag;

    [SerializeField] private CuePlayer2D soundManager;                          //サウンド追加分 1/15

    void Start()
    {
        selectNum = 0;
        selectStopFlag = false;
        isTransition = false;
        if (stopTime == 0) stopTime = 0.6f;
        if (katamukiNum == 0) katamukiNum = 0.5f;

        bgmSliderDivisionNum = 10.0f;      //とりあえず10とする。偶数で良い感じの数字を入れてもらいたい。
        seSliderDivisionNum = 10.0f;       //                      〃
        handleSliderDivisionNum = 10.0f;   //                      〃

        //↓デフォルトの値を0.5とする。とりあえず今はここで0.5とするが、本当は実際の値を引っ張ってきて反映させる。
        bgmSlider.value = 0.5f;
        seSlider.value = 0.5f;
        handleSlider.value = 0.5f;
        isAlertStandup = false;
        if (hd.isConnectHandle) isConnectJoycon = true;
        choosingModeFlag = false;
        alertStandingFlag = false;
    }


    void Update()
    {
        if (isAlertStandup == true && alertStandingFlag == false) alertStandingFlag = true;
        if (choosingModeFlag == true) time += Time.deltaTime;

        if (time > 0.4f)//点滅間隔
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


        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            (hd.GetControlllerAccel(1) < -katamukiNum && selectStopFlag == false)
            )
        {
            if (choosingModeFlag == false)
            {
                if (frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
                selectNum++;
                if (selectNum > 4) selectNum = 0;
                if (frames[selectNum].activeInHierarchy == false) frames[selectNum].SetActive(true);
                soundManager.Play("Select");                                         //サウンド追加分 6/15
                if (isConnectJoycon) hd.JoyconRumble(0, 160, 320, 0.2f, 50);//第一引数が0で左コントローラー(右手で持つ) 、他はSetRumble()の引数と同様
                selectStopFlag = true;
            }
            else
            {
                if (selectNum < 3)
                {
                    if (selectNum == 0 && bgmSlider.value < 1)
                    {
                        soundManager.Play("Increase");                                  //サウンド追加分 12/15
                        bgmSlider.value += 1.0f / bgmSliderDivisionNum;
                    }
                    if (selectNum == 1 && seSlider.value < 1)
                    {
                        soundManager.Play("Increase");                                  //サウンド追加分 13/15
                        seSlider.value += 1.0f / seSliderDivisionNum;
                    }
                    if (selectNum == 2 && handleSlider.value < 1)
                    {
                        soundManager.Play("Increase");                                  //サウンド追加分 14/15
                        handleSlider.value += 1.0f / handleSliderDivisionNum;
                    }
                    if (isConnectJoycon) hd.JoyconRumble(0, 160, 320, 0.2f, 50);//第一引数が0で左コントローラー(右手で持つ) 、他はSetRumble()の引数と同様
                }
                selectStopFlag = true;
            }
        }


        if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            (hd.GetControlllerAccel(1) > katamukiNum && selectStopFlag == false)
            )
        {
            if (choosingModeFlag == false)
            {
                if (frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
                selectNum--;
                if (selectNum < 0) selectNum = 4;
                if (frames[selectNum].activeInHierarchy == false) frames[selectNum].SetActive(true);
                soundManager.Play("Select");                                         //サウンド追加分 7/15
                if (isConnectJoycon) hd.JoyconRumble(1, 160, 320, 0.2f, 50);
                selectStopFlag = true;
            }

            if (choosingModeFlag == true)
            {
                if (selectNum < 3)
                {
                    if (selectNum == 0 && bgmSlider.value > 0)
                    {
                        bgmSlider.value -= 1.0f / bgmSliderDivisionNum;
                        soundManager.Play("Decrease");                                  //サウンド追加分 8/15
                    }
                    else if (selectNum == 1 && seSlider.value > 0)
                    {
                        seSlider.value -= 1.0f / seSliderDivisionNum;
                        soundManager.Play("Decrease");                                  //サウンド追加分 9/15
                    }
                    else if (selectNum == 2 && handleSlider.value > 0)
                    {
                        handleSlider.value -= 1.0f / handleSliderDivisionNum;
                        soundManager.Play("Decrease");                                  //サウンド追加分 10/15
                    }
                    if (isConnectJoycon) hd.JoyconRumble(1, 160, 320, 0.2f, 50);
                                        //第一引数が1で右コントローラー（左手で持つ）、他はSetRumble()の引数と同様
                }
                selectStopFlag = true;
            }
        }


        if (hd.GetRightBrakeDown() == true || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (choosingModeFlag == false)
            {
                if (selectNum < 3)
                {
                    choosingModeFlag = true;
                    Debug.Log("choosin is true");
                }
                if (selectNum > 2 && frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
                //isTransition = true;
                //fc.isFadeOut = true;
                if (selectNum == 3)
                {
                    isAlertStandup = true;
                    soundManager.Play("Alart");                                             //サウンド追加分 2/15
                    configAlertNoPanel.SetActive(!configAlertNoPanel.activeInHierarchy);

                }
                if (selectNum == 4)
                {
                    isAlertStandup = true;
                    soundManager.Play("Alart");                                             //サウンド追加分 3/15
                    configAlertYesPanel.SetActive(!configAlertYesPanel.activeInHierarchy);

                }
                if (isConnectJoycon)
                {
                    hd.JoyconRumble(0, 160, 320, 0.2f, 100);
                    hd.JoyconRumble(1, 160, 320, 0.2f, 100);//第一引数が1で右コントローラー、他はSetRumble()の引数と同様
                }
            }
            else
            {
                choosingModeFlag = false;
                if (frames[selectNum].activeInHierarchy == false) frames[selectNum].SetActive(true);
                if (isConnectJoycon)
                {
                    hd.JoyconRumble(0, 160, 320, 0.2f, 100);
                    hd.JoyconRumble(1, 160, 320, 0.2f, 100);//第一引数が1で右コントローラー、他はSetRumble()の引数と同様
                }
            }

            if (isAlertStandup == true && alertStandingFlag == true)
            {
                isTransition = true;
                //Debug.Log("alertDisplaySpaceKey");
                isAlertStandup = false;
                alertStandingFlag = false;
                if (isConnectJoycon)
                {
                    hd.JoyconRumble(0, 160, 320, 0.2f, 100);
                    hd.JoyconRumble(1, 160, 320, 0.2f, 100);//第一引数が1で右コントローラー、他はSetRumble()の引数と同様
                }
            }

            
            
        }

        if (hd.GetLeftBrakeDown() == true || Input.GetKeyUp(KeyCode.Backspace))
        {
            if (choosingModeFlag == true)
            {
                switch (selectNum)//本当は保存しといたその時々のデフォルト値を0.5fの所に入れる
                {
                    case 0:
                        bgmSlider.value = 0.5f;
                        break;
                    case 1:
                        seSlider.value = 0.5f;
                        break;
                    case 2:
                        handleSlider.value = 0.5f;
                        break;
                    default:
                        break;
                }
                choosingModeFlag = false;
                if (frames[selectNum].activeInHierarchy == false) frames[selectNum].SetActive(true);
                if (isConnectJoycon)
                {
                    hd.JoyconRumble(0, 160, 320, 0.2f, 100);
                    hd.JoyconRumble(1, 160, 320, 0.2f, 100);//第一引数が1で右コントローラー、他はSetRumble()の引数と同様
                }
            }
            if (isAlertStandup == true && alertStandingFlag == true)
            {
                if (configAlertYesPanel.activeInHierarchy) configAlertYesPanel.SetActive(!configAlertYesPanel.activeInHierarchy);
                else if (configAlertNoPanel.activeInHierarchy) configAlertNoPanel.SetActive(!configAlertNoPanel.activeInHierarchy);
                isAlertStandup = false;
                alertStandingFlag = false;
                if (frames[selectNum].activeInHierarchy == false) frames[selectNum].SetActive(true);
                if (isConnectJoycon)
                {
                    hd.JoyconRumble(0, 160, 320, 0.2f, 100);
                    hd.JoyconRumble(1, 160, 320, 0.2f, 100);//第一引数が1で右コントローラー、他はSetRumble()の引数と同様
                }
            }
        }


        if (isTransition == true/* && fc.isFadeOut == false*/ && isAlertStandup == false)
        {
            isTransition = false;
            switch (selectNum)
            {
                case 0:
                case 1:
                case 2:
                    if (frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
                    selectNum++;
                    break;
                case 3:
                    if (isIngame == true)
                    {
                        //Pauseから来た時戻る処理書く
                    }
                    else
                    {
                        if (configAlertYesPanel.activeInHierarchy) configAlertYesPanel.SetActive(!configAlertYesPanel.activeInHierarchy);
                        else if (configAlertNoPanel.activeInHierarchy) configAlertNoPanel.SetActive(!configAlertNoPanel.activeInHierarchy);

                        if (frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);

                        //モード選択から来た時戻る処理書く
                        soundManager.Play("MenuBack");                              //サウンド追加分 4/15
                        configCanvas.SetActive(!configCanvas.activeInHierarchy);
                        returnCanvas.SetActive(!returnCanvas.activeInHierarchy);
                    }
                    //if (frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
                    selectNum = 0;
                    if (frames[selectNum].activeInHierarchy == false) frames[selectNum].SetActive(true);
                    break;
                case 4:
                    //スライダーで変更した値を反映させる処理書く

                    if (isIngame == true)
                    {
                        //Pauseから来た時戻る処理書く
                    }
                    else
                    {
                        if (configAlertYesPanel.activeInHierarchy) configAlertYesPanel.SetActive(!configAlertYesPanel.activeInHierarchy);
                        else if (configAlertNoPanel.activeInHierarchy) configAlertNoPanel.SetActive(!configAlertNoPanel.activeInHierarchy);

                        if (frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);

                        //モード選択から来た時戻る処理書く
                        soundManager.Play("MenuBack");                              //サウンド追加分 5/15
                        configCanvas.SetActive(!configCanvas.activeInHierarchy);
                        returnCanvas.SetActive(!returnCanvas.activeInHierarchy);
                    }

                    selectNum = 0;
                    if (frames[selectNum].activeInHierarchy == false) frames[selectNum].SetActive(true);
                    break;
                default:
                    break;
            }
        }












        /*

        if (  
            (hd.GetRightBrake() == true || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.KeypadEnter))
            && choosingModeFlag == false
            )
        {
            if (selectNum < 3)
            {
                choosingModeFlag = true;
            }
            if (selectNum > 2 && frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
            //isTransition = true;
            //fc.isFadeOut = true;
            if (selectNum == 3)
            {
                isAlertStandup = true;
                soundManager.Play("Alart");                                             //サウンド追加分 2/15
                configAlertNoPanel.SetActive(!configAlertNoPanel.activeInHierarchy);
                
            }
            if (selectNum == 4)
            {
                isAlertStandup = true;
                soundManager.Play("Alart");                                             //サウンド追加分 3/15
                configAlertYesPanel.SetActive(!configAlertYesPanel.activeInHierarchy);
                
            }
        }

        if (isAlertStandup)
        {
            if (Input.GetKeyDown(KeyCode.Space) || hd.GetRightBrake() == true)
            {
                isTransition = true;
                //Debug.Log("alertDisplaySpaceKey");
                isAlertStandup = false;
            }
            if (Input.GetKeyDown(KeyCode.Backspace) || hd.GetLeftBrake() == true)
            {
                if (configAlertYesPanel.activeInHierarchy) configAlertYesPanel.SetActive(!configAlertYesPanel.activeInHierarchy);
                else if (configAlertNoPanel.activeInHierarchy) configAlertNoPanel.SetActive(!configAlertNoPanel.activeInHierarchy);
                isAlertStandup = false;
                if (frames[selectNum].activeInHierarchy == false) frames[selectNum].SetActive(true);

            }
        }


        

        if ((Input.GetKeyDown(KeyCode.RightArrow) ||
            (hd.GetControlllerAccel(1) < -katamukiNum && selectStopFlag == false)
            )  &&  choosingModeFlag == false)
        {
            if (frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
            selectNum++;
            if (selectNum > 4) selectNum = 0;
            if (frames[selectNum].activeInHierarchy == false) frames[selectNum].SetActive(true);
            soundManager.Play("Select");                                         //サウンド追加分 6/15
            if (isConnectJoycon)
            {
                hd.JoyconRumble(0, 160, 320, 0.1f, 100);//第一引数が0で左コントローラー、他はSetRumble()の引数と同様
                hd.JoyconRumble(1, 160, 320, 0.1f, 100);
            }
            selectStopFlag = true;
        }

        if ((Input.GetKeyDown(KeyCode.LeftArrow) ||
            (hd.GetControlllerAccel(1) > katamukiNum && selectStopFlag == false)
            )  &&  choosingModeFlag == false)
        {
            if (frames[selectNum].activeInHierarchy == true) frames[selectNum].SetActive(false);
            selectNum--;
            if (selectNum < 0) selectNum = 4;
            if (frames[selectNum].activeInHierarchy == false) frames[selectNum].SetActive(true);
            soundManager.Play("Select");                                         //サウンド追加分 7/15
            hd.JoyconRumble(0, 160, 320, 0.2f, 100);
            hd.JoyconRumble(1, 160, 320, 0.2f, 100);//第一引数が1で右コントローラー、他はSetRumble()の引数と同様
            selectStopFlag = true;
        }




        if ((Input.GetKeyDown(KeyCode.LeftArrow) ||
            (hd.GetControlllerAccel(1) > katamukiNum && selectStopFlag == false)) 
            && choosingModeFlag == true)
        {
            if (selectNum < 3)
            {
                if (selectNum == 0 && bgmSlider.value > 0)
                {
                    bgmSlider.value -= 1.0f / bgmSliderDivisionNum;
                    soundManager.Play("Decrease");                                  //サウンド追加分 8/15
                }
                else if (selectNum == 1 && seSlider.value > 0)
                {
                    seSlider.value -= 1.0f / seSliderDivisionNum;
                    soundManager.Play("Decrease");                                  //サウンド追加分 9/15
                }
                else if (selectNum == 2 && handleSlider.value > 0)
                {
                    handleSlider.value -= 1.0f / handleSliderDivisionNum;
                    soundManager.Play("Decrease");                                  //サウンド追加分 10/15
                }
                if (isConnectJoycon) hd.JoyconRumble(1, 160, 320, 0.2f, 50);//第一引数が1で右コントローラー、他はSetRumble()の引数と同様
            }
            selectStopFlag = true;
        }

        if ((Input.GetKeyDown(KeyCode.RightArrow) ||
            (hd.GetControlllerAccel(1) < -katamukiNum && selectStopFlag == false)
            )  && choosingModeFlag == true)
        {
            if (selectNum < 3)
            {
                if (selectNum == 0 && bgmSlider.value < 1)
                {
                    soundManager.Play("Increase");                                  //サウンド追加分 12/15
                    bgmSlider.value += 1.0f / bgmSliderDivisionNum;
                }
                if (selectNum == 1 && seSlider.value < 1)
                {
                    soundManager.Play("Increase");                                  //サウンド追加分 13/15
                    seSlider.value += 1.0f / seSliderDivisionNum;
                }
                if (selectNum == 2 && handleSlider.value < 1)
                {
                    soundManager.Play("Increase");                                  //サウンド追加分 14/15
                    handleSlider.value += 1.0f / handleSliderDivisionNum;
                }
                if (isConnectJoycon) hd.JoyconRumble(0, 160, 320, 0.2f, 50);//第一引数が0で左コントローラー、他はSetRumble()の引数と同様
            }
            selectStopFlag = true;
        }


        if (choosingModeFlag)
        {
            if ((hd.GetRightBrake() == true) ||
            Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                choosingModeFlag = false;
            }
            if (hd.GetLeftBrake() == true || Input.GetKeyDown(KeyCode.Backspace))
            {
                switch (selectNum)//本当は保存しといたその時々のデフォルト値を0.5fの所に入れる
                {
                    case 0:
                        bgmSlider.value = 0.5f;
                        break;
                    case 1:
                        seSlider.value = 0.5f;
                        break;
                    case 2:
                        handleSlider.value = 0.5f;
                        break;
                    default:
                        break;
                }
                choosingModeFlag = false;
            }
            

        }
        */


    }

}
