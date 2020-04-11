using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudDumplingA : MonoBehaviour
{
    //サウンド追加分 1/2
    [SerializeField] private CuePlayer crayBallDebrySound;

    void Reset(){
        crayBallDebrySound = GetComponent<CuePlayer>();
    }
    //サウンド追加分 1/2終了

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            //スピードを0にしてオブジェクトを消す
            other.rigidbody.velocity = Vector3.zero;

            //サウンド追加分 2/2
            crayBallDebrySound.PlayAndDestroy("CrayBallDebries");
            //Destroy(this.gameObject);                             //サウンド変更部分
        }
    }
}
