using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WillieCheck : MonoBehaviour
{
    [SerializeField] Image willieCheck;
    [SerializeField] PlayerDirecting playerDirecting;
    [SerializeField] Color willieGoColor;
    [SerializeField] GameObject goText;
    [SerializeField] Color willieUseChargeColor;
    [SerializeField] GameObject chargeTimeText;
    float chargeTime;
    int seconds;

    // Start is called before the first frame update
    void Start()
    {
        chargeTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerDirecting.willieFlg == true && playerDirecting.willieChargeFlag == false)
        {
            goText.SetActive(false);
            willieCheck.color = willieUseChargeColor;
            chargeTime = playerDirecting.williChargeTime;
        }
        else if (playerDirecting.willieFlg == true && playerDirecting.willieChargeFlag == true)
        {
            chargeTimeText.SetActive(true);
            chargeTime -= Time.deltaTime;
            seconds = (int)chargeTime;
            chargeTimeText.GetComponent<Text>().text = seconds.ToString();
        }
        else
        {
            chargeTimeText.SetActive(false);
            chargeTime = 0f;
            goText.SetActive(true);
            willieCheck.color = willieGoColor;
        }
    }
}
