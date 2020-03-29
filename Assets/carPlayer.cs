using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carPlayer : MonoBehaviour
{
    [Range(-1.0f, +1.0f)]
    public float steering = 0.0f;
  
    public float engineTorqu;

    public float breakTorqu;
    public float maxAngle;
    public float maxTorqu;
    public float maxBreakTorqe;

    public WheelCollider[] FrontWheel;
    public WheelCollider[] RearWheel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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

        if (Input.GetKey(KeyCode.RightArrow) && steering < 1.0)
        {
            steering = steering + 0.1f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && steering > -1.0)
        {
            steering = steering + -0.1f;
        }
        else
        {
            steering = 0;
        }
    }
}
