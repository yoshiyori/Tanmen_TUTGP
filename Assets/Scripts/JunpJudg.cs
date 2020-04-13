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
    [SerializeField] private CuePlayer jumpSound;
    [SerializeField] private MovePlayer movePlayer;

    public Handle hd;//JoyConから数値受け取る時とかに使う
    //[SerializeField] bool joyconFlag;//JoyCon使うかどうかのフラグ

    private void Start()
    {
        triggerObsFlag = false;
        rigid = playerObject.GetComponent<Rigidbody>();
        playerPosition = playerObject.transform.position;
        //inertiaPlayer = playerObject.GetComponent<InertiaPlayer>();                         //サウンド追加分 2/4
    }

    private void Update()
    {
        if (triggerObsFlag == true && ( Input.GetKey(KeyCode.S) || 
            (hd.GetControllerSwing() >= 10 && movePlayer.joyconFlag == true) )
            )//ここのInputを振り上げ
        {
            rigid.AddForce(0, junpSpeed, 0);
            triggerObsFlag = false;
            nowJunpFlag = true;

            //サウンド追加分 3/4
            movePlayer.junp = true;
            jumpSound.Play("Jump");
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
        /*if (nowJunpFlag == true && ( Input.GetKey(KeyCode.A) || 
            (hd.GetControllerSwing() <= -10 && movePlayer.joyconFlag == true) )
            )//ここのInputを振り下ろしにして*/
        if(nowJunpFlag)
        {
            rigid.AddRelativeForce(-junpAccelSpeed, 0, 0);
            nowJunpFlag = false;
            Debug.Log("JunpAcccel");
            movePlayer.succesRollingJump = true;                                         //サウンド追加分 4/4
        }

    }
}
