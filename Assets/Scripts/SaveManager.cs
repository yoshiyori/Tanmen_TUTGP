﻿using System;
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

    public void PourData(RankingSaveData argSave)
    {
        Array.Copy(argSave.goalTimes, save.goalTimes, argSave.goalTimes.Length);
        Array.Copy(argSave.rankerNames, save.rankerNames, argSave.rankerNames.Length);
    }

    public string[] AbstractionNameData()
    {
        return save.rankerNames;
    }

    public float[] AbstractionTimeData()
    {
        return save.goalTimes;
    }
}
