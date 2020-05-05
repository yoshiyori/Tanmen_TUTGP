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

    //サウンド追加分 1/4
    [SerializeField] private CuePlayer jumpSound;
    [SerializeField] private MovePlayer movePlayer;

    //Gauge関係
    [SerializeField] private GameObject SwingGaugePanel;
    [SerializeField] private Slider swingGauge;
    [SerializeField] private float upNum;
    private bool nowAccelFlag;
    private float time;
    private float useGaugeTime;
    private bool isPlus;

    [SerializeField] CheckTouching ct;

    public Handle hd;//JoyConから数値受け取る時とかに使う
    //[SerializeField] bool joyconFlag;//JoyCon使うかどうかのフラグ

    private void Start()
    {
        triggerObsFlag = false;
        rigid = playerObject.GetComponent<Rigidbody>();
        playerPosition = playerObject.transform.position;
        //inertiaPlayer = playerObject.GetComponent<InertiaPlayer>();                         //サウンド追加分 2/4

        //Gauge関係
        swingGauge.value = 0.0f;
        useGaugeTime = 5;
        isPlus = false;
        nowAccelFlag = false;
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

            //サウンド追加分 3/4
            movePlayer.junp = true;
            jumpSound.Play("Jump");

            //Gauge関係
            SwingGaugePanel.SetActive(!SwingGaugePanel.activeInHierarchy);
        }
        triggerObsFlag = false;

        if (nowJunpFlag == true)
        {
            ChargeGauge(1);//ぐるぐるするやつを選択するために１をいれた
            if (ct.GetTouching()) nowJunpFlag = false;
        }
        if (nowJunpFlag == false && SwingGaugePanel.activeInHierarchy == true)
        {
            UseGauge();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            triggerObsFlag = true;
            nowSpeed = rigid.velocity;
        }
        if (other.gameObject.tag == "Rode")
        {
            nowJunpFlag = false;
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
            Debug.Log("JunpAcccel");
            movePlayer.succesRollingJump = true;                                         //サウンド追加分 4/4
        }

    }

    void ChargeGauge(int argModeSelectNum)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            swingGauge.value += upNum;
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
                    }
                }
                if (hd.GetCycleSwing(0) < -2.5)
                {
                    if (isPlus == true)
                    {
                        swingGauge.value += upNum;
                        isPlus = false;
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
                    }
                }
                if (hd.GetControlllerAccel(1) <= -0.3)
                {
                    if (isPlus == false)
                    {
                        swingGauge.value += upNum;
                        isPlus = true;
                    }
                }
                if ((hd.GetControlllerAccel(1) <= 0.1 && hd.GetControlllerAccel(1) >= 0.0) ||
                    (hd.GetControlllerAccel(1) >= -0.1 && hd.GetControlllerAccel(1) <= 0.0))
                {
                    if (isPlus == true)
                    {
                        swingGauge.value += upNum;
                        isPlus = false;
                    }

                }
                break;
            default:
                break;
        }


    }

    void UseGauge()
    {
        time += Time.deltaTime;
        if (time > useGaugeTime)
        {
            time = 0.0f;
            swingGauge.value -= upNum * 2;
            rigid.AddRelativeForce(-junpAccelSpeed, 0, 0);
        }
        if (swingGauge.value <= 0.0f)
        {
            SwingGaugePanel.SetActive(!SwingGaugePanel.activeInHierarchy);
        }
    }
}
