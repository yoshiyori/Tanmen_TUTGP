using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteShoter : MonoBehaviour
{
    public Rigidbody meteorite;    //rigidbody型の変数を用意します
    public Transform triggerPos;
    public float shotForce = 10f;  //浮動小数点型の変数shotForceを用意し、10fを入れます（弾を撃つ力））

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            float y = 20.0f;
            float z = Random.Range(-15.0f, 15.0f);
            Vector3 position = new Vector3(triggerPos.position.x, y, z);
            Rigidbody shot = Instantiate(meteorite, position, triggerPos.rotation) as Rigidbody;
            shot.AddForce(-triggerPos.right * shotForce, ForceMode.Impulse);
        }
    }

    /*void Update()
    {
        if (Input.GetButtonUp("Fire1"))     //もし”Fire1”（左Ctrlキー）が押されたなら・・
        {
            //Rigidbody型の変数shotを用意し、Instantiateで（prefab、位置、回転）を指定して複製したものを入れます
            Rigidbody shot = Instantiate(projectile, shotPos.position, shotPos.rotation) as Rigidbody;
            //変数shotで作られた複製にAddForceで物理的な力（Impulse衝撃力）をz軸方向（forward）に加えます
            shot.AddForce(shotPos.forward * shotForce, ForceMode.Impulse);
        }
    }*/
}
