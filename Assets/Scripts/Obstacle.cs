using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public bool triggerObsFlag;
    [SerializeField] private bool triggerTypeKakunin;//mud:false, cone:true

    private void Start()
    {
        triggerObsFlag = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            triggerObsFlag = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && triggerTypeKakunin == false)
        {
            triggerObsFlag = false;
        }
    }

}
