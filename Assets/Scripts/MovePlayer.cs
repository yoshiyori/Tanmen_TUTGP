using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
	//変数
	private Rigidbody rigid;
	[SerializeField] float accelSpeed;
	[SerializeField] public float maxSpeed;
	[SerializeField] public float rotaSpeed;
	[SerializeField] float brakeSpeed;
	[SerializeField] float mudSpeed;
	public float swingBoostSpeed; //スイングブースト時のスピード（スピードゲージ追加に伴いpublic化）
	[SerializeField] float blerLimit;//この速度からブラーをかける
	public Animator PlayerAni;
	private float TureMaxSpeed;
	public double blerSpeed;
	public GameObject[] mud;
	public GameObject junpFlag;
	public GameObject objectPlayer;
	public GameObject front;
	public bool mudTrigger;
	public bool junp;
	public bool sandControl;
	public bool blerTrigger;
	private bool willieFlg;
	public bool turnTipe;
	private bool willeOnOff;
	private int count = 0;
	public int startLook = 0;
	private int driftCount;
	[System.NonSerialized] public Vector3 nowSpeed; //スピードゲージ追加時にpublic化（Inspector上では見えないように設定済み）
	private Vector3 oldSpeed;
	private Vector3 pos;
	public bool cameraStop;
	public bool swingBoostFlag; //スイングブースト中かどうかのFlag
	public SwingJumpJudge sjj;
	[SerializeField] ParticleSystem concentratedLine; //集中線のパーティクルを入れる
	[SerializeField] GameObject concentratedLineCamera; //集中線を写す専用カメラを入れる
	private bool concentratedLineEndFlag; //集中線終了用フラグ

	//サウンド追加分
	[SerializeField] private CuePlayer actionSound;
	public bool succesRollingJump = false;


	public Handle hd;//JoyConから数値受け取る時とかに使う
	[SerializeField] public bool joyconFlag;//JoyCon使うかどうかのフラグ
	[SerializeField] public float handleSensitivity;
	[SerializeField] HdSensitivity data;

	float time;//ゴール後数秒後に止まるようにするために使う（OC用）

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
		joyconFlag = hd.isConnectHandle;

		actionSound.InitializeAisacControl("Landing");                                  //サウンド追加分 2/8
		handleSensitivity = 0.2f + data.handleSensitivity;
	}

	// Update is called once per frame
	void Update()
	{

		if (Mathf.Approximately(Time.timeScale, 0f))
		{
			return;
		}

		if (GameManeger.gameStartFlag == true)
		{
			PlayerAni.speed = 0;
			return;
		}
		else if (GameManeger.gameStartFlag == false && GameManeger.goalFlag == false)
		{
			PlayerAni.speed = 1;
		}

		if (GameManeger.goalFlag == true)
		{
			time += Time.deltaTime;
            if (rigid.velocity.x > 0)
            {
                //PlayerAni.speed = 0;
            }
            return;
		}

		if (startLook == 0)
		{
			PlayerAni.SetTrigger("Start");
			startLook = 1;
		}

	
		//junp = junpFlag.GetComponent<JunpJudg>().nowJunpFlag;							//サウンド変更部分
		willeOnOff = objectPlayer.GetComponent<PlayerDirecting>().williOnOff;
		willieFlg = objectPlayer.GetComponent<PlayerDirecting>().willieFlg;
		cameraStop = false;
		nowSpeed = rigid.velocity;
		blerSpeed = Math.Sqrt(nowSpeed.x * nowSpeed.x + nowSpeed.z * nowSpeed.z);
		pos = this.gameObject.transform.localEulerAngles;
		maxSpeed = TureMaxSpeed;


		//ここで速度とかの制御

		//ここで速度とかの制御


	

		if (swingBoostFlag == true) //すぃんぐすぴーど実装時追加分
		{
			maxSpeed = swingBoostSpeed;
			PlayerAni.SetBool("Dush", true);
			//集中線出現用処理
			concentratedLineCamera.SetActive(true);
			concentratedLineEndFlag = true;
		}
		else if (swingBoostFlag == false && concentratedLineEndFlag == true) //集中線消去用処理
		{
			maxSpeed = TureMaxSpeed;
			concentratedLine.Clear();
			concentratedLineCamera.SetActive(false);
			PlayerAni.SetBool("Dush", false);
		}
		else if(swingBoostFlag == false && willieFlg == false)
		{
			for (int i = 0; i < mud.Length; i++)
			{
				mudTrigger = mud[i].GetComponent<Obstacle>().triggerObsFlag;
				if (mudTrigger == true)
				{
					maxSpeed = mudSpeed;
					rigid.velocity = rigid.velocity * 0.5f;
					break;
				}
			}

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
			//サウンド追加分 3/8
		}
		if (rigid.velocity.x > 0)
		{
			//rigid.velocity = Vector3.zero;
		}

		if (blerSpeed > blerLimit)
		{
			blerTrigger = true;
		}
		else
		{
			blerTrigger = false;
		}


		if (joyconFlag == true)
		{
			if ((hd.GetRightBrake() == true || hd.GetLeftBrake() == true) && rigid.velocity.x < 0.1)
			{
				rigid.AddRelativeForce(brakeSpeed * 2 / 3, 0, 0);
				actionSound.Play("Break");                                                      //サウンド追加分 4/8
			}
			if ((hd.GetRightBrake() == true && hd.GetLeftBrake() == true) && rigid.velocity.x < 0.1)
			{
				rigid.AddRelativeForce(brakeSpeed, 0, 0);
				actionSound.Play("Break");                                                      //サウンド追加分 5/8
			}
		}

		//回転

		if (Input.GetKey(KeyCode.Z) && willeOnOff == false)
		{
			turnTipe = false;
			driftCount = 0;
		}
		else
		{
			turnTipe = true;
			if (driftCount == 0)
			{
			    Vector3 frontNow = front.transform.position- transform.position;
				//rigid.velocity = Quaternion.Euler(frontNow.normalized) * rigid.velocity;
			}
			driftCount++;
		}
		turnPlayer();


		junpFlag.GetComponent<SwingJumpJudge>().JunpPlayer();

		//サウンド追加分 6/8
		if (succesRollingJump)
		{
			//actionSound.Play("Rolling");
			succesRollingJump = false;
		}
		if ((nowSpeed.magnitude <= 1f) && actionSound.JudgeAtomSourceStatus("Playing", 1))
		{
			actionSound.Stop(1);
		}

		//自転車を漕ぐ音
		if ((nowSpeed.magnitude > 1f) && !actionSound.JudgeAtomSourceStatus("Playing", 1))
		{
			//Debug.Log("Running");
			actionSound.Play("Running", 1);
		}
		else if ((nowSpeed.magnitude < 1f) && actionSound.JudgeAtomSourceStatus("Playing", 1))
		{
			actionSound.Stop(1);
		}
		//サウンド追加分 6/8 終了

		handleSensitivity = 0.2f + data.handleSensitivity;

		if (joyconFlag == true && junp == false)
		{
			//var rot = transform.rotation.eulerAngles;
			//rot.y = hd.GetControlllerAccel(-100);
			//transform.rotation = Quaternion.Euler(rot);
			if (hd.GetControlllerAccel(handleSensitivity, -2.5f) < 0)
			{
				PlayerAni.SetBool("Left", true);
			}
			else if (hd.GetControlllerAccel(handleSensitivity, -2.5f) > 0)
			{
				PlayerAni.SetBool("Right", true);
			}
			this.gameObject.transform.Rotate(new Vector3(0, hd.GetControlllerAccel(handleSensitivity, -2.5f), 0));
			rigid.velocity = Quaternion.Euler(0, hd.GetControlllerAccel(handleSensitivity, -2.5f), 0) * rigid.velocity;
		}
		//確認用
		if (Input.GetKey(KeyCode.Z))
		{
			//Debug.Log(rigid.velocity.magnitude);
			Debug.Log(Math.Sqrt(nowSpeed.x * nowSpeed.x + nowSpeed.z * nowSpeed.z));

		}
		oldSpeed = nowSpeed;

		if (joyconFlag == true)
		{
			//var rot = transform.rotation.eulerAngles;
			//rot.y = hd.GetControlllerAccel(-100);
			//transform.rotation = Quaternion.Euler(rot);
			//this.gameObject.transform.Rotate(new Vector3(0, hd.GetControlllerAccel(0.2f, -5), 0));
		}


		//確認用
		if (Input.GetKey(KeyCode.Z))
		{
			Debug.Log(maxSpeed);
			Debug.Log("x : " + nowSpeed.x + "y : " + nowSpeed.y + "z : " + nowSpeed.z);

		}
		oldSpeed = nowSpeed;
	}



	void turnPlayer()
	{
		bool check = false;

		/*if (rigid.velocity.magnitude < maxSpeed / 3)
		{
			

		}*/
		if (Input.GetKey(KeyCode.RightArrow))
		{
			check = true;
			this.gameObject.transform.Rotate(new Vector3(0, rotaSpeed, 0));
			if (turnTipe == true)
			{
				rigid.velocity = Quaternion.Euler(0, rotaSpeed, 0) * rigid.velocity;
			}
			PlayerAni.SetBool("Right", true);
		}
		else
		{
			PlayerAni.SetBool("Right", false);
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			check = true;
			this.gameObject.transform.Rotate(new Vector3(0, -rotaSpeed, 0));
			if (turnTipe == true)
			{
				rigid.velocity = Quaternion.Euler(0, -rotaSpeed, 0) * rigid.velocity;
			}
			PlayerAni.SetBool("Left", true);
		}
		else
		{
			PlayerAni.SetBool("Left", false);
		}
		/*else if (rigid.velocity.magnitude > maxSpeed / 3 && rigid.velocity.magnitude <= maxSpeed * 2 / 3)
		{


			if (count < 60)
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
					PlayerAni.SetBool("Right", true);
				}
				else
				{
					PlayerAni.SetBool("Right", false);
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
					PlayerAni.SetBool("Left", true);
				}
				else
				{
					PlayerAni.SetBool("Left", false);
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
					PlayerAni.SetBool("Right", true);
				}
				else
				{
					PlayerAni.SetBool("Right", false);
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
					PlayerAni.SetBool("Left", true);
				}
				else
				{
					PlayerAni.SetBool("Left", false);
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

		}*/



		if (check == false)
		{
			count = 0;
			//Debug.Log("動いた");
		}

		//Debug.Log(check);
		//Debug.Log(count);
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag.Equals("Road"))
		{
			sandControl = true;
			postureControl();
			//サウンド追加分 7/8
			//着地音
			if (junp)
			{
				actionSound.SetRandomAisacControl("GroundRandomizer_Pitch");
				actionSound.SetRandomAisacControl("GroundRandomizer_Filter");
				actionSound.Play("Landing");
				junp = false;
			}

			//走行音の切り替え
			actionSound.SetAisacControl("Landing", 0f, 1);
			//サウンド追加分 7/8 終了
			PlayerAni.SetBool("Junp", false);
		}

	}


	void OnCollisionExit(Collision other)
	{
		if (other.gameObject.tag.Equals("Road"))
		{
			sandControl = false;
			actionSound.SetAisacControl("Landing", 1f, 1);          //サウンド追加分 8/8
		}
		PlayerAni.SetBool("Junp", true);
	}

	public bool GetSandCtrl()   //スイングブースト実装時追加
	{
		return sandControl;
	}
	void postureControl()
	{
		RaycastHit hit;
		if (Physics.Raycast(
				   transform.position,
				   -transform.up,
				   out hit,
				   float.PositiveInfinity))
		{
			Quaternion q = Quaternion.FromToRotation(
						transform.up,
						hit.normal);

			transform.rotation *= q;

		}
	}
}