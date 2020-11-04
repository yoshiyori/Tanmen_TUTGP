using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;

public class TitleRankingManager : MonoBehaviour
{

    private string[] rankingPlayerName;
    private float[] rankingPlayerTime;


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

    private string[] rankingPlayerName2;
    private float[] rankingPlayerTime2;


    [SerializeField] private GameObject[] rankersNameBox2;
    [SerializeField] private GameObject[] rankersTimeBox2;
    [SerializeField] private GameObject[] firstRankerSecTimeBox2;
    [SerializeField] private GameObject[] secondRankerSecTimeBox2;
    [SerializeField] private GameObject[] thirdRankerSecTimeBox2;
    private Text[] rankersNameBoxText2;
    private Text[] rankersTimeBoxText2;
    private Text[] firstRankerSecTimeBoxText2;
    private Text[] secondRankerSecTimeBoxText2;
    private Text[] thirdRankerSecTimeBoxText2;


    [SerializeField] Handle hd;
    [SerializeField] private float stopTime;
    private bool isTransition;
    [SerializeField] private float katamukiNum;

    [SerializeField] private GameObject titleCanvas;
    [SerializeField] private GameObject rankingCanvas;

    private int nowRankingNum;
    private float time;
    private bool selectStopFlag;

    [SerializeField] Text TitleBox;

    //Save
    string filePath;
    string filePath2;
    RankingSaveData save;
    RankingSaveData save2;


    void Awake()
    {
        filePath = Application.dataPath + "/StreamingAssets/SaveData" + "/savedata.json";
        filePath2 = Application.dataPath + "/StreamingAssets/SaveData" + "/savedata2.json";

        save = new RankingSaveData();
        save2 = new RankingSaveData();
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
            Save();
            //Debug.Log("MakeJsonFile");
        }
        if (System.IO.File.Exists(filePath2) == false)
        {
            for (int i = 0; i < 10; i++)
            {
                save2.rankerNames[i] = "---";
                save2.goalTimes[i] = 0.0f;
                save2.arrayLengthNum = 0;
            }
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        for (int j = 0; j < 4; j++)
                        {
                            save2.sectionTime1st[j] = 0.0f;
                        }
                        break;
                    case 1:
                        for (int j = 0; j < 4; j++)
                        {
                            save2.sectionTime2nd[j] = 0.0f;
                        }
                        break;
                    case 2:
                        for (int j = 0; j < 4; j++)
                        {
                            save2.sectionTime3rd[j] = 0.0f;
                        }
                        break;
                    default:
                        break;
                }
            }
            Save2();
            //Debug.Log("MakeJsonFile");
        }
        //Save();   //コメントアウト外して実行するとJsonDataまっさら初期状態になる。
    }
    void Start()
    {

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

        isTransition = false;
        if (stopTime == 0) stopTime = 0.8f;
        if (katamukiNum == 0) katamukiNum = 0.5f;

        TitleBox.text = "コース1:ランキング";

        nowRankingNum = 0;

        Load();

        rankingPlayerName2 = new string[10];
        rankingPlayerTime2 = new float[10];

        Load2();

    }


    void Update()
    {
        if (nowRankingNum == 0)
        {
            DisplayRanking();
            TitleBox.text = "コース1:ランキング";
        }
        else
        {
            DisplayRanking2();
            TitleBox.text = "コース2:ランキング";
        }

        if (selectStopFlag == true)
        {
            time += Time.deltaTime;
            if (time > stopTime)
            {
                selectStopFlag = false;
                time = 0.0f;
            }
        }


        if (((Input.GetKeyDown(KeyCode.LeftArrow) ||
                (hd.GetControlllerAccel(0.2f, 1) > katamukiNum && selectStopFlag == false)
                ) ||
             (Input.GetKeyDown(KeyCode.RightArrow) ||
                (hd.GetControlllerAccel(0.2f, 1) < -katamukiNum && selectStopFlag == false)
                ))
           )
        {
            selectStopFlag = true;
            if (nowRankingNum == 0)
            {
                nowRankingNum = 1;
            }
            else
            {
                nowRankingNum = 0;
            }
        }

        if ((hd.GetRightBrakeDown() == true) || (hd.GetLeftBrakeDown() == true) || Input.GetKeyDown(KeyCode.Space))
        {
            titleCanvas.SetActive(!titleCanvas.activeInHierarchy);
            rankingCanvas.SetActive(!rankingCanvas.activeInHierarchy);
        }

    }


    void DisplayRanking()
    {
        for (int i = 0; i < save.arrayLengthNum; i++)
        {
            Debug.Log(save.rankerNames[i]);
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

    void DisplayRanking2()
    {
        for (int i = 0; i < save2.arrayLengthNum; i++)
        {
            rankersNameBoxText[i].text = save2.rankerNames[i];
            int minutes = Mathf.FloorToInt(save2.goalTimes[i] / 60f);
            int seconds = Mathf.FloorToInt(save2.goalTimes[i] % 60f);
            int mseconds = Mathf.FloorToInt((save2.goalTimes[i] % 60f - seconds) * 1000);
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
                        forSectionSeconds = Mathf.FloorToInt(save2.sectionTime1st[j]);
                        mseconds = Mathf.FloorToInt((save2.sectionTime1st[j] - forSectionSeconds) * 1000);
                        firstRankerSecTimeBoxText[j].text = string.Format("{2}/ {0:00}'{1:000}", forSectionSeconds, mseconds, j + 1);
                        break;
                    case 1:
                        //seconds = Mathf.FloorToInt(save.sectionTime2nd[j] % 60f);
                        forSectionSeconds = Mathf.FloorToInt(save2.sectionTime2nd[j]);
                        mseconds = Mathf.FloorToInt((save2.sectionTime2nd[j] - forSectionSeconds) * 1000);
                        secondRankerSecTimeBoxText[j].text = string.Format("{2}/ {0:00}'{1:000}", forSectionSeconds, mseconds, j + 1);
                        break;
                    case 2:
                        //seconds = Mathf.FloorToInt(save.sectionTime3rd[j] % 60f);
                        forSectionSeconds = Mathf.FloorToInt(save2.sectionTime3rd[j]);
                        mseconds = Mathf.FloorToInt((save2.sectionTime3rd[j] - forSectionSeconds) * 1000);
                        thirdRankerSecTimeBoxText[j].text = string.Format("{2}/ {0:00}'{1:000}", forSectionSeconds, mseconds, j + 1);
                        break;
                    default:
                        break;
                }
            }
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

    public void Save2()
    {
        string json = JsonUtility.ToJson(save2);

        StreamWriter streamWriter = new StreamWriter(Application.dataPath + "/StreamingAssets/SaveData" + "/savedata2.json", false);
        streamWriter.Write(json);
        streamWriter.Flush();
        streamWriter.Close();
    }

    public void Load()
    {
        if (File.Exists(filePath))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(Application.dataPath + "/StreamingAssets/SaveData" + "/savedata.json");
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            save = JsonUtility.FromJson<RankingSaveData>(data);


            Array.Copy(save.rankerNames, rankingPlayerName, 10);
            Array.Copy(save.goalTimes, rankingPlayerTime, 10);

        }
    }

    public void Load2()
    {
        if (File.Exists(filePath2))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(Application.dataPath + "/StreamingAssets/SaveData" + "/savedata2.json");
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            save2 = JsonUtility.FromJson<RankingSaveData>(data);


            Array.Copy(save2.rankerNames, rankingPlayerName2, 10);
            Array.Copy(save2.goalTimes, rankingPlayerTime2, 10);

        }
    }
}
