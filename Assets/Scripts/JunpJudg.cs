using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunpJudg : MonoBehaviour
{
    [SerializeField] float junpAccelSpeed;
    public bool triggerObsFlag;
    public bool nowJunpFlag;
    private Rigidbody rigid;
    public GameObject playerObject;
    public float junpSpeed;
    private Vector3 nowSpeed;
    private Vector3 playerPosition;

    //サウンド追加分 1/4
    [SerializeField] private CriAtomSource jumpSound;
    private InertiaPlayer inertiaPlayer;

    private void Start()
    {
        triggerObsFlag = false;
        rigid = playerObject.GetComponent<Rigidbody>();
        playerPosition = playerObject.transform.position;
        inertiaPlayer = playerObject.GetComponent<InertiaPlayer>();                         //サウンド追加分 2/4
    }

    private void Update()
    {
        if (triggerObsFlag == true && Input.GetKey(KeyCode.S))//ここのInputを振り上げ
        {
            rigid.AddForce(0, junpSpeed, 0);
            triggerObsFlag = false;
            nowJunpFlag = true;

            //サウンド追加分 3/4
            inertiaPlayer.junp = true;
            jumpSound.Play();
        }
        triggerObsFlag = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            triggerObsFlag = true;
            nowSpeed = rigid.velocity;
        }
        if (other.gameObject.tag == "Lode")
        {
            nowJunpFlag = false;
        }

    }
    public void JunpPlayer()
    {

        if (nowJunpFlag == true && Input.GetKey(KeyCode.A))//ここのInputを振り下ろしにして
        {
            rigid.AddRelativeForce(-junpAccelSpeed, 0, 0);
            nowJunpFlag = false;
            Debug.Log("JunpAcccel");
            inertiaPlayer.succesRollingJump = true;                                         //サウンド追加分 4/4
        }

    }
}
