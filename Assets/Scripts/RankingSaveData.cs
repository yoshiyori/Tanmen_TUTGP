using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable] 
public class RankingSaveData {
    public string[] rankerNames = new string[10];
    public float[] goalTimes = new float[10];
    public int arrayLengthNum;
    public float[] sectionTime1st = new float[4];
    public float[] sectionTime2nd = new float[4];
    public float[] sectionTime3rd = new float[4];

}
