using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryControl : MonoBehaviour
{
    public GameObject player;
    [SerializeField] GameObject trajectoryRider;
    [SerializeField] GameObject trajectoryWheel;
    private GameObject trjRiderObj;
    private GameObject trjWheelObj;
    private MovePlayer mp;
    [SerializeField] private bool firstFlag;
    [SerializeField] private bool boostingFlag;
    private Vector3 wheelPosition;

    void Start()
    {
        mp = player.GetComponent<MovePlayer>();
        firstFlag = false;
        boostingFlag = false;
        wheelPosition = new Vector3(0.0f, 1.3f, -2.6f);
    }

    void Update()
    {
        if (mp.swingBoostFlag == true && boostingFlag == false)
        {
            firstFlag = true;
        }
        if (firstFlag)
        {
            
            trjRiderObj = Instantiate(trajectoryRider, this.gameObject.transform.position, Quaternion.identity, this.transform);
            trjWheelObj = Instantiate(trajectoryWheel, this.gameObject.transform.position, Quaternion.identity, this.gameObject.transform);
            firstFlag = false;
            boostingFlag = true;
            
        }

        if (boostingFlag)
        {

        }

        if (mp.swingBoostFlag == false && boostingFlag == true)
        {
            //Destroy(trjRiderObj, 2.0f);
            //Destroy(trjWheelObj, 2.0f);
            boostingFlag = false;
        }


    }

    public bool GetBoostingFlag()
    {
        return boostingFlag;
    } 
}
