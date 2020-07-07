using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuraChanging : MonoBehaviour
{
    [SerializeField] private ParticleSystem par_exParticle;
    [SerializeField] private ParticleSystem parParticle;
    [SerializeField] private Slider swingGauge;
    [SerializeField] private GameObject swingBoostText;
    private Color perColorYellow;
    private Color perColorGreen;
    private Color perColorBlue;
    private Color perClear;

    void Start()
    {
        ParticleSystem.MainModule parpar = GetComponent<ParticleSystem>().main;
        perColorYellow = new Color(1.0f, 1.0f, 0.11f, 1.0f);
        perColorGreen = new Color(0.25f, 1.0f, 0.11f, 1.0f);
        perColorBlue = new Color(0.25f, 0.26f, 1.0f, 1.0f);
        perClear = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (swingBoostText.activeInHierarchy == false || swingGauge.value == 0)
        {
            ChangeAuraColor(-1);
        }
        else if (swingGauge.value < 0.3)
        {
            ChangeAuraColor(0);//Yellow
        }
        else if (swingGauge.value < 0.65)
        {
            ChangeAuraColor(1);//Green
        }
        else
        {
            ChangeAuraColor(2);//Blue
        }

        ChangeAuraSize(swingGauge.value);

    }

    private void ChangeAuraColor(int selectColorNum)//0=yellow,1=green,2=blue
    {
        ParticleSystem.MinMaxGradient color = new ParticleSystem.MinMaxGradient();
        color.mode = ParticleSystemGradientMode.Color;
        switch (selectColorNum)
        {
            case 0:
                color.color = perColorYellow;
                break;
            case 1:
                color.color = perColorGreen;
                break;
            case 2:
                color.color = perColorBlue;
                break;
            default:
                color.color = perClear;
                break;
        }

        ParticleSystem.MainModule main = parParticle.main;
        ParticleSystem.MainModule main2 = par_exParticle.main;
        main.startColor = color;
        main2.startColor = color;
    }

    private void ChangeAuraSize(float value)
    {
        var emission = par_exParticle.emission;
        emission.rateOverTime = 50f + (value * 50f);

        ParticleSystem.MainModule main = parParticle.main;
        ParticleSystem.MainModule main2 = par_exParticle.main;
        main.gravityModifier = -0.1f + (value);

        var shape = par_exParticle.shape;
        shape.radius = 0.7f + (1.3f * value);
    }

}
