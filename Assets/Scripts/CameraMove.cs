using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public GameObject AimLookAt;
    GameObject AimPosition;

    Camera currentCamera;
    float time;

    // Use this for initialization
    void Start()
    {
        currentCamera = GetComponent<Camera>();
        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (AimLookAt == null || AimPosition == null)
        {
            return;
        }

        // 現在位置から目的位置へ補間
        time += Time.deltaTime * 0.5f;
        Vector3 pos = transform.position;
        pos = Vector3.Lerp(pos, AimPosition.transform.position, time);
        transform.position = pos;

        transform.LookAt(AimLookAt.transform.position, Vector3.up);
    }

    public void SetAim(GameObject lookat, GameObject position)
    {
        AimLookAt = lookat;
        AimPosition = position;
        time = 0.0f;
    }
  

}

/*  Transform cameraTrans;
    [SerializeField] Transform playerTrans;
    [SerializeField] Vector3 cameraVec; 
    [SerializeField] Vector3 cameraRot;
    private Vector3 lerpCamera;
    public GameObject player;
    private float rotaSpeed;
    public int tipe;
    private bool joyconFlag;
    private bool stop;
    public Handle hd;

    void Start ()
    {
        if (tipe == 0)
        {
            rotaSpeed = player.GetComponent<InertiaPlayer>().rotaSpeed;
            cameraTrans = transform;
            cameraTrans.rotation = Quaternion.Euler(cameraRot);
            joyconFlag = player.GetComponent<InertiaPlayer>().joyconFlag;
        }
        if (tipe == 1)
        {
            rotaSpeed = player.GetComponent<MovePlayer>().rotaSpeed;
            cameraTrans = transform;
            cameraTrans.rotation = Quaternion.Euler(cameraRot);
            joyconFlag = player.GetComponent<MovePlayer>().joyconFlag;

        }
        if (tipe == 2)
        {
            rotaSpeed = 2;
            cameraTrans = transform;
            cameraTrans.rotation = Quaternion.Euler(cameraRot);
            joyconFlag = false;

        }

    }

    private void Update()
    {
        if (tipe == 1)
        {
            stop = player.GetComponent<MovePlayer>().cameraStop;
        }
    }
    void LateUpdate()
    {
        if (tipe != 2)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                // this.gameObject.transform.Rotate(new Vector3(0, rotaSpeed, 0));
                //transform.RotateAround(playerTrans.position, Vector3.up, rotaSpeed);
                queue(cameraVec, -rotaSpeed);
                cameraTrans.LookAt(playerTrans.position);

            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                // this.gameObject.transform.Rotate(new Vector3(0, -rotaSpeed, 0));
                // transform.RotateAround(playerTrans.position, Vector3.up, -rotaSpeed);
                queue(cameraVec, rotaSpeed);
                cameraTrans.LookAt(playerTrans.position);
            }

            if (joyconFlag == true && hd.GetControlllerAccel(10) != 0.0f)
            {
                queue(cameraVec, hd.GetControlllerAccel(5));
                cameraTrans.LookAt(playerTrans.position);

                if (leftRightNum > 0)
                {
                    queue(cameraVec, leftRightNum);
                    cameraTrans.LookAt(playerTrans.position);
                }
                else if (leftRightNum < 0)
                {
                    queue(cameraVec, leftRightNum);
                    cameraTrans.LookAt(playerTrans.position);
                }

            }
        }

            cameraTrans.position = Vector3.Lerp(cameraTrans.position, playerTrans.position + cameraVec, 10.0f * Time.deltaTime);


    }
    void queue(Vector3 arg_Vec,float arg_Rote)
    {
        if (stop == false)
        {
            float X, Y;

            X = Mathf.Cos(arg_Rote / 60) * arg_Vec.x + -Mathf.Sin(arg_Rote / 60) * arg_Vec.z;
            Y = Mathf.Sin(arg_Rote / 60) * arg_Vec.x + Mathf.Cos(arg_Rote / 60) * arg_Vec.z;


            cameraVec = new Vector3(X, arg_Vec.y, Y);
        }

    }*/
