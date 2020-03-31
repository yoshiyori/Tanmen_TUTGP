using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class InertiaPlayer : MonoBehaviour
{

	//変数
	private Rigidbody rigid;
	[SerializeField] float accelSpeed;
	[SerializeField] float maxSpeed;
	[SerializeField] public float rotaSpeed;
	[SerializeField] float brakeSpeed;
	[SerializeField] float mudSpeed;
	private float TureMaxSpeed;
	public GameObject mud;
	public GameObject junpFlag;
	public GameObject objectPlayer;
	private bool mudTrigger;
	public bool junp;
	private bool willieFlg;
	public bool turnTipe;
	private Vector3 nowSpeed;
	private Vector3 oldSpeed;
	private Vector3 pos;

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
	}

	// Update is called once per frame
	void Update()
    {
	
		mudTrigger = mud.GetComponent<Obstacle>().triggerObsFlag;
		//junp = junpFlag.GetComponent<JunpJudg>().nowJunpFlag;							//サウンド変更部分
		willieFlg = objectPlayer.GetComponent<PlayerDirecting>().willieFlg;
		
		nowSpeed = rigid.velocity;
		pos = this.gameObject.transform.localEulerAngles;

		//ここで速度とかの制御



		if (mudTrigger == true)
		{
			maxSpeed = mudSpeed;
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
		if (Input.GetKey(KeyCode.DownArrow) && rigid.velocity.x<0.1)
		{
			rigid.AddRelativeForce(brakeSpeed,0, 0);
			actionSound.Play("Break");														//サウンド追加分 2/6
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
			Debug.Log(maxSpeed);

		}
		
		oldSpeed = nowSpeed;

		//サウンド追加分 5/6
		if(succesRollingJump)
		{
			actionSound.Play("Rolling");
			succesRollingJump = false;
		}
		if((nowSpeed.magnitude <= 1f) && actionSound.GetAtomSourceStatus(1).ToString().Equals("Playing"))
		{
			actionSound.Stop(1);
		}
		//サウンド追加分 5/6 終了
	}

	void turnPlayer()
	{
		if (Input.GetKey(KeyCode.RightArrow))
		{
			this.gameObject.transform.Rotate(new Vector3(0, rotaSpeed, 0));
			if (turnTipe == true)
			{
				rigid.velocity = Quaternion.Euler(0, rotaSpeed, 0) * rigid.velocity;
			}
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			this.gameObject.transform.Rotate(new Vector3(0, -rotaSpeed, 0));
			if (turnTipe == true)
			{
				rigid.velocity = Quaternion.Euler(0, -rotaSpeed, 0) * rigid.velocity;
			}
		}

	}
	//サウンド追加分 6/6
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag.Equals("Road"))
		{
			if(junp){
				actionSound.Play("Landing");
				junp = false;
			}

			if((nowSpeed.magnitude > 1f) && !actionSound.GetAtomSourceStatus(1).ToString().Equals("Playing"))
			{
				//Debug.Log("Running");
				actionSound.Play("Running", 1);
			}
		}
	}
	
	void OnCollisionExit(Collision other)
	{
		if((other.gameObject.tag.Equals("Road")) && actionSound.GetAtomSourceStatus(1).ToString().Equals("Playing"))
		{
			//Debug.Log("Exit");
			actionSound.Stop(1);
		}
	}
}


// 初期化とか必要な変数をそろえてる
//Vector3 Pold = new Vector3(0.0f,0.0f,0.0f);
//Vector3 Pnew = new Vector3(0.0f, 0.0f, 0.0f);
//Vector3 Vold = new Vector3(0.0f, 0.0f, 0.0f);
//Vector3 Vnew = new Vector3(0.0f, 0.0f, 0.0f);
//Vector3 A;
//Vector3 Direction;
//private float DeltaT = 0.05f;
//private float Accel = 0.0f;
//private bool OneTime = true;
/*	気にしたら負け
		if (OneTime == true)
		{
			Direction = Quaternion.Euler(Pold) * Vector3.right;
			OneTime = false;
		}
		else if (OneTime == false)
		{
			Direction = Quaternion.Euler(Direction) * Vector3.right;
		}


	//ここで慣性計算してるである。（渡辺先生のパクった）

		Direction = Quaternion.Euler(Vnew) * Vector3.right;
		A = Direction * Accel;	

		Pold = Pnew;
		Vold = Vnew;

		Vnew = Vold + A * DeltaT;
		Pnew = Pold + Vold * DeltaT;

		this.gameObject.transform.Translate(Pnew);
		//加速度の初期化
		Accel = 0.0f;

		//押し続けたら加速し続ける、逆もしかり。
		if (Input.GetKey(KeyCode.RightArrow))
		{
			transform.Rotate(new Vector3(0, 15, 0) * Time.deltaTime);

		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			transform.Rotate(new Vector3(0, -15, 0) * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			Accel = Accel - 0.01f;
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			Accel = Accel + 0.03f;
		}
		*/
