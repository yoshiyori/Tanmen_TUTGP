using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool triggerObsFlag;
    private Rigidbody rigid;
    public GameObject playerObject;
    //public float downSpeed;
    private Vector3 nowSpeed;
    [SerializeField] private bool triggerTypeKakunin;//mud:false, cone:true
    [SerializeField] private CuePlayer mudSound;                                //サウンド追加分1/3

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
           // nowSpeed = rigid.velocity;
            mudSound.Play("Mud");                                                        //サウンド追加分2/3

        /*    if (nowSpeed.x < 0)
            {
                rigid.AddForce(downSpeed, 0, 0);
            }
            else if (nowSpeed.x > 0)
            {
                rigid.AddForce(-downSpeed, 0, 0);
            }
       
           */

           //mud_CueBank.play("Mud");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && triggerTypeKakunin == false)
        {
            triggerObsFlag = false;
            mudSound.Stop();                                                        //サウンド追加分3/3
        }
    }

}
