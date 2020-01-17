using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteShoter : MonoBehaviour
{
    public Rigidbody projectile;    //rigidbody型の変数を用意します
    public Transform triggerPos;       //弾を撃つ場所の位置と回転、大きさを入れる変数shotPosを用意します
    public float shotForce = 10f;  //浮動小数点型の変数shotForceを用意し、100fを入れます（弾を撃つ力））

    void OnTriggerEnter(Collider other)
    {
        float y = 20.0f;
        float z = Random.Range(-50.0f, 50.0f);
        Rigidbody shot = Instantiate(projectile, Pos.position, shotPos.rotation) as Rigidbody;
    }

    void Update()
    {
        if (Input.GetButtonUp("Fire1"))     //もし”Fire1”（左Ctrlキー）が押されたなら・・
        {
            //Rigidbody型の変数shotを用意し、Instantiateで（prefab、位置、回転）を指定して複製したものを入れます
            Rigidbody shot = Instantiate(projectile, shotPos.position, shotPos.rotation) as Rigidbody;
            //変数shotで作られた複製にAddForceで物理的な力（Impulse衝撃力）をz軸方向（forward）に加えます
            shot.AddForce(shotPos.forward * shotForce, ForceMode.Impulse);
        }
    }
}
