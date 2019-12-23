using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField][Range (0, 1)] float speed; //走るスピード
    [SerializeField] GameObject[] Rails; //レール　Targetが子についたオブジェクト　左から順番にアタッチ
    [SerializeField] GameObject[] PlayerInternal; //プレイヤーの乗るやつ　場所はスタートしたら強制移動なのでどこでも良い
    private List<Transform[]> targets = new List<Transform[]> (); //レール配列のリスト
    private List<LTSpline> paths = new List<LTSpline> (); //レール配列から生成したSpline曲線のリスト
    private List<Vector3[]> targetVectors = new List<Vector3[]> (); //レール配列のリストVec3ver.
    private float trackPosition;
    private int activeRailNum = 1;
    void Start ()
    {
        if (Rails != null)
        {
            foreach (var Rail in Rails)
            {
                //targetsに各レールのガイドのtransformを入れる
                targets.Add (Rail.GetComponentsInChildrenWithoutSelf<Transform> ());
            }
            //transformをVector3に
            targetVectors = targets.ConvertAll (new Converter<Transform[], Vector3[]> (ToVector3));
            PathSetUp ();
        }
    }

    // Update is called once per frame
    void Update ()
    {
        //前進
        for (int i = 0; i < paths.Count; i++)
        {
            GoForward (i); //前に進むやつ
        }
        trackPosition += Time.deltaTime * speed;
        //ループ
        if (trackPosition > 1)
        {
            trackPosition = 0;
        }

        //レーン切り替え
        if (Input.GetKeyDown (KeyCode.RightArrow))
        {
            activeRailNum++;
            if (activeRailNum >= PlayerInternal.Length)
            {
                activeRailNum = PlayerInternal.Length - 1;
            }
            RailChange (activeRailNum);
        }
        if (Input.GetKeyDown (KeyCode.LeftArrow))
        {
            activeRailNum--;
            if (activeRailNum < 0)
            {
                activeRailNum = 0;
            }
            RailChange (activeRailNum);
        }
    }

    void PathSetUp ()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            //Vector3ジャグ配列を元にパスの配列に代入　spline曲線を生成
            var x = new LTSpline (targetVectors[i]);
            paths.Add (x);
        }
    }

    void GoForward (int i)
    {
        paths[i].place (PlayerInternal[i].transform, trackPosition);
    }

    void RailChange (int i)
    {
        this.transform.parent = PlayerInternal[i].transform;
        this.transform.localPosition = Vector3.zero; //これをアニメーションさせればいい感じになるかも
    }
    public static Vector3[] ToVector3 (Transform[] t)
    {
        Vector3[] v = new Vector3[t.Length];
        for (int i = 0; i < t.Length; i++)
        {
            v[i] = t[i].position;
        }
        return v;
    }
    //void OnDrawGizmos () { if (paths != null) paths[1].gizmoDraw (); }//Spline曲線のギズモを書ける
}