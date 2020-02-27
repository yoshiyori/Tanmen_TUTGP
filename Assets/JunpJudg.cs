using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunpJudg : MonoBehaviour
{
    [SerializeField] float junpAccelSpeed;
    public bool triggerObsFlag;
    public bool nowJunpFlag;
    private Rigidbody rigid;
    public GameObject playerObject;
    public float junpSpeed;
    private Vector3 nowSpeed;
    private Vector3 playerPosition;

    private void Start()
    {
        triggerObsFlag = false;
        rigid = playerObject.GetComponent<Rigidbody>();
        playerPosition = playerObject.transform.position;
    }

    private void Update()
    {
        if (triggerObsFlag == true && Input.GetKey(KeyCode.S))//ここのInputを振り上げ
        {
            rigid.AddForce(0, junpSpeed, 0);
            triggerObsFlag = false;
            nowJunpFlag = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            triggerObsFlag = true;         
            nowSpeed = rigid.velocity;
        }
        if (playerPosition.y < 1.3)
        {
            nowJunpFlag = false;
        }

    }
    public void JunpPlayer()
    {

        if (nowJunpFlag == true && Input.GetKey(KeyCode.A))//ここのInputを振り下ろしにして
        {
            rigid.AddRelativeForce(-junpAccelSpeed, 0, 0);
            nowJunpFlag = false;
            Debug.Log("JunpAcccel");
        }

    }
}
