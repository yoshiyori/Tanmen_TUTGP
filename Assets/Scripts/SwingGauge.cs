using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngineInternal;

public class SwingGauge : MonoBehaviour
{
    [SerializeField] Handle hd;
    [SerializeField] private Slider swingGauge;
    [SerializeField] private int modeSelectNum;
    [SerializeField] private float upNum;


    private bool isPlus;

    void Start()
    {
        swingGauge.value = 0.0f;
        isPlus = false;
        if (upNum == 0.0f)
        {
            upNum = 0.05f;
        }
    }

    void Update()
    {
        ChargeGauge(modeSelectNum);
    }

    void ChargeGauge(int argModeSelectNum)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            swingGauge.value = 0.0f;
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
                if ( (hd.GetControlllerAccel(1) <= 0.1 && hd.GetControlllerAccel(1) >= 0.0) ||
                    (hd.GetControlllerAccel(1) >= -0.1 && hd.GetControlllerAccel(1) <= 0.0) )
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
}
