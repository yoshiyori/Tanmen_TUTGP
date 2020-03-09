using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudDumplingA : MonoBehaviour
{
    //サウンド追加分 1/2
    [SerializeField] private CrayBallDebriesSound crayBallDebriesSound;
    [SerializeField, NonEditable] private bool destroyFlag;
    private bool recentDestroy = false;

    private bool destroy{
        get{
            return destroyFlag;
        }
        set{
            Debug.Log("set destroy : " + value.ToString());
            destroyFlag = value;
        }
    }

    void Setup(){
        destroy = false;
    }

    void Update(){
        if(destroy){
            Destroy(this.gameObject);
        }
        /*if(recentDestroy && !destroy){
            Debug.Log("unti");
        }
        recentDestroy = destroy;*/
    }
    //サウンド追加分 1/2終了

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            //スピードを0にしてオブジェクトを消す
            other.rigidbody.velocity = Vector3.zero;

            //サウンド追加分 2/2
            if(crayBallDebriesSound != null)
            {
                crayBallDebriesSound.PlayAndDestroyed();
            }
            //Destroy(this.gameObject);                     //サウンド変更部分
            destroy = true;
            //recentDestroy = true;
            //Debug.Log(destroy);
        }
    }
}
