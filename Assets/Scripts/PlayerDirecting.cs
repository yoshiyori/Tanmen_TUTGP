using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirecting : MonoBehaviour
{
	[SerializeField] float willieSpeed;
	[SerializeField] float willieTime;
    public float williChargeTime;
	private Rigidbody rigid;
	public GameObject player;
	public bool willieFlg;
    public bool willieChargeFlag;
	private float startDetaTime;
	private float willieSTime;
	public Animator PlayerAni;
	private bool dorift;
	public bool williOnOff;
	[SerializeField] private CuePlayer playerSound;                 //サウンド追加分 1/2

	[SerializeField] private Handle hd;                             //Joycon関係追加 5/26
	[SerializeField] private GameObject jumpConfirm;
	private JumpingConfirm jcon;                    //緊急のため荒療治（スイングジャンプ中にウィリーしちゃうから追加）
	[SerializeField] private bool checkNowJump;
	// Start is called before the first frame update
	void Start()
    {
		rigid = player.GetComponent<Rigidbody>();
		willieFlg = false;
        willieChargeFlag = false;
		jcon = jumpConfirm.GetComponent<JumpingConfirm>();
	}

    // Update is called once per frame
    void Update()
    {
		checkNowJump = jcon.allSwingJumpFlag;
		startDetaTime = Time.time;
		dorift = player.GetComponent<MovePlayer>().turnTipe;

        if (Mathf.Approximately(Time.timeScale, 0f))
        {
            return;
        }
        if(GameManeger.gameStartFlag == true || GameManeger.goalFlag == true)
        {
            return;
        }

        startDetaTime = Time.time;

		if (willieFlg == true)
		{
			rigid.AddRelativeForce(-willieSpeed, 0, 0);
		}

		//ウィリー
		
		if ( (Input.GetKeyDown(KeyCode.Space)  || hd.GetControllerSwing() >= 8) &&willieFlg == false && checkNowJump == false && dorift == true)
		{
			willieFlg = true;
			willieSTime = startDetaTime;
			this.gameObject.transform.Rotate(new Vector3(-40, 0, 0));
			playerSound.Play("Willy");
			PlayerAni.SetTrigger("Dush");//サウンド追加分 2/2
			williOnOff = true;
		}

		if (startDetaTime > willieTime + willieSTime && willieFlg == true)
		{
            if(willieChargeFlag == false)
            {
                this.gameObject.transform.Rotate(new Vector3(40, 0, 0));
                PlayerAni.SetTrigger("DushEnd");
				williOnOff = false;
            }
            willieChargeFlag = true;
		}

        if (startDetaTime > williChargeTime + willieTime + willieSTime && willieChargeFlag == true)
        {
            willieFlg = false;
            willieSTime = 0;
            willieChargeFlag = false;
        }
		
	}
	
}
