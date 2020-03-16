using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InertiaPlayer : MonoBehaviour
{
	

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
	//変数
	private Rigidbody rigid;
	[SerializeField] float accelSpeed;
	[SerializeField] float maxSpeed;
	[SerializeField] public float rotaSpeed;
	[SerializeField] float brakeSpeed;
	public GameObject mud;
	public GameObject junpFlag;
	public GameObject objectPlayer;
	private bool mudTrigger;
	public bool junp;
	private bool willieFlg;
	private Vector3 nowSpeed;
	private Vector3 oldSpeed;
	private Vector3 pos;

	//サウンド追加分 1/4
	[SerializeField] private CriAtomSource playerSound;
	[SerializeField] private CriAtomSource runningSound;
	public bool succesRollingJump = false;


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
	
	}

	// Update is called once per frame
	void Update()
    {
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
		mudTrigger = mud.GetComponent<Obstacle>().triggerObsFlag;
		//junp = junpFlag.GetComponent<JunpJudg>().nowJunpFlag;							//サウンド変更部分
		willieFlg = objectPlayer.GetComponent<PlayerDirecting>().willieFlg;
		
		nowSpeed = rigid.velocity;
		pos = this.gameObject.transform.localEulerAngles;

		//ここで速度とかの制御

		if (mudTrigger == false && rigid.velocity.magnitude < maxSpeed)
		{
			rigid.AddRelativeForce(-accelSpeed, 0, 0);
		}
		else if (willieFlg == false)
		{
			if (mudTrigger == true)
			{
				rigid.velocity = new Vector3(-1, 0, 0);

			}
		}
		else if (maxSpeed < -nowSpeed.x)
		{
			nowSpeed.x = oldSpeed.x - 10;

		}
		
		if (Input.GetKey(KeyCode.DownArrow) && rigid.velocity.x<0.1)
		{
			rigid.AddRelativeForce(brakeSpeed,0, 0);
			playerSound.Play("Break");														//サウンド追加分 2/4
		}
		if (rigid.velocity.x > 0)
		{
			rigid.velocity = Vector3.zero;
		}		

		if (joyconFlag == true)
		{
			if ((hd.GetRightBrake() == true || hd.GetLeftBrake() == true) && rigid.velocity.x < 0.1)
			{
				rigid.AddRelativeForce(brakeSpeed * 2 / 3, 0, 0);
				playerSound.Play("Break");                                                      //サウンド追加分 2/4
			}
			if ((hd.GetRightBrake() == true && hd.GetLeftBrake() == true) && rigid.velocity.x < 0.1)
			{
				rigid.AddRelativeForce(brakeSpeed, 0, 0);
				playerSound.Play("Break");                                                      //サウンド追加分 2/4
			}
		}

		//回転

		if (Input.GetKey(KeyCode.RightArrow))
		{
			this.gameObject.transform.Rotate(new Vector3(0, rotaSpeed, 0));
			//rigid.velocity = Quaternion.Euler(0, rotaSpeed, 0)* rigid.velocity;
			
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			this.gameObject.transform.Rotate(new Vector3(0, -rotaSpeed, 0));
			//rigid.velocity = Quaternion.Euler(0, -rotaSpeed, 0) * rigid.velocity;
		}

		junpFlag.GetComponent<JunpJudg>().JunpPlayer();

		//サウンド追加分 3/4
		if(succesRollingJump){
			playerSound.Play("Rolling");
			succesRollingJump = false;
		}

		if((nowSpeed.magnitude > 1f) && !runningSound.status.ToString().Equals("Playing")){
			runningSound.Play();
		}
		if(nowSpeed.magnitude <= 1f){
			runningSound.Stop();
		}
		//サウンド追加分 3/4 終了	
		
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
			Debug.Log(rigid.velocity.magnitude);

		}
		
		oldSpeed = nowSpeed;


	}
	
	//サウンド追加分 4/4
	void OnCollisionEnter(Collision other)
	{
		//Debug.Log(other.gameObject.name);
		if(other.gameObject.tag.Equals("Lode") && junp)
		{
			playerSound.Play("Landing");
			junp = false;
		}
	}
	
}
