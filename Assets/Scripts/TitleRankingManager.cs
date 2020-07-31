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
    private Text[] rankersNameBoxText;
    private Text[] rankersTimeBoxText;


    [SerializeField] Handle hd;
    [SerializeField] private float stopTime;
    private bool isTransition;
    [SerializeField] private float katamukiNum;

    [SerializeField] private GameObject titleCanvas;
    [SerializeField] private GameObject rankingCanvas;

    //Save
    string filePath;
    RankingSaveData save;


    void Awake()
    {
        filePath = Application.dataPath + "/SaveData" + "/savedata.json";

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

        rankingPlayerName = new string[10];
        rankingPlayerTime = new float[10];

        rankersNameBoxText = new Text[10];
        rankersTimeBoxText = new Text[10];
        for (int i = 0; i < 10; i++)
        {
            rankersNameBoxText[i] = rankersNameBox[i].GetComponent<Text>();
            rankersTimeBoxText[i] = rankersTimeBox[i].GetComponent<Text>();
        }

        isTransition = false;
        if (stopTime == 0) stopTime = 0.6f;
        if (katamukiNum == 0) katamukiNum = 0.5f;

        Load();

    }


    void Update()
    {
        DisplayRanking();
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

        StreamWriter streamWriter = new StreamWriter(Application.dataPath + "/SaveData" + "/savedata.json", false);
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
