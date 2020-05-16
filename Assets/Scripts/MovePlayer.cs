using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
	//変数
	private Rigidbody rigid;
	[SerializeField] float accelSpeed;
	[SerializeField] float maxSpeed;
	[SerializeField] public float rotaSpeed;
	[SerializeField] float brakeSpeed;
	[SerializeField] float mudSpeed;
	[SerializeField] float swingBoostSpeed;	//スイングブースト時のスピード
	private float TureMaxSpeed;
	public GameObject mud;
	public GameObject junpFlag;
	public GameObject objectPlayer;
	private bool mudTrigger;
	public bool junp;
	public bool sandControl;
	private bool willieFlg;
	public bool turnTipe;
	private int count = 0;
	private Vector3 nowSpeed;
	private Vector3 oldSpeed;
	private Vector3 pos;
	public bool cameraStop;
	public bool swingBoostFlag;	//スイングブースト中かどうかのFlag


	//サウンド追加分 1/6
	[SerializeField] private CuePlayer actionSound;
	public bool succesRollingJump = false;
	//サウンド追加分 1/6 終了


	public Handle hd;//JoyConから数値受け取る時とかに使う
	[SerializeField] public bool joyconFlag;//JoyCon使うかどうかのフラグ

	void Awake()
	{
		//FPSを手動で固定
		Application.targetFrameRate = 60;
	}

	private void Start()
	{
		//rigidbodyの取得
		rigid = GetComponent<Rigidbody>();
		TureMaxSpeed = maxSpeed;
		sandControl = false;

	}

	// Update is called once per frame
	void Update()
	{

		mudTrigger = mud.GetComponent<Obstacle>().triggerObsFlag;
		//junp = junpFlag.GetComponent<JunpJudg>().nowJunpFlag;							//サウンド変更部分
		willieFlg = objectPlayer.GetComponent<PlayerDirecting>().willieFlg;
		cameraStop = false;
		nowSpeed = rigid.velocity;
		pos = this.gameObject.transform.localEulerAngles;
		
		//ここで速度とかの制御



		if (mudTrigger == true)
		{
			maxSpeed = mudSpeed;
		}
		else if (swingBoostFlag == true) //すぃんぐすぴーど実装時追加分
		{
			maxSpeed = swingBoostSpeed;
		}
		else
		{
			maxSpeed = TureMaxSpeed;
		}

		if (rigid.velocity.magnitude < maxSpeed)
		{
			rigid.AddRelativeForce(-accelSpeed, 0, 0);
		}
		else if (maxSpeed < -nowSpeed.x)
		{
			if (mudTrigger == false)
			{
				nowSpeed.x = oldSpeed.x - 10;
			}
			else
			{
				nowSpeed.x = oldSpeed.x - 100000000;
			}
		}
		if (Input.GetKey(KeyCode.DownArrow) && rigid.velocity.x < 0.1)
		{
			rigid.AddRelativeForce(brakeSpeed, 0, 0);
			cameraStop = true;
			actionSound.Play("Break");
			//サウンド追加分 2/6
		}
		if (rigid.velocity.x > 0)
		{
			//rigid.velocity = Vector3.zero;
		}

		if (joyconFlag == true)
		{
			if ((hd.GetRightBrake() == true || hd.GetLeftBrake() == true) && rigid.velocity.x < 0.1)
			{
				rigid.AddRelativeForce(brakeSpeed * 2 / 3, 0, 0);
				actionSound.Play("Break");                                                      //サウンド追加分 3/6
			}
			if ((hd.GetRightBrake() == true && hd.GetLeftBrake() == true) && rigid.velocity.x < 0.1)
			{
				rigid.AddRelativeForce(brakeSpeed, 0, 0);
				actionSound.Play("Break");                                                      //サウンド追加分 4/6
			}
		}

		//回転
		
		turnPlayer();
		
		
		junpFlag.GetComponent<JunpJudg>().JunpPlayer();

		//サウンド追加分 5/6
		if(succesRollingJump)
		{
			actionSound.Play("Rolling");
			succesRollingJump = false;
		}
		if((nowSpeed.magnitude <= 1f) && actionSound.JudgeAtomSourceStatus("Playing", 1))
		{
			actionSound.Stop(1);
		}
		//サウンド追加分 5/6 終了

		if (joyconFlag == true)
		{
			//var rot = transform.rotation.eulerAngles;
			//rot.y = hd.GetControlllerAccel(-100);
			//transform.rotation = Quaternion.Euler(rot);
			this.gameObject.transform.Rotate(new Vector3(0, hd.GetControlllerAccel(-5), 0));
		}


		//確認用
		if (Input.GetKey(KeyCode.Z))
		{
			Debug.Log(rigid.velocity);
		
		}
		oldSpeed = nowSpeed;


	}

	void turnPlayer()
	{
		bool check = false;

		if (rigid.velocity.magnitude < maxSpeed / 3)
		{
			if (Input.GetKey(KeyCode.RightArrow))
			{
				check = true;
				this.gameObject.transform.Rotate(new Vector3(0, rotaSpeed, 0));
				if (turnTipe == true)
				{
					rigid.velocity = Quaternion.Euler(0, rotaSpeed, 0) * rigid.velocity;
				}
			}
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				check = true;
				this.gameObject.transform.Rotate(new Vector3(0, -rotaSpeed, 0));
				if (turnTipe == true)
				{
					rigid.velocity = Quaternion.Euler(0, -rotaSpeed, 0) * rigid.velocity;
				}
			}
			
		}
		else if (rigid.velocity.magnitude > maxSpeed / 3 && rigid.velocity.magnitude <= maxSpeed * 2 / 3)
		{


			if (count <60)
			{
				if (Input.GetKey(KeyCode.RightArrow))
				{
					check = true;
					this.gameObject.transform.Rotate(new Vector3(0, rotaSpeed, 0));
					if (turnTipe == true)
					{
						rigid.velocity = Quaternion.Euler(0, rotaSpeed, 0) * rigid.velocity;
					}
					count++;
				}
				if (Input.GetKey(KeyCode.LeftArrow))
				{
					check = true;
					this.gameObject.transform.Rotate(new Vector3(0, -rotaSpeed, 0));
					if (turnTipe == true)
					{
						rigid.velocity = Quaternion.Euler(0, -rotaSpeed, 0) * rigid.velocity;
					}
					count++;
				}
				
			}
			else
			{
				cameraStop = true;
				if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
				{
					check = true;
				}
			}
			
		}
		else if (rigid.velocity.magnitude >= maxSpeed * 2 / 3)
		{
			if (count < 20)
			{
				if (Input.GetKey(KeyCode.RightArrow))
				{
					check = true;
					this.gameObject.transform.Rotate(new Vector3(0, rotaSpeed, 0));
					if (turnTipe == true)
					{
						rigid.velocity = Quaternion.Euler(0, rotaSpeed, 0) * rigid.velocity;
					}
					count++;
				}
				if (Input.GetKey(KeyCode.LeftArrow))
				{
					check = true;
					this.gameObject.transform.Rotate(new Vector3(0, -rotaSpeed, 0));
					if (turnTipe == true)
					{
						rigid.velocity = Quaternion.Euler(0, -rotaSpeed, 0) * rigid.velocity;
					}
					count++;
				}
				
			}
			else
			{
				cameraStop = true;
				if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
				{
					check = true;
				}
			}
			
		}	
		
		if(check == false)
		{
			count = 0;	
			//Debug.Log("動いた");
		}

		//Debug.Log(check);
		//Debug.Log(count);
	}

	//サウンド追加分 6/6
	void OnCollisionEnter(Collision other)
	{
		
		if (other.gameObject.tag.Equals("Road"))
		{
			sandControl = true;
			if (junp){
				actionSound.Play("Landing");
				junp = false;
			}

			if((nowSpeed.magnitude > 1f) && !actionSound.JudgeAtomSourceStatus("Playing", 1))
			{
				//Debug.Log("Running");
				actionSound.Play("Running", 1);
			}
			
		}
	}

	void OnCollisionExit(Collision other)
	{
		if (other.gameObject.tag.Equals("Road"))
		{
			sandControl = false;
		}
		if ((other.gameObject.tag.Equals("Road")) && actionSound.JudgeAtomSourceStatus("Playing", 1))
		{
			//Debug.Log("Exit");
			actionSound.Stop(1);
		}
	}
	//サウンド追加分 6/6 終了

	public bool GetSandCtrl()	//スイングブースト実装時追加
	{
		return sandControl;
	}
}