using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Transform cameraTrans;
    [SerializeField] Transform playerTrans;
    [SerializeField] Vector3 cameraVec; 
    [SerializeField] Vector3 cameraRot;
    private Vector3 lerpCamera;
    public GameObject player;
    private float rotaSpeed;

    void Start ()
    {
        rotaSpeed = player.GetComponent<InertiaPlayer>().rotaSpeed;
        cameraTrans = transform;  
        cameraTrans.rotation = Quaternion.Euler (cameraRot);
        
    }
    
    private void Update()
    {
       
        
    }
    void LateUpdate()
    {
     /*   if (Input.GetKey(KeyCode.RightArrow))
        {
            // this.gameObject.transform.Rotate(new Vector3(0, rotaSpeed, 0));
            transform.RotateAround(playerTrans.position, Vector3.up, rotaSpeed);

        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // this.gameObject.transform.Rotate(new Vector3(0, -rotaSpeed, 0));
            transform.RotateAround(playerTrans.position, Vector3.up, -rotaSpeed);
        }*/
        cameraTrans.position = Vector3.Lerp(cameraTrans.position, playerTrans.position + cameraVec, 10.0f * Time.deltaTime);
        
      
    }
}