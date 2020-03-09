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
    [SerializeField] private ConeBreakSound coneSound;       //サウンド追加分 1/2

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
            coneSound.PlayAndDestroyed();                               //サウンド追加分 2/2
            Destroy(this.gameObject);
        }
    }

 
}
