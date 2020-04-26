using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngineInternal;

public class SwingGauge : MonoBehaviour
{
    [SerializeField] Handle hd;
    [SerializeField] private Slider swingGauge;
    //[SerializeField] int point;
    [SerializeField] private int modeSelectNum;

    private bool isPlus;

    private Material mat;
    private Color red;
    private Color blue;
    private Color green;
    private Color white;

    void Start()
    {
        mat = this.GetComponent<Renderer>().material;
        swingGauge.value = 0.0f;
        red = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        blue = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        green = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        white = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        isPlus = false;
    }

    void Update()
    {
        ChargeGauge(modeSelectNum);
    }

    void ChargeGauge(int argModeSelectNum)
    {
        switch (argModeSelectNum)
        {
            case 0:
                if (hd.GetCycleSwing(0) > 2.5)
                {
                    if (isPlus == false)
                    {
                        swingGauge.value += 0.05f;
                        isPlus = true;
                    }
                }
                if (hd.GetCycleSwing(0) < -2.5)
                {
                    if (isPlus == true)
                    {
                        swingGauge.value += 0.05f;
                        isPlus = false;
                    }

                }
                break;
            case 1:





                break;
            default:
                break;
        }

        
    }
}
