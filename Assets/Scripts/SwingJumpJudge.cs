using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwingJumpJudge : MonoBehaviour
{
    [SerializeField] float junpAccelSpeed;
    public bool triggerObsFlag;
    public bool nowJunpFlag;
    private Rigidbody rigid;
    public GameObject playerObject;
    public float junpSpeed;
    private Vector3 nowSpeed;
    private Vector3 playerPosition;
    public Animator PlayerAni;

    //サウンド追加分 1/8
    [SerializeField] private CuePlayer jumpSound;
    [SerializeField] private MovePlayer movePlayer;
    private float loopTime = 0f;

    //Gauge関係
    [SerializeField] private GameObject swingGaugeObject;
    [SerializeField] private GameObject swingCommandTextObject;
    [SerializeField] private Slider swingGauge;
    [SerializeField] private float upNum;
    private float time;
    private bool isPlus;
    private bool touchingGroundFrag;
    [SerializeField] private bool afterJumpingFlag;
    [SerializeField] int useGaugeSpeed;
    private bool movingFlag;
    [SerializeField] int decreaseGaugeSpeed;


    public Handle hd;//JoyConから数値受け取る時とかに使う
    //[SerializeField] bool joyconFlag;//JoyCon使うかどうかのフラグ

    private void Start()
    {
        triggerObsFlag = false;
        rigid = playerObject.GetComponent<Rigidbody>();
        playerPosition = playerObject.transform.position;
        //inertiaPlayer = playerObject.GetComponent<InertiaPlayer>();                         //サウンド追加分 2/8

        //Gauge関係
        swingGauge.value = 0.0f;
        isPlus = false;
        touchingGroundFrag = movePlayer.GetSandCtrl();
        afterJumpingFlag = false;
        movingFlag = false;
        if (useGaugeSpeed == 0) useGaugeSpeed = 30;
        if (decreaseGaugeSpeed == 0) decreaseGaugeSpeed = 15;
        if (upNum == 0.0f)
        {
            upNum = 0.05f;
        }
    }

    private void Update()
    {
        if (triggerObsFlag == true)
        {
            rigid.AddForce(0, junpSpeed, 0);
            triggerObsFlag = false;
            nowJunpFlag = true;

            //サウンド追加分 3/8
            movePlayer.junp = true;
            jumpSound.Play("Jump");
            PlayerAni.SetTrigger("Junp");
            //Gauge関係
            if (swingGaugeObject.activeInHierarchy == false && swingCommandTextObject.activeInHierarchy == false)
            {
                swingGaugeObject.SetActive(true);
                swingCommandTextObject.SetActive(true);
            }
        }
        triggerObsFlag = false;
        touchingGroundFrag = movePlayer.GetSandCtrl();

        if (nowJunpFlag == true && afterJumpingFlag == true)
        {
            ChargeGauge(1);//ぐるぐるするやつを選択するために１をいれた
            if (touchingGroundFrag)
            {
                nowJunpFlag = false;
                afterJumpingFlag = false;
                if (swingCommandTextObject.activeInHierarchy)
                {
                    if (swingGauge.value > 0.0f) movePlayer.succesRollingJump = true;
                    movePlayer.swingBoostFlag = true;
                    swingCommandTextObject.SetActive(false);
                }
            }
        }
        if (nowJunpFlag == false && swingGaugeObject.activeInHierarchy == true && swingCommandTextObject.activeInHierarchy == false)
        {
            UseGauge();

            //サウンド追加分 4/8
            jumpSound.StopStrechLoop(1);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            triggerObsFlag = true;
            nowSpeed = rigid.velocity;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            afterJumpingFlag = true;
        }
    }

    public void JunpPlayer()
    {
        /*if (nowJunpFlag == true && ( Input.GetKey(KeyCode.A) || 
            (hd.GetControllerSwing() <= -10 && movePlayer.joyconFlag == true) )
            )//ここのInputを振り下ろしにして*/
        if (nowJunpFlag) 
        {
            rigid.AddRelativeForce(-junpAccelSpeed, 0, 0);
            nowJunpFlag = false;
            //Debug.Log("JunpAcccel");
            movePlayer.succesRollingJump = true;                //サウンド追加分 5/8
        }

    }

    void ChargeGauge(int argModeSelectNum)
    {
        time += Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            swingGauge.value += upNum * 0.09f;
            movingFlag = true;
            
            //サウンド追加分 6/8
            jumpSound.loopTime = 0.5f + (Mathf.Sqrt(1 - swingGauge.value * swingGauge.value) * 0.5f);            //スイングブースト ループ再生間隔制御
            jumpSound.PlayStrechLoop("SwingBoost", 1);
        }

        //サウンド追加分 7/8
        else
        {
            jumpSound.StopStrechLoop(1, false);
        }

        switch (argModeSelectNum)
        {
            case 0:
                if (hd.GetCycleSwing(0) > 2.5)
                {
                    if (isPlus == false)
                    {
                        swingGauge.value += upNum;
                        isPlus = true;
                        movingFlag = true;
                    }
                }
                if (hd.GetCycleSwing(0) < -2.5)
                {
                    if (isPlus == true)
                    {
                        swingGauge.value += upNum;
                        isPlus = false;
                        movingFlag = true;
                    }

                }
                break;
            case 1:
                if (hd.GetControlllerAccel(1) >= 0.3)
                {
                    if (isPlus == false)
                    {

                        swingGauge.value += upNum;
                        isPlus = true;
                        movingFlag = true;
                    }
                }
                if (hd.GetControlllerAccel(1) <= -0.3)
                {
                    if (isPlus == false)
                    {
                        swingGauge.value += upNum;
                        isPlus = true;
                        movingFlag = true;
                    }
                }
                if ((hd.GetControlllerAccel(1) <= 0.1 && hd.GetControlllerAccel(1) >= 0.0) ||
                    (hd.GetControlllerAccel(1) >= -0.1 && hd.GetControlllerAccel(1) <= 0.0))
                {
                    if (isPlus == true)
                    {
                        swingGauge.value += upNum;
                        isPlus = false;
                        movingFlag = true;
                    }

                }
                break;
            default:
                break;
        }

        if (movingFlag == true)
        {
            time = 0.0f;
            movingFlag = false;
        }
        if (time > 0.5)
        {

            swingGauge.value -= upNum * decreaseGaugeSpeed * Time.deltaTime;
        }
    }

    void UseGauge()
    {
        time += Time.deltaTime;
        if (time > Time.deltaTime)
        {
            time = 0.0f;
            swingGauge.value -= upNum * useGaugeSpeed * Time.deltaTime;
            rigid.AddRelativeForce(-junpAccelSpeed, 0, 0);
        }
        if (swingGauge.value <= 0.0f)
        {
            if (swingGaugeObject.activeInHierarchy) swingGaugeObject.SetActive(false);
            movePlayer.swingBoostFlag = false;
        }
    }

    public bool GetAfterJumpingFlag()
    {
        return afterJumpingFlag;
    }

    //サウンド追加分 8/8
    private void PlaySwingBoostSound(){
        if(loopTime <= 0f){
            jumpSound.Play("SwingBoost");
        }

        loopTime += Time.deltaTime;
        if(loopTime >= 0.7f - swingGauge.value * 0.5f){
            //Debug.Log(loopTime.ToString() + ", " + (1f - swingGauge.value * 0.5f).ToString());
            loopTime = 0f;
        }
    }
}