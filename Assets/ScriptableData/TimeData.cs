using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TimeData : ScriptableObject
{
    public float goalTime;
    public float[] secTimes = new float[4];
}
