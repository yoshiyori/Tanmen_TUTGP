using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class carPlayer : MonoBehaviour
{

    [Range(-1.0f, +1.0f)]
    public float steering = 0.0f;

    public float engineTorqu;
    public float breakTorqu;
    public float maxAngle;
    public float maxTorqu;
    public float maxBreakTorqe;
    private bool check;
    public WheelCollider[] FrontWheel;
    public WheelCollider[] RearWheel;

    // カメラの注視点
    public GameObject CameraLookAt;

    // カメラの位置の配列
    public GameObject[] CameraPositions;

    //サウンド追加分 1/3
    [SerializeField] private CuePlayer playerSound;
    [SerializeField] private Rigidbody playerRigid;
    //サウンド追加分 1/3 終了

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        check = false;
        // ハンドルを切る
        float angleWheel = maxAngle * steering;
        foreach (var wheel in FrontWheel)
        {
            wheel.steerAngle = angleWheel;
            // タイヤの角度を変える
            var rotate = wheel.transform.localRotation;
            rotate = Quaternion.AngleAxis(angleWheel, Vector3.up);
            wheel.transform.localRotation = rotate;
        }
        float torqu = maxTorqu * engineTorqu;
        foreach (var wheel in RearWheel)
        {
            wheel.motorTorque = torqu;
        }
        foreach (var wheel in FrontWheel)
        {
            wheel.motorTorque = torqu;
        }

        float breakTorqu2 = maxBreakTorqe * breakTorqu;

        // 前後のタイヤにブレーキ
        foreach (var wheel in FrontWheel)
        {
            wheel.brakeTorque = breakTorqu2;
          
        }
        foreach (var wheel in RearWheel)
        {
            wheel.brakeTorque = breakTorqu2;
          
        }

        
        

        Tire();

        TorqueOperation();

        //サウンド追加分 2/3
        if((FrontWheel.Any(wheel => wheel.isGrounded) || RearWheel.Any(wheel => wheel.isGrounded)) &&
            playerRigid.velocity.magnitude > 0.5f && !playerSound.JudgeAtomSourceStatus("Playing", 1))
        {
            playerSound.Play("Running", 1);
        }
        else
        {
            playerSound.Stop(1);
        }
        //サウンド追加分 2/3 終了
    }

    void Tire()
    {


        if (Input.GetKey(KeyCode.RightArrow) && steering < 1.0)
        {
            steering = steering + 0.001f;
           
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && steering > -1.0)
        {
            steering = steering + -0.001f;
          
        }
        
    }
    void TorqueOperation()
    {
        //サウンド追加分 3/3
        if(Input.GetKey(KeyCode.DownArrow)){
            playerSound.Play("Break");
        }
        //サウンド追加分 3/3 終了

        if (Input.GetKey(KeyCode.DownArrow) && engineTorqu < 100)
        {
            breakTorqu = 1.0f;

        }
        else if(engineTorqu >-100)
        {
            engineTorqu -= 40;
        }

    }
}