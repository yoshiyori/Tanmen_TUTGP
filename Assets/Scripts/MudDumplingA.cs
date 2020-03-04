using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudDumplingA : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            //スピードを0にしてオブジェクトを消す
            other.rigidbody.velocity = Vector3.zero;
            Destroy(this.gameObject);
        }
    }
}
