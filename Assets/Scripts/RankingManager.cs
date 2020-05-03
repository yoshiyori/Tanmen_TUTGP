using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;

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

    }


    void Update()
    {
        AlpabetMove();

        if (isfinishEnterWord == true) SaveRankerData();

        if (isDisplayRanking == true)
        {
            DisplayRanking();
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
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log((char)wordSelectNum);
            thirdWordsName[wordRemainingTime] = (char)wordSelectNum;
            userName = new string(thirdWordsName);
            //Debug.Log(userName);
            wordRemainingTime++;
            userNamewordBoxText.text = userName;

            if (wordRemainingTime >= 3)
            {
                isfinishEnterWord = true;
            }
        }

    }

    void SaveRankerData()
    {
        sm.Load();
        rSave.arrayLengthNum = sm.AbstractionArrayLengthNum();
        if (rSave.arrayLengthNum == 0)
        {
            rankingPlayerName[0] = userName;
            rankingPlayerTime[0] = UnityEngine.Random.Range(300.0f, 600.0f);//まだタイムないので仮置き
                                                                            //save.goalTimes[0] = Random.Range(5, 10).ToString("00") + "."
                                                                            //    + Random.Range(0, 99).ToString("00") + "'" 
                                                                            //    + Random.Range(1,999).ToString("000");
                                                                            //save.rankerNames[0] = userName;
                                                                            //save.goalTimes[0] = UnityEngine.Random.Range(300.0f, 600.0f);//まだタイムないので仮置き
                                                                            //save.goalTimes[0] = Random.Range(5, 10).ToString("00") + "."
                                                                            //    + Random.Range(0, 99).ToString("00") + "'" 
                                                                            //    + Random.Range(1,999).ToString("000");
            
        }
        else
        {
            float goalTimeKari = UnityEngine.Random.Range(300.0f, 600.0f);//本当ならここにそのときのTime入れる
            for (int i = 0; i < rSave.arrayLengthNum; i++)
            {
                if (rankingPlayerTime[i] > goalTimeKari)
                {
                    betweenRanksNum = i;
                    break;
                }
                if (betweenRanksNum > -1)
                {
                    InsertGoalTimesArray(rankingPlayerTime, goalTimeKari, betweenRanksNum);
                    InsertRankerNamesArray(rankingPlayerName, userName, betweenRanksNum);
                }
            }

            
        }
        sm.PourData(rankingPlayerName, rankingPlayerTime, 10);
        sm.Save();
        InputDataUIPanel.SetActive(!InputDataUIPanel.activeInHierarchy);
        isfinishEnterWord = false;
        isDisplayRanking = true;
    }

    string[] InsertRankerNamesArray(string[] argArray, string InsertName, int insertNum)
    {
        string[] a = new string[argArray.Length];

        for (int i = 0; i < argArray.Length; i++)
        {
            if (i == insertNum)
            {
                a[i] = InsertName;
            }
            else if (i > insertNum)
            {
                a[i] = argArray[i - 1];
            }
            else
            {
                a[i] = argArray[i];
            }
        }

        return a;
    }

    float[] InsertGoalTimesArray(float[] argArray, float InsertTime, int insertNum)
    {
        float[] a = new float[10];

        for (int i = 0; i < argArray.Length; i++)
        {
            if (i == insertNum)
            {
                a[i] = InsertTime;
            }
            else if (i > insertNum)
            {
                a[i] = argArray[i - 1];
            }
            else
            {
                a[i] = argArray[i];
            }
        }

        return a;
    }

    void DisplayRanking()
    {
        sm.Load();
        sm.AbstractionNameData().CopyTo(rSave.rankerNames, sm.AbstractionArrayLengthNum());
        sm.AbstractionTimeData().CopyTo(rSave.goalTimes, sm.AbstractionArrayLengthNum());
        rSave.arrayLengthNum = sm.AbstractionArrayLengthNum();

        for (int i = 0; i < rSave.arrayLengthNum; i++)
        {
            rankersNameBoxText[i].text = rSave.rankerNames[i];
            rankersTimeBoxText[i].text = (rSave.goalTimes[i] / 60).ToString("00") + "," + (rSave.goalTimes[i] % 60).ToString("00") + "'" + "000";
        }
        if (rSave.rankerNames.Length < 10)
        {
            for (int i = 0; i < 10 - rSave.arrayLengthNum; i++)
            {
                rankersNameBoxText[i + rSave.arrayLengthNum].text = "---";
                rankersTimeBoxText[i + rSave.arrayLengthNum].text = "00.00'000";
            }
        }
    }


}
