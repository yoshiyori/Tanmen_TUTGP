using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDriver : Driver
{

    public UnityEngine.UI.Slider sliderSteering;
    public UnityEngine.UI.Slider sliderEngineTorqu;
    public UnityEngine.UI.Button buttonSwitchView;
    public UnityEngine.UI.Button buttonBreak;

    public Camera MainCamera;
    CameraMove followCamera;

    int currentView;

    // Use this for initialization
    void Start()
    {

        if (MainCamera != null)
        {
            followCamera = MainCamera.GetComponent<CameraMove>();
        }

        if (sliderSteering != null)
        {
            sliderSteering.onValueChanged.AddListener(delegate (float newValue) {
                // ステアリングのスライダーが更新されたとき
                if (myCar != null)
                {
                    myCar.steering = newValue;
                }
            });
        }

        if (sliderEngineTorqu != null)
        {
            sliderEngineTorqu.onValueChanged.AddListener(delegate (float newValue) {
                // エンジントルクのスライダーが更新されたとき
                if (myCar != null)
                {
                    myCar.engineTorqu = newValue;
                }
            });
        }

        if (buttonBreak != null)
        {
            buttonBreak.onClick.AddListener(delegate {
                // ブレーキを踏んだ
                if (myCar != null)
                {
                    myCar.breakTorqu = 1.0f;
                }
            });
        }

        if (buttonSwitchView != null)
        {
            buttonSwitchView.onClick.AddListener(delegate {
                // 視点切り替え
                currentView++;
                if (currentView >= myCar.CameraPositions.Length)
                {
                    currentView = 0;
                }
                SwitchView(currentView);
            });
        }

        currentView = 1;
        SwitchView(currentView);

    }

    // Update is called once per frame
    void Update()
    {
        if (myCar != null && myCar.breakTorqu > 0.0f)
        {
            myCar.breakTorqu -= Time.deltaTime;
            if (myCar.breakTorqu < 0.0f)
            {
                myCar.breakTorqu = 0.0f;
            }
        }
    }

    public void SwitchView(int viewNo)
    {
        if (viewNo < 0)
        {
            viewNo = 0;
        }
        else if (viewNo >= myCar.CameraPositions.Length)
        {
            viewNo = myCar.CameraPositions.Length - 1;
        }
        if (followCamera != null && myCar.CameraPositions[viewNo] != null)
        {
            followCamera.SetAim(myCar.CameraLookAt, myCar.CameraPositions[viewNo]);
        }
    }

}
