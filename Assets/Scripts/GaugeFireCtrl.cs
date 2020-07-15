using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeFireCtrl : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireParticle;
    [SerializeField] private Slider swingGauge;
    [SerializeField] private GameObject swingBoostText;
    private Color fireBaseColor;
    private Color fireColorYellow;
    private Color fireColorGreen;
    private Color fireColorBlue;
    private Color perClear;
    [SerializeField] bool fireColorChangeFlag;
    [SerializeField] float particleMinGravityNum;
    [SerializeField] float particleMaxGravityNum;
    [SerializeField] float particleRadiusNum;
    [SerializeField] float particleEmissionRotNum;//Rate Over Time = Rot

    void Start()
    {
        ParticleSystem.MainModule parpar = GetComponent<ParticleSystem>().main;
        fireColorYellow = new Color(1.0f, 1.0f, 0.11f, 1.0f);
        fireColorGreen = new Color(0.25f, 1.0f, 0.11f, 1.0f);
        fireColorBlue = new Color(0.25f, 0.26f, 1.0f, 1.0f);
        fireBaseColor = new Color(1.0f, 0.345f, 0.102f) ;
        perClear = new Color(0, 0, 0, 0);

        if (particleEmissionRotNum == 0.0f) particleEmissionRotNum = 150f;
        if (particleMaxGravityNum == 0.0f) particleMaxGravityNum = -0.6f;
        if (particleMinGravityNum == 0.0f) particleMinGravityNum = -0.9f;
        if (particleRadiusNum == 0.0f) particleRadiusNum = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireColorChangeFlag)
        {
            ChangeFireColor();
        }
        ChangeFireSize();

    }

    private void ChangeFireColor()
    {
        ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
        color.mode = ParticleSystemGradientMode.Color;

        if (swingBoostText.activeInHierarchy == false || swingGauge.value == 0)
        {
            color.color = perClear;
        }
        else if (swingGauge.value < 0.3)
        {
            color.color = fireColorYellow;//Yellow
        }
        else if (swingGauge.value < 0.65)
        {
            color.color = fireColorGreen; //Green
        }
        else
        {
            color.color = fireColorBlue;//Blue
        }

        ParticleSystem.MainModule main = fireParticle.main;
        main.startColor = color;
    }

    private void ChangeFireSize()
    {
        //ParticleSystem.MinMaxCurve maxGravityNum = new ParticleSystem.MinMaxCurve();
        //ParticleSystem.MinMaxCurve minGravityNum = new ParticleSystem.MinMaxCurve();
        //maxGravityNum.mode = ParticleSystemCurveMode.TwoConstants;
        //minGravityNum.mode = ParticleSystemCurveMode.TwoConstants;

        //maxGravityNum = swingGauge.value * -0.9f;
        //minGravityNum = swingGauge.value * -0.6f;

        ParticleSystem.MainModule main = fireParticle.main;
        ParticleSystem.ShapeModule shape = fireParticle.shape;
        ParticleSystem.EmissionModule emission = fireParticle.emission;
        main.gravityModifier = new ParticleSystem.MinMaxCurve(swingGauge.value * particleMaxGravityNum, swingGauge.value * particleMinGravityNum);
        shape.radius = swingGauge.value * particleRadiusNum;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(swingGauge.value * particleEmissionRotNum);

        if (swingBoostText.activeInHierarchy == false && swingGauge.value < 0.6)
        {
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.1f, 0.1f);
        }
        else
        {
            main.startLifetime = new ParticleSystem.MinMaxCurve(1.0f, 1.5f);
        }
        


    }


}
