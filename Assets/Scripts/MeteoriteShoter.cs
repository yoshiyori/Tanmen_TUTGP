using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteShoter : MonoBehaviour
{
    public GameObject meteorite;  //打ち出す隕石を入れる変数
    public GameObject player; //Playerオブジェクトを入れる変数
    public GameObject target; //狙うエリアを入れる変数
    public bool mudDumplingB = false; //泥団子B用の仕様に変更するときに使う（要相談）
    private Vector3 targetPosition; //ターゲットの座標取得用変数

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject　== player)
        {
            //隕石を打ち出す場所を決める(旧型)
            /*float x = other.transform.position.x - 1.0f; //プレイヤーの少し後ろから打ち出す
            float y = 20.0f;
            float z = Random.Range(-15.0f, 15.0f);　//横の位置はランダム（現在は直線の道幅いっぱい）
            Vector3 shotPosition = new Vector3(x, y, z);*/

            //オブジェクトの位置ベクトルを参照して泥団子の打ち出す位置を決める
            Vector3 shotPosition = transform.position + transform.right * 1.0f + transform.up * 20.0f + transform.forward * Random.Range(-15.0f, 15.0f);

            if(mudDumplingB == true)
            {
                //泥団子B用の動作。ターゲットの中心に向かって飛ぶ
                //ターゲットの座標取得
                targetPosition = target.transform.position;
            }
            else
            {
                //泥団子A用の動作。エリア（BoxCollider）の中のいずれかの場所に飛ぶ
                //注意：BoxColliderの中心は原点（0,0,0）から動かさないようにすること
                BoxCollider targetArea = target.GetComponent<BoxCollider>();
                float xRange = targetArea.size.x / 2.0f;
                float zRange = targetArea.size.z / 2.0f;
                Vector3 targetCenter = target.transform.position;
                targetPosition = new Vector3(targetCenter.x + Random.Range(-xRange, xRange), 0, targetCenter.z + Random.Range(-zRange, zRange));
            }

            //初速度の計算
            Vector3 velocity = CalculateVelocity(shotPosition, targetPosition);

            //泥団子生成
            GameObject createMeteorite = Instantiate(meteorite) as GameObject;
            createMeteorite.transform.position = shotPosition;

            //泥団子発射
            createMeteorite.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);

        }
    }

    //初速度の計算（ネットにあったものとほぼそのまま）
    private Vector3 CalculateVelocity(Vector3 pointA,Vector3 pointB)
    {

        float angle = 0f; //水平に飛ばすので角度は0

        // 射出角をラジアンに変換（多分今回はこの動作いらないけど、角度を指定したいとか言い出した時のために残しておく）
        float rad = angle * Mathf.PI / 180;

        // 水平方向の距離x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        // 垂直方向の距離y
        float y = pointA.y - pointB.y;

        // 斜方投射の公式を初速度について解く
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // 条件を満たす初速を算出できなければVector3.zeroを返す
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }
}
