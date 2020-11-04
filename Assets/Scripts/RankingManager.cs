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
    [SerializeField] private GameObject[] firstRankerSecTimeBox;
    [SerializeField] private GameObject[] secondRankerSecTimeBox;
    [SerializeField] private GameObject[] thirdRankerSecTimeBox;
    private Text[] rankersNameBoxText;
    private Text[] rankersTimeBoxText;
    private Text[] firstRankerSecTimeBoxText;
    private Text[] secondRankerSecTimeBoxText;
    private Text[] thirdRankerSecTimeBoxText;

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

    private bool alphabetSelectStopFlag;
    private float alphabetStopTimer;
    [SerializeField] private float alphabetStopTime;//JoyCon使用時、alphabet選ぶ際、傾けっぱなしにした場合の一文字待機時間

    private float pushStopTimer;
    [SerializeField] private bool pushStopFlag;
    [SerializeField] private float pushStopTime;

    //Save
    string filePath;
    RankingSaveData save;

    [SerializeField] TimeData timedata;

    //サウンド追加分
    [SerializeField] private CuePlayer2D soundManager;

    [SerializeField] private int RankingSceneNumber;

    void Awake()
    {
        if (RankingSceneNumber == 0)
        {
            filePath = Application.dataPath + "/StreamingAssets/SaveData" + "/savedata.json";
        }
        else
        {
            filePath = Application.dataPath + "/StreamingAssets/SaveData" + "/savedata2.json";
        }
        
        save = new RankingSaveData();
        if (System.IO.File.Exists(filePath) == false)
        {
            for (int i = 0; i < 10; i++)
            {
                save.rankerNames[i] = "---";
                save.goalTimes[i] = 0.0f;
                save.arrayLengthNum = 0;
            }
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        for (int j = 0; j < 4; j++)
                        {
                            save.sectionTime1st[j] = 0.0f;
                        }
                        break;
                    case 1:
                        for (int j = 0; j < 4; j++)
                        {
                            save.sectionTime2nd[j] = 0.0f;
                        }
                        break;
                    case 2:
                        for (int j = 0; j < 4; j++)
                        {
                            save.sectionTime3rd[j] = 0.0f;
                        }
                        break;
                    default:
                        break;
                }
            }
            Save(RankingSceneNumber);
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
        firstRankerSecTimeBoxText = new Text[4];
        secondRankerSecTimeBoxText = new Text[4];
        thirdRankerSecTimeBoxText = new Text[4];
        for (int i = 0; i < 10; i++)
        {
            rankersNameBoxText[i] = rankersNameBox[i].GetComponent<Text>();
            rankersTimeBoxText[i] = rankersTimeBox[i].GetComponent<Text>();
        }
        for (int i = 0; i < 4; i++)
        {
            firstRankerSecTimeBoxText[i] = firstRankerSecTimeBox[i].GetComponent<Text>();
        }
        for (int i = 0; i < 4; i++)
        {
            secondRankerSecTimeBoxText[i] = secondRankerSecTimeBox[i].GetComponent<Text>();
        }
        for (int i = 0; i < 4; i++)
        {
            thirdRankerSecTimeBoxText[i] = thirdRankerSecTimeBox[i].GetComponent<Text>();
        }
        isDisplayRanking = false;

        selectNum = 0;
        selectStopFlag = false;
        alphabetSelectStopFlag = false;
        pushStopFlag = false;
        isTransition = false;
        if (stopTime == 0) stopTime = 0.6f;
        if (alphabetStopTime == 0) alphabetStopTime = 0.1f;
        if (pushStopTime == 0) pushStopTime = 1.0f;
        if (katamukiNum == 0) katamukiNum = 0.5f;
        alertStandingFlag = false;

    }


    void Update()
    {
        if (pushStopFlag == true)
        {
            pushStopTimer += Time.deltaTime;
            if (pushStopTimer > pushStopTime)
            {
                pushStopFlag = false;
                pushStopTimer = 0.0f;
            }
        }

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

            if (alertStandingFlag == false && alertPanel.activeInHierarchy == true)
            {
                if (Input.GetKeyUp(KeyCode.Space) || (hd.GetRightBrakeDown() == true && pushStopFlag == false))
                {
                    alertStandingFlag = true;
                }
            }

            if ((hd.GetRightBrakeDown() == true && pushStopFlag == false) || Input.GetKeyDown(KeyCode.Space))
            {
                pushStopFlag = true;
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

            


            if (isTransition == true)
            {
                if (selectNum == 0)
                {
                    isTransition = false;
                    selectNum = 0;
                    GameManeger.goalFlag = false;

                    //サウンド追加分
                    CueManager.singleton.AddTimeStrechVoicePool();
                    if (RankingSceneNumber == 0)
                    {
                        SceneManager.LoadScene("Main");
                    }
                    else
                    {
                        SceneManager.LoadScene("Main2");
                    }
                }
                else
                {
                    isTransition = false;
                    selectNum = 0;
                    GameManeger.moveTitle = true;
                    GameManeger.goalFlag = false;
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

        if (alphabetSelectStopFlag == true)
        {
            alphabetStopTimer += Time.deltaTime;
            if (alphabetStopTimer > stopTime)
            {
                alphabetSelectStopFlag = false;
                alphabetStopTimer = 0.0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) ||
            (hd.GetControlllerAccel(0.2f, 1) < -katamukiNum && alphabetSelectStopFlag == false))
        {
            alphabetSelectStopFlag = true;
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
        if (Input.GetKeyDown(KeyCode.LeftArrow) || (hd.GetControlllerAccel(0.2f, 1) > katamukiNum && alphabetSelectStopFlag == false))
        {
            alphabetSelectStopFlag = true;
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

        if (Input.GetKeyDown(KeyCode.Backspace) || (hd.GetLeftBrakeDown() == true))
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

        if (Input.GetKeyUp(KeyCode.Space) || (hd.GetRightBrakeDown() == true))
        {
            pushStopFlag = true;
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
            for (int i = 0; i < 4; i++)
            {
                save.sectionTime1st[i] = timedata.secTimes[i];
            }
            
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
                    float[] staySectionTimes = new float[4];

                    Array.Copy(rankingPlayerName, stayNames, 10);
                    Array.Copy(rankingPlayerTime, stayNums, 10);


                    rankingPlayerName[i] = userName;
                    rankingPlayerTime[i] = timedata.goalTime;
                    if (i < 3)
                    {
                        switch (i)
                        {
                            case 0:
                                Array.Copy(save.sectionTime1st, staySectionTimes, 4);
                                
                                for (int j = 0; j < 4; j++)
                                {
                                    save.sectionTime1st[j] = timedata.secTimes[j];
                                    save.sectionTime2nd[j] = staySectionTimes[j];
                                }
                                break;
                            case 1:
                                Array.Copy(save.sectionTime2nd, staySectionTimes, 4);
                                for (int j = 0; j < 4; j++)
                                {
                                    save.sectionTime2nd[j] = timedata.secTimes[j];
                                    save.sectionTime3rd[j] = staySectionTimes[j];
                                }
                                break;
                            case 2:
                                for (int j = 0; j < 4; j++)
                                {
                                    save.sectionTime3rd[j] = timedata.secTimes[j];
                                }
                                break;
                            default:
                                break;
                        }
                    }

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
                if (save.arrayLengthNum < 3)
                {
                    switch (save.arrayLengthNum)
                    {
                        case 1:
                            for (int j = 0; j < 4; j++)
                            {
                                save.sectionTime2nd[j] = timedata.secTimes[j];
                            }
                            break;
                        case 2:
                            for (int j = 0; j < 4; j++)
                            {
                                save.sectionTime3rd[j] = timedata.secTimes[j];
                            }
                            break;
                        default:
                            break;
                    }
                }
                save.arrayLengthNum += 1;
            }

            
        }

        Array.Copy(rankingPlayerName, save.rankerNames, 10);
        Array.Copy(rankingPlayerTime, save.goalTimes, 10);

        Save(RankingSceneNumber);
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
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                //int minutes = Mathf.FloorToInt(save.goalTimes[i] / 60f);
                //int seconds;
                int forSectionSeconds;
                int mseconds;
                switch (i)
                {
                    case 0:
                        //seconds = Mathf.FloorToInt(save.sectionTime1st[j] % 60f);
                        forSectionSeconds = Mathf.FloorToInt(save.sectionTime1st[j]);
                        mseconds = Mathf.FloorToInt((save.sectionTime1st[j] - forSectionSeconds) * 1000);
                        firstRankerSecTimeBoxText[j].text = string.Format("{2}/ {0:00}'{1:000}", forSectionSeconds, mseconds, j + 1);
                        break;
                    case 1:
                        //seconds = Mathf.FloorToInt(save.sectionTime2nd[j] % 60f);
                        forSectionSeconds = Mathf.FloorToInt(save.sectionTime2nd[j]);
                        mseconds = Mathf.FloorToInt((save.sectionTime2nd[j] - forSectionSeconds) * 1000);
                        secondRankerSecTimeBoxText[j].text = string.Format("{2}/ {0:00}'{1:000}", forSectionSeconds, mseconds, j + 1);
                        break;
                    case 2:
                        //seconds = Mathf.FloorToInt(save.sectionTime3rd[j] % 60f);
                        forSectionSeconds = Mathf.FloorToInt(save.sectionTime3rd[j]);
                        mseconds = Mathf.FloorToInt((save.sectionTime3rd[j] - forSectionSeconds) * 1000);
                        thirdRankerSecTimeBoxText[j].text = string.Format("{2}/ {0:00}'{1:000}", forSectionSeconds, mseconds, j + 1);
                        break;
                    default:
                        break;
                }
            }
        }

    }

    public void Save(int rankingNum)
    {
        string json = JsonUtility.ToJson(save);
        StreamWriter streamWriter;
        if (rankingNum == 0)
        {
            streamWriter = new StreamWriter(Application.dataPath + "/StreamingAssets/SaveData" + "/savedata.json", false);
        }
        else
        {
            streamWriter = new StreamWriter(Application.dataPath + "/StreamingAssets/SaveData" + "/savedata2.json", false);
        }
        
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
