using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Handle : MonoBehaviour
{
    private Joycon m_joyconR;
    private Joycon m_joyconL;
    private Vector3 accel;
    private Vector3 gyro;
    private Joycon.Button? rButton;
    private Joycon.Button? lButton;
    private bool rBrake;
    private bool lBrake;

    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    //[SerializeField] bool rlCheck;//checkの場合right

    private void Start()
    {
        var joycons = JoyconManager.Instance.j;
        m_joyconR = joycons.Find(c => !c.isLeft);
        m_joyconL = joycons.Find(c => c.isLeft);

        rBrake = false;
        lBrake = false;
    }

    private void Update()
    {
        rButton = null;
        lButton = null;

        accel = m_joyconL.GetAccel();
        gyro = m_joyconL.GetGyro();

        foreach (var button in m_buttons)
        {
            if (m_joyconL.GetButton(button))
            {
                lButton = button;
            }
            if (m_joyconR.GetButton(button))
            {
                rButton = button;
            }
        }

        if (lButton == Joycon.Button.SL || lButton == Joycon.Button.SR) lBrake = true;
        else lBrake = false;
        if (rButton == Joycon.Button.SL || rButton == Joycon.Button.SR) rBrake = true;
        else rBrake = false;


    }


    public float GetControlllerAccel(float magunification)
    {
        float handleValue = 10;
        if (Mathf.Abs(accel.x) < 0.1 || Mathf.Abs(GetControllerSwing()) >= 8) handleValue = 0.0f;
        else handleValue = Mathf.Round(accel.x * magunification);


        return handleValue;
    }

    public float GetControllerSwing()
    {
        return Mathf.Round(gyro.x);
    }

    public bool GetRightBrake()
    {
        return rBrake;
    }

    public bool GetLeftBrake()
    {
        return lBrake;
    }

}
