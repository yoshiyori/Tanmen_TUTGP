using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudDumplingA : MonoBehaviour
{
    [SerializeField] private CrayBallDebriesSound crayBallDebriesSound;     //サウンド追加分 1/2

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            //スピードを0にしてオブジェクトを消す
            other.rigidbody.velocity = Vector3.zero;
            crayBallDebriesSound.PlayAndDestroyed();                        //サウンド追加分 2/2
            Destroy(this.gameObject);
        }
    }
}
