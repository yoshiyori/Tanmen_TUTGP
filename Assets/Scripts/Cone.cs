using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    [SerializeField] GameObject[] obstacles;

    Obstacle[] obsScr = new Obstacle[3];//ここの配列数は手動で変えなければいけない。後でもっと楽な感じにする
    //List<Obstacle> obsScr;
    public bool coneFlag;       //coneに当たった時true
    private int flagCtrlNum;    //trueの数記録

    void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            obsScr[i] = obstacles[i].GetComponent<Obstacle>();
        }
        
        flagCtrlNum = 0;
        coneFlag = false;
    }

    void Update()
    {
        int flagCheckNum = 0;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (obsScr[i].triggerObsFlag == true) flagCheckNum++;
        }
        if (flagCheckNum > flagCtrlNum)//true増えたら当たった判定送る
        {
            coneFlag = true;
            flagCtrlNum++;
        }
    }
}
