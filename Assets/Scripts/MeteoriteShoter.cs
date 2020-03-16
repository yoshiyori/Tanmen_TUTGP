using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteShoter : MonoBehaviour
{
    public GameObject meteorite;  //打ち出す隕石を入れる変数
    public GameObject player; //Playerオブジェクトを入れる変数
    [SerializeField] float shotForce = 10f;  //隕石を打ち出す力の数値

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject　== player)
        {
            //隕石を打ち出す場所を決める
            float x = other.transform.position.x - 1.0f; //プレイヤーの少し後ろから打ち出す
            float y = 20.0f;
            float z = Random.Range(-15.0f, 15.0f);　//横の位置はランダム（現在は直線の道幅いっぱい）
            Vector3 position = new Vector3(x, y, z);

            //隕石生成
            GameObject createMeteorite = Instantiate(meteorite) as GameObject;
            createMeteorite.transform.position = position;

            //隕石発射
            createMeteorite.GetComponent<Rigidbody>().AddForce(createMeteorite.transform.right * -shotForce);

        }
    }
}
