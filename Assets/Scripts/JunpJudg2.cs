using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunpJudg2 : MonoBehaviour
{
    [SerializeField] float junpAccelSpeed;
    public bool triggerObsFlag;
    public bool nowJunpFlag;
    private Rigidbody rigid;
    public GameObject playerObject;
    public float junpSpeed;
    private Vector3 nowSpeed;
    private Vector3 playerPosition;

    public Handle hd;

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
        if (triggerObsFlag == true && (hd.GetControllerSwing() >= 10 || Input.GetKey(KeyCode.S)))//ここのInputを振り上げ
        {
            rigid.AddForce(0, junpSpeed, 0);
            triggerObsFlag = false;
            nowJunpFlag = true;

            //サウンド追加分 3/4
            inertiaPlayer.junp = true;
            jumpSound.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            triggerObsFlag = true;         
            nowSpeed = rigid.velocity;
        }
        if (playerPosition.y < 1.3)
        {
            nowJunpFlag = false;
        }

    }
    public void JunpPlayer()
    {

        if (nowJunpFlag == true && (hd.GetControllerSwing() <= -10 || Input.GetKey(KeyCode.A)))//ここのInputを振り下ろしにして
        {
            rigid.AddRelativeForce(-junpAccelSpeed, 0, 0);
            nowJunpFlag = false;
            Debug.Log("JunpAcccel");
            inertiaPlayer.succesRollingJump = true;                                         //サウンド追加分 4/4
        }

    }
}
