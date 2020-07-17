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

    //サウンド追加分 1/3
    [SerializeField] CuePlayer2D soundManager;
    private float aisacControlVal;
    private CriAtomExPlayback exPlayback;

    // Start is called before the first frame update
    void Start()
    {
        //サウンド追加分 2/3
        exPlayback = soundManager.Play("Wind");
    }

    // Update is called once per frame
    void Update()
    {
        nowSpeed = player.GetComponent<Rigidbody>().velocity.magnitude;
        speedGauge.value = nowSpeed;

        //サウンド追加分 3/3
        soundManager.SetAisacControl("Wind", 0f + (1f - 0f) * ((nowSpeed - 0f) / (60f - 0f)));
        soundManager.UpdateCuePlayer2D(exPlayback);
    }
}
