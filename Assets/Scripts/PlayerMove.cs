using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField][Range (0, 1)] float speed; //走るスピード
    [SerializeField] GameObject[] Rails; //レール　Targetが子についたオブジェクト　左から順番にアタッチ
    [SerializeField] GameObject[] PlayerInternal; //プレイヤーの乗るやつ　場所はスタートしたら強制移動なのでどこでも良い
    private Transform[][] targets = new Transform[3][]; //Transform[x][] xはレールの個数
    private LTSpline[] paths;
    private Vector3[][] targetVectors = new Vector3[3][]; //Vector3[x][] xはレールの個数 
    private float trackPosition;
    private int activeRailNum = 1;
    void Start ()
    {
        if (Rails != null)
        {
            int i = 0;
            foreach (var Rail in Rails)
            {
                //targetsに各レールのガイドのtransformを入れる
                targets[i] = Rail.GetComponentsInChildrenWithoutSelf<Transform> ();
                i++;

            }
		
            for (int k = 0; k < targets.Length; k++)
            {
                //初期化処理
                targetVectors[k] = new Vector3[targets.Length];
                paths = new LTSpline[targets.Length];
            }
		

            for (int j = 0; j < targets.Length; j++)
            {
                for (int l = 0; l < targets[0].Length; l++)
                {
                    //targetsジャグ配列をVector3ジャグ配列に代入
                    targetVectors[j][l] = targets[j][l].position;
                }
            }

			pathSetUp ();
        }
    }

    // Update is called once per frame
    void Update ()
    {
		//Debug.Log(paths.Length);
        //前進
        paths[0].place (PlayerInternal[0].transform, trackPosition);
        paths[1].place (PlayerInternal[1].transform, trackPosition);
        paths[2].place (PlayerInternal[2].transform, trackPosition);
        //レーンを増やしたらその都度増やす　どうにかしたい
        trackPosition += Time.deltaTime * speed;

        //レーン切り替え
        if (Input.GetKeyDown (KeyCode.RightArrow))
        {
            activeRailNum++;
            if (activeRailNum >= PlayerInternal.Length)
            {
                activeRailNum = PlayerInternal.Length - 1;
            }
        }
        if (Input.GetKeyDown (KeyCode.LeftArrow))
        {
            activeRailNum--;
            if (activeRailNum < 0)
            {
                activeRailNum = 0;
            }
        }

        switch (activeRailNum)
        {
            case 0:
                this.transform.parent = PlayerInternal[0].transform;
                this.transform.localPosition = Vector3.zero; //ここにアニメーションつければ回避っぽくなるかしら？　未検証
                break;
            case 1:
                this.transform.parent = PlayerInternal[1].transform;
                this.transform.localPosition = Vector3.zero;
                break;
            case 2:
                this.transform.parent = PlayerInternal[2].transform;
                this.transform.localPosition = Vector3.zero;
                break;
                //レーンを増やしたらその都度増やす　どうにかしたい
        }

    }

    ///<summary>
    ///パスの生成
    ///</summary>
    void pathSetUp ()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            //Vector3ジャグ配列を元にパスの配列に代入　spline曲線を生成
            paths[i] = new LTSpline (targetVectors[i]);
        }
    }
}