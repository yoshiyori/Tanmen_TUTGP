using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeAttck : MonoBehaviour
{
    public bool triggerObsFlag;
    private Rigidbody rigid;
    public GameObject playerObject;
    public float downSpeed;
    private Vector3 nowSpeed;

    [SerializeField] private CuePlayer cornSound;           //サウンド追加分 1/2

    private void Start()
    {
        triggerObsFlag = false;
        rigid = playerObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            triggerObsFlag = true;
            nowSpeed = rigid.velocity;

            rigid.velocity = Vector3.zero;
            cornSound.PlayAndDestroy("Corn");                //サウンド追加分 2/2
            //Destroy(this.gameObject);                      //サウンド変更分 1/1
        }
    }

 
}
