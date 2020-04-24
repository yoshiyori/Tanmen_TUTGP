using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    float totalTime;
    public GameObject[] secEndPosition;
    float secTime;
    float[] secEndTime;
    private int secNumber;

    void Start()
    {
        totalTime = 0f;
        secNumber = 0;
        secEndTime = new float[secEndPosition.Length];
    }

    void Update()
    {
        totalTime += Time.deltaTime;
        //Debug.Log("Time:" + totalTime);

        secTime += Time.deltaTime;
        //Debug.Log("secTime:" + secTime);
    }

    void OnTriggerEnter(Collider other)
    {

        for(int i = 0;i < secEndPosition.Length;i++)
        {
            if(other.gameObject == secEndPosition[i])
            {
                secEndTime[i] = secTime;
                secTime = 0f;
                Debug.Log("Sec" + (i + 1) + "time:" + secEndTime[i]);
                if(i == secEndPosition.Length - 1)
                {
                    Debug.Log("TotalTime:" + totalTime);
                }
            }
        }
    }

}
