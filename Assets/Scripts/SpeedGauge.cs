using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedGauge : MonoBehaviour
{

    [SerializeField] GameObject player;
    private float maxSpeed;
    private float nowSpeed;
    private float nowSpeedGauge;
    [SerializeField] Slider speedGauge;

    // Start is called before the first frame update
    void Start()
    {
        maxSpeed = player.GetComponent<MovePlayer>().swingBoostSpeed;
        speedGauge.maxValue = maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        maxSpeed = player.GetComponent<MovePlayer>().swingBoostSpeed;
        speedGauge.maxValue = maxSpeed;
        nowSpeed = player.GetComponent<Rigidbody>().velocity.magnitude;
        speedGauge.value = nowSpeed;
    }
}
