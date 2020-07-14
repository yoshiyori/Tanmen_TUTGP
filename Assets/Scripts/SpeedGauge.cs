using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedGauge : MonoBehaviour
{

    [SerializeField] GameObject player;
    private float nowSpeed;
    private float nowSpeedGauge;
    [SerializeField] Slider speedGauge;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nowSpeed = player.GetComponent<Rigidbody>().velocity.magnitude;
        speedGauge.value = nowSpeed;
    }
}
