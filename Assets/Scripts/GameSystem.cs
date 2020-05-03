using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : Driver
{
  

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
          //  followCamera.SetAim(myCar.CameraLookAt, myCar.CameraPositions[viewNo]);
        }
    }

}
