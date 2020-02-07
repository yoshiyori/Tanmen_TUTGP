using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteShoter : MonoBehaviour
{
    public Rigidbody meteorite;  //打ち出す隕石を入れる変数
    public Transform triggerPos;  //triggerの位置を入れる変数
    [SerializeField] float shotForce = 10f;  //隕石を打ち出す力の数値

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //隕石を打ち出す場所を決める
            float y = 20.0f;　//高さ
            float z = Random.Range(-15.0f, 15.0f);　//横の位置はランダム（現在は道幅いっぱい）
            Vector3 position = new Vector3(triggerPos.position.x, y, z);　//打ち出す場所を入れる変数

            Rigidbody shot = Instantiate(meteorite, position, triggerPos.rotation) as Rigidbody;　//隕石生成
            shot.AddForce(-triggerPos.right * shotForce, ForceMode.Impulse);　//隕石発射
        }
    }
}
