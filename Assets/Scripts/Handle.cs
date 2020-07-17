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
    private Joycon.Button? rButtonUp;
    private Joycon.Button? lButtonUp;
    private bool rSLR;
    private bool lSLR;
    private bool rSLRDown;
    private bool lSLRDown;
    private bool rSLRPressCheckFlag;
    private bool lSLRPressCheckFlag;
    private bool ZR;
    private bool ZL;
    private bool ZRDown;
    private bool ZLDown;
    private bool ZRPressCheckFlag;
    private bool ZLPressCheckFlag;


    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];


    //[SerializeField] bool rlCheck;//checkの場合right

    public bool isConnectHandle;

    [SerializeField] float HandleValueCheck;

    private void Start()
    {
        var joycons = JoyconManager.Instance.j;
        m_joyconR = joycons.Find(c => !c.isLeft);
        m_joyconL = joycons.Find(c => c.isLeft);

        rSLR = false;
        lSLR = false;
        rSLRDown = false;
        lSLRDown = false;
        rSLRPressCheckFlag = false;
        lSLRPressCheckFlag = false;
        ZR = false;
        ZL = false;

        if (m_joyconR != null || m_joyconL != null)
        {
            isConnectHandle = true;
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
                if (m_joyconL.GetButtonUp(button))
                {
                    lButtonUp = button;
                }
                if (m_joyconR.GetButtonUp(button))
                {
                    rButtonUp = button;
                }

            }
        }
        else
        {
            isConnectHandle = false;
        }

    }

    private void Update()
    {
        if (m_joyconR == null || m_joyconL == null) return;
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

        if (lButton == Joycon.Button.SHOULDER_2)
        {
            ZL = true;
            if (ZLPressCheckFlag == false)
            {
                ZLPressCheckFlag = true;
                ZLDown = true;
            }
            else
            {
                ZLDown = false;
            }
        }
        else
        {
            ZL = false;
            ZLDown = false;
        }

        if (rButton == Joycon.Button.SHOULDER_2)
        {
            ZR = true;
            if (ZRPressCheckFlag == false)
            {
                ZRPressCheckFlag = true;
                ZRDown = true;
            }
            else
            {
                ZRDown = false;
            }
        }
        else
        {
            ZR = false;
            ZRDown = false;
        }



        if (lButton == Joycon.Button.SL || lButton == Joycon.Button.SR)
        {
            lSLR = true;
            if (lSLRPressCheckFlag == false)
            {
                lSLRPressCheckFlag = true;
                lSLRDown = true;
            }
            else
            {
                lSLRDown = false;
            }
        }
        else
        {
            lSLR = false;
            lSLRDown = false;
        }
        if (rButton == Joycon.Button.SL || rButton == Joycon.Button.SR)
        {
            rSLR = true;
            if (rSLRPressCheckFlag == false)
            {
                rSLRDown = true;
                rSLRPressCheckFlag = true;
            }
            else
            {
                rSLRDown = false;
            }
        }
        else
        {
            rSLR = false;
            rSLRDown = false;
        }

        

        if (lButton == null && lSLRPressCheckFlag == true)
        {
            lSLRPressCheckFlag = false;
        }
        if (rButton == null && lSLRPressCheckFlag == true)
        {
            rSLRPressCheckFlag = false;
        }

        if (lButton == null && ZRPressCheckFlag == true)
        {
            ZRPressCheckFlag = false;
        }
        if (rButton == null && ZLPressCheckFlag == true)
        {
            ZLPressCheckFlag = false;
        }

    }


    public float GetControlllerAccel(float magunification)
    {
        float handleValue = 10;
        if (Mathf.Abs(accel.x) < 0.1 || Mathf.Abs(GetControllerSwing()) >= 8) handleValue = 0.0f;
        else handleValue = Mathf.Round(accel.x * magunification);

        HandleValueCheck = handleValue;
        return handleValue;
    }

    public float GetControllerSwing()
    {
        return Mathf.Round(gyro.x);
    }

    public bool GetRightBrake()
    {
        return lSLR;
    }

    public bool GetLeftBrake()
    {
        return rSLR;
    }

    public bool GetRightBrakeDown()
    {
        return lSLRDown;
    }

    public bool GetLeftBrakeDown()
    {
        return rSLRDown;
    }

    public void JoyconRumble(int checkLRNum, float lowFleq, float highFleq, float amp, int time)
    {
        if (checkLRNum == 0)
        {
            if(isConnectHandle) m_joyconL.SetRumble(lowFleq, highFleq, amp, time);
        }
        else
        {
            if(isConnectHandle) m_joyconR.SetRumble(lowFleq, highFleq, amp, time);
        }
    }

    public float GetCycleSwing(int selectNum)
    {
        if (selectNum == 0)
        {
            return Mathf.Round(gyro.x);
        }
        else if (selectNum == 1)
        {
            return Mathf.Round(gyro.y);
        }
        else
        {
            return Mathf.Round(gyro.z);
        }
    }

    public bool GetZR()
    {
        return ZR;
    }

    public bool GetZL()
    {
        return ZL;
    }
    public bool GetZRDown()
    {
        return ZRDown;
    }

    public bool GetZLDown()
    {
        return ZLDown;
    }

}
