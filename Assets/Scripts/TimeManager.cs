using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public float totalTime; //全体のタイム計測
    /*public GameObject[] secEndPosition;
    float secTime;
    float[] secEndTime;
    private int secNumber;*/

    void Start()
    {
        totalTime = 0f;
        //secNumber = 0;
        //secEndTime = new float[secEndPosition.Length];
    }

    void Update()
    {
        totalTime += Time.deltaTime;
        //Debug.Log("Time:" + totalTime);

        //secTime += Time.deltaTime;
        //Debug.Log("secTime:" + secTime);
    }

}
