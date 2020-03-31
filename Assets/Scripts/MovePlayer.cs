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
	private float TureMaxSpeed;
	public GameObject mud;
	public GameObject junpFlag;
	public GameObject objectPlayer;
	private bool mudTrigger;
	public bool junp;
	private bool willieFlg;
	public bool turnTipe;
	private int count = 0;
	private Vector3 nowSpeed;
	private Vector3 oldSpeed;
	private Vector3 pos;
	public bool cameraStop;


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
		TureMaxSpeed = maxSpeed;
		
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
			playerSound.Play("Break"); 
			//サウンド追加分 2/4
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
				playerSound.Play("Break");                                                      //サウンド追加分 2/4
			}
			if ((hd.GetRightBrake() == true && hd.GetLeftBrake() == true) && rigid.velocity.x < 0.1)
			{
				rigid.AddRelativeForce(brakeSpeed, 0, 0);
				playerSound.Play("Break");                                                      //サウンド追加分 2/4
			}
		}

		//回転
		
		turnPlayer();
		
		
		junpFlag.GetComponent<JunpJudg>().JunpPlayer();

		//サウンド追加分 3/4
		if (succesRollingJump)
		{
			playerSound.Play("Rolling");
			succesRollingJump = false;
		}

		if ((nowSpeed.magnitude > 1f) && !runningSound.status.ToString().Equals("Playing"))
		{
			runningSound.Play();
		}
		if (nowSpeed.magnitude <= 1f)
		{
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
			Debug.Log(maxSpeed);

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
			Debug.Log("動いた");
		}

		Debug.Log(check);
		Debug.Log(count);
	}
	//サウンド追加分 4/4
	void OnCollisionEnter(Collision other)
	{
		//Debug.Log(other.gameObject.name);
		if (other.gameObject.tag.Equals("Lode") && junp)
		{
			playerSound.Play("Landing");
			junp = false;
		}
	}
}
