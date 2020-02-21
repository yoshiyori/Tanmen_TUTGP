using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour
{
    [SerializeField] GameObject[] obstacles;

    Obstacle[] obsScr = new Obstacle[3];//ここの配列数は手動で変えなければいけない。後でもっと楽な感じにする
    public bool mudFlag;       //mudに当たった時true
    [SerializeField]private int flagCtrlNum;    //trueの数記録
    [SerializeField] int flagCheckNum = 0;
    void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            obsScr[i] = obstacles[i].GetComponent<Obstacle>();
        }

        flagCtrlNum = 0;
        mudFlag = false;
    }

    void Update()
    {
        flagCheckNum = 0;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (obsScr[i].triggerObsFlag == true) flagCheckNum++;
        }
        if (flagCheckNum > flagCtrlNum && mudFlag == false)//true増えたら当たった判定送る
        {
            mudFlag = true;
        }
        if (flagCheckNum == flagCtrlNum && mudFlag == true)//true増えたら当たった判定送る
        {
            mudFlag = false;
        }

    }
}
