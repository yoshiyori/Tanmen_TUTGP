using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;

public class RankingManager : MonoBehaviour
{
    [SerializeField] SaveManager sm;
    RankingSaveData rSave;
    private int wordSelectNum;
    private int allWordPanel;

    private string[] rankingPlayerName;
    private float[] rankingPlayerTime;

    [SerializeField] private GameObject selectPanel;
    [SerializeField] private GameObject alphabets;
    [SerializeField] private GameObject userNameWordBox;
    [SerializeField] private GameObject InputDataUIPanel;

    private RectTransform alphabetsRectTrans;
    private RectTransform selectPanelRectTrans;
    private Text userNamewordBoxText;

    [SerializeField] private GameObject[] rankersNameBox;
    [SerializeField] private GameObject[] rankersTimeBox;
    private Text[] rankersNameBoxText;
    private Text[] rankersTimeBoxText;

    private char[] thirdWordsName;
    private string userName;
    private int wordRemainingTime;

    private bool isfinishEnterWord;
    private int betweenRanksNum;

    private bool isDisplayRanking;

    private float flashTimer;

    [SerializeField] private GameObject alertPanel;
    [SerializeField] private GameObject[] selectFrames;

    private int selectNum;
    private float time = 0f;
    private float stopTimer;
    [SerializeField] Handle hd;
    private bool selectStopFlag;
    [SerializeField] private float stopTime;
    private bool isTransition;
    [SerializeField] private float katamukiNum;
    private bool alertStandingFlag;

    //Save
    string filePath;
    RankingSaveData save;

    [SerializeField] TimeData timedata;

    //サウンド追加分
    [SerializeField] private CuePlayer2D soundManager;

    void Awake()
    {
        filePath = Application.dataPath + "/StreamingAssets/SaveData" + "/savedata.json";
        
        save = new RankingSaveData();
        if (System.IO.File.Exists(filePath) == false)
        {
            for (int i = 0; i < 10; i++)
            {
                save.rankerNames[i] = "---";
                save.goalTimes[i] = 0.0f;
                save.arrayLengthNum = 0;
            }
            Save();
            //Debug.Log("MakeJsonFile");
        }
        //Save();   //コメントアウト外して実行するとJsonDataまっさら初期状態になる。
    }
    void Start()
    {
        rSave = new RankingSaveData();
        alphabetsRectTrans = alphabets.GetComponent<RectTransform>();
        selectPanelRectTrans = selectPanel.GetComponent<RectTransform>();
        userNamewordBoxText = userNameWordBox.GetComponent<Text>();
        wordSelectNum = 65;//A = 65, Z = 90
        thirdWordsName = new char[3];
        isfinishEnterWord = false;
        betweenRanksNum = -1;
        allWordPanel = 0;

        rankingPlayerName = new string[10];
        rankingPlayerTime = new float[10];

        rankersNameBoxText = new Text[10];
        rankersTimeBoxText = new Text[10];
        for (int i = 0; i < 10; i++)
        {
            rankersNameBoxText[i] = rankersNameBox[i].GetComponent<Text>();
            rankersTimeBoxText[i] = rankersTimeBox[i].GetComponent<Text>();
        }
        isDisplayRanking = false;

        selectNum = 0;
        selectStopFlag = false;
        isTransition = false;
        if (stopTime == 0) stopTime = 0.6f;
        if (katamukiNum == 0) katamukiNum = 0.5f;
        alertStandingFlag = false;

    }


    void Update()
    {
        

        if (isfinishEnterWord == false && isDisplayRanking == false) AlpabetMove();

        if (isfinishEnterWord == true) SaveRankerData();

        DisplayRanking();

        if (isDisplayRanking == true)
        {
            time += Time.deltaTime;

            if (time > 0.5f)
            {
                time = 0.0f;
                selectFrames[selectNum].SetActive(!selectFrames[selectNum].activeInHierarchy);
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

            if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            (hd.GetControlllerAccel(0.2f, 1) > katamukiNum && selectStopFlag == false) && isTransition == false
            )
            {
                if (selectNum > 0)
                {
                    if (selectFrames[selectNum].activeInHierarchy == true) selectFrames[selectNum].SetActive(false);
                    selectNum--;
                    hd.JoyconRumble(1, 160, 320, 0.3f, 100);//第一引数が1で右コントローラー、他はSetRumble()の引数と同様

                    //サウンド追加分
                    soundManager.Play("Select");
                }
                selectStopFlag = true;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) ||
                (hd.GetControlllerAccel(0.2f, 1) < -katamukiNum && selectStopFlag == false) && isTransition == false
                )
            {
                if ((selectNum < selectFrames.Length - 1 && selectNum >= 0))
                {
                    if (selectFrames[selectNum].activeInHierarchy == true) selectFrames[selectNum].SetActive(false);
                    selectNum++;
                    hd.JoyconRumble(0, 160, 320, 0.3f, 100);//第一引数が0で左コントローラー、他はSetRumble()の引数と同様

                    //サウンド追加分
                    soundManager.Play("Select");
                }
                selectStopFlag = true;
            }



            if ((hd.GetRightBrakeDown() == true) || Input.GetKeyDown(KeyCode.Space))
            {
                if (alertStandingFlag == true && isTransition == false)
                {
                    isTransition = true;
                }
                if (alertStandingFlag == false)
                {
                    if (alertPanel.activeInHierarchy == false)
                    {
                        alertPanel.SetActive(true);

                        //サウンド追加分
                        soundManager.Play("Alart");
                    }
                }
            }

            if (alertStandingFlag == false && alertPanel.activeInHierarchy == true)
            {
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    alertStandingFlag = true;
                }
            }


            if (isTransition == true)
            {
                if (selectNum == 0)
                {
                    isTransition = false;
                    selectNum = 0;
                    SceneManager.LoadScene("Main");
                }
                else
                {
                    isTransition = false;
                    selectNum = 0;
                    GameManeger.moveTitle = true;
                    SceneManager.LoadSceneAsync("CourceSelect");
                }
            }
        }
    }

    void AlpabetMove()
    {
        flashTimer += Time.deltaTime;
        if (flashTimer > 0.3f)
        {
            flashTimer = 0.0f;
            selectPanel.SetActive(!selectPanel.activeInHierarchy);
        }
       
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if ((allWordPanel == 0 && wordSelectNum >= 72) || 
                allWordPanel > 0 && allWordPanel < 10
                )
            {
                alphabetsRectTrans.localPosition += new Vector3(-50.0f, 0.0f, 0.0f);
                wordSelectNum++;
                allWordPanel++;
            }
            else if (wordSelectNum >= 90)
            {
                wordSelectNum = 65;
                allWordPanel = 0;
                alphabetsRectTrans.localPosition += new Vector3(50.0f * 10, 0.0f, 0.0f);
                selectPanelRectTrans.localPosition += new Vector3(-50.0f * 15, 0.0f, 0.0f);
            }
            else
            {
                selectPanelRectTrans.localPosition += new Vector3(50.0f, 0.0f, 0.0f);
                wordSelectNum++;
            }

            //サウンド追加分
            soundManager.Play("Select");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if ((allWordPanel == 10 && wordSelectNum <= 82) ||
                 allWordPanel < 10 && allWordPanel > 0
                )
            {
                alphabetsRectTrans.localPosition += new Vector3(50.0f, 0.0f, 0.0f);
                wordSelectNum--;
                allWordPanel--;
            }
            else if (wordSelectNum <= 65)
            {
                wordSelectNum = 90;
                allWordPanel = 10;
                alphabetsRectTrans.localPosition += new Vector3(-50.0f * 10, 0.0f, 0.0f);
                selectPanelRectTrans.localPosition += new Vector3(50.0f * 15, 0.0f, 0.0f);
            }
            else
            {
                selectPanelRectTrans.localPosition += new Vector3(-50.0f, 0.0f, 0.0f);
                wordSelectNum--;
            }

            //サウンド追加分
            soundManager.Play("Select");
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {

            if (wordRemainingTime > 0)
            {
                char[] kariokiArray = new char[wordRemainingTime - 1];
                Array.Copy(thirdWordsName, kariokiArray, wordRemainingTime - 1);
                thirdWordsName = new char[3];
                Array.Copy(kariokiArray, thirdWordsName, wordRemainingTime - 1);
                wordRemainingTime--;
                userNamewordBoxText.text = new string(thirdWordsName);

                //サウンド追加分
                soundManager.Play("MenuBack");
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            thirdWordsName[wordRemainingTime] = (char)wordSelectNum;
            userName = new string(thirdWordsName);
            wordRemainingTime++;
            userNamewordBoxText.text = userName;

            //サウンド追加分
            soundManager.Play("Decision");

            if (wordRemainingTime >= 3)
            {
                isfinishEnterWord = true;
            }
        }

    }

    void SaveRankerData()
    {
        bool isAdd = false; 
        Load();
        if (save.arrayLengthNum == 0)
        {
            rankingPlayerName[0] = userName;
            rankingPlayerTime[0] = timedata.goalTime;
            save.arrayLengthNum += 1;
        }
        else
        {
            for (int i = 0; i < save.arrayLengthNum; i++)
            {
                if (rankingPlayerTime[i] > timedata.goalTime)
                {
                    float[] stayNums = new float[10];
                    string[] stayNames = new string[10];

                    Array.Copy(rankingPlayerName, stayNames, 10);
                    Array.Copy(rankingPlayerTime, stayNums, 10);


                    rankingPlayerName[i] = userName;
                    rankingPlayerTime[i] = timedata.goalTime;

                    Array.Copy(stayNames, i, rankingPlayerName, i + 1, 10 - (i + 1));
                    Array.Copy(stayNums, i, rankingPlayerTime, i + 1, 10 - (i + 1));
                    isAdd = true;
                    if (save.arrayLengthNum < 10) save.arrayLengthNum += 1;
                    break;
                }
            }
            if (isAdd == false && save.arrayLengthNum < 10)
            {
                rankingPlayerName[save.arrayLengthNum] = userName;
                rankingPlayerTime[save.arrayLengthNum] = timedata.goalTime;
                save.arrayLengthNum += 1;
            }

            
        }

        Array.Copy(rankingPlayerName, save.rankerNames, 10);
        Array.Copy(rankingPlayerTime, save.goalTimes, 10);

        Save();
        InputDataUIPanel.SetActive(!InputDataUIPanel.activeInHierarchy);
        isfinishEnterWord = false;
        isDisplayRanking = true;
    }



    void DisplayRanking()
    {
        Load();
        for (int i = 0; i < save.arrayLengthNum; i++)
        {
            rankersNameBoxText[i].text = save.rankerNames[i];
            int minutes = Mathf.FloorToInt(save.goalTimes[i] / 60f);
            int seconds = Mathf.FloorToInt(save.goalTimes[i] % 60f);
            int mseconds = Mathf.FloorToInt((save.goalTimes[i] % 60f - seconds) * 1000);
            rankersTimeBoxText[i].text = string.Format("{0:00}.{1:00}'{2:000}", minutes, seconds, mseconds);
        }

    }

    public void Save()
    {
        string json = JsonUtility.ToJson(save);

        StreamWriter streamWriter = new StreamWriter(Application.dataPath + "/StreamingAssets/SaveData" + "/savedata.json", false);
        streamWriter.Write(json);
        streamWriter.Flush();
        streamWriter.Close();
    }

    public void Load()
    {
        if (File.Exists(filePath))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            save = JsonUtility.FromJson<RankingSaveData>(data);


            Array.Copy(save.rankerNames, rankingPlayerName, 10);
            Array.Copy(save.goalTimes, rankingPlayerTime, 10);

        }
    }
}
