using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class MudDumplingA : MonoBehaviour
{
    //サウンド追加分 1/2
    [SerializeField] private CuePlayer crayBallDebrySound;
    [SerializeField] private MeshFilter crayBallDebryMesh;
    [SerializeField] private Collider crayBallDebryCollider;
    private bool destroy = false;

    void Reset(){
        crayBallDebrySound = GetComponent<CuePlayer>();
        crayBallDebryMesh = GetComponent<MeshFilter>();
        crayBallDebryCollider = GetComponent<Collider>();
    }

    void Update(){
        if(destroy){
            Destroy(this.gameObject);
        }
    }
    //サウンド追加分 1/2終了

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            //スピードを0にしてオブジェクトを消す
            other.rigidbody.velocity = Vector3.zero;

            //サウンド追加分 2/2
            crayBallDebrySound.PlayAndDestroy("CrayBallDebries", ref crayBallDebryMesh, ref crayBallDebryCollider);
            //Destroy(this.gameObject);                     //サウンド変更部分
            //destroy = true;
        }
    }
}
