using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    string filePath;
    RankingSaveData save;

    void Awake()
    {
        filePath = "Assets/SaveData" + "/" + ".rankingsavedata.json";
        save = new RankingSaveData();
        Save();
        /*
        for (int i = 0; i < 10; i++)
        {
            save.rankerNames[i] = new string;
        }
        */
    }

    void Update()
    {

    }

    public void Save()
    {
        string json = JsonUtility.ToJson(save);

        StreamWriter streamWriter = new StreamWriter(filePath);
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
        }
    }

    public void PourData(string[] argNameArray, float[] argTimeArray, int argArrayLengthNum)
    {
        argNameArray.CopyTo(save.rankerNames, 10);
        argTimeArray.CopyTo(save.goalTimes, 10);
        save.arrayLengthNum = argArrayLengthNum;

    }

    public string[] AbstractionNameData()
    {
        return save.rankerNames;
    }

    public float[] AbstractionTimeData()
    {
        return save.goalTimes;
    }

    public int AbstractionArrayLengthNum()
    {
        return save.arrayLengthNum;
    }
}
