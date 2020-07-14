using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WillieCheck : MonoBehaviour
{
    [SerializeField] PlayerDirecting playerDirecting;
    [SerializeField] GameObject goUI;
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
            goUI.SetActive(false);
            chargeTime = playerDirecting.williChargeTime + 1;
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
            goUI.SetActive(true);
        }
    }
}
