using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InertiaPlayer2 : MonoBehaviour
{
	//変数
	private Rigidbody rigid;
	[SerializeField] float accelSpeed;
	[SerializeField] float maxSpeed;
	[SerializeField] float rotaSpeed;
	[SerializeField] float brakeSpeed;


	public GameObject mud;
	public GameObject junpFlag;
	private bool mudTrigger;
	public bool junp;
	private Vector3 nowSpeed;
	private Vector3 oldSpeed;

	public Handle hd;
	//private float oldRotY;

	//サウンド追加分 1/5
	[SerializeField] private CriAtomSource playerSound;
	[SerializeField] private CriAtomSource runningSound;
	public bool succesRollingJump = false;

	void Awake()
	{
		//FPSを手動で固定
		Application.targetFrameRate = 60;
	}

	private void Start()
	{
		//rigidbodyの取得
		rigid = GetComponent<Rigidbody>();
		//oldRotY = 0.0f;

	}

	// Update is called once per frame
	void Update()
	{
		mudTrigger = mud.GetComponent<Obstacle>().triggerObsFlag;
		//junp = junpFlag.GetComponent<JunpJudg2>().nowJunpFlag;
		nowSpeed = rigid.velocity;

		//ここで速度とかの制御

		if (mudTrigger == false && rigid.velocity.magnitude < maxSpeed)
		{
			rigid.AddRelativeForce(-accelSpeed, 0, 0);
		}
		else if (mudTrigger == true)
		{
			rigid.velocity = new Vector3(-1, 0, 0);

		}
		else if (maxSpeed < -nowSpeed.x)
		{
			nowSpeed.x = oldSpeed.x - 10;

		}
		if ((hd.GetRightBrake() == true || hd.GetLeftBrake() == true) && rigid.velocity.x < 0.1)
		{
			rigid.AddRelativeForce(brakeSpeed*2/3, 0, 0);
			playerSound.Play("Break");														//サウンド追加分 2/4
		}
		if ((hd.GetRightBrake() == true && hd.GetLeftBrake() == true) && rigid.velocity.x < 0.1)
		{
			rigid.AddRelativeForce(brakeSpeed, 0, 0);
			playerSound.Play("Break");														//サウンド追加分 2/4
		}
		if (rigid.velocity.x > 0)
		{
			rigid.velocity = Vector3.zero;
		}

		//回転

		if (Input.GetKey(KeyCode.RightArrow))
		{
			this.gameObject.transform.Rotate(new Vector3(0, rotaSpeed, 0));
			rigid.velocity = Quaternion.Euler(0, rotaSpeed, 0) * rigid.velocity;
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			this.gameObject.transform.Rotate(new Vector3(0, -rotaSpeed, 0));
			rigid.velocity = Quaternion.Euler(0, -rotaSpeed, 0) * rigid.velocity;
		}

		junpFlag.GetComponent<JunpJudg2>().JunpPlayer();

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

		//確認用
		if (Input.GetKey(KeyCode.Z))
		{
			Debug.Log(rigid.velocity.magnitude);

		}

		oldSpeed = nowSpeed;

		var rot = transform.rotation.eulerAngles;
		rot.y = hd.GetControlllerAccel();
		transform.rotation = Quaternion.Euler(rot);
		//if (Mathf.Abs(rot.y) > Mathf.Abs(oldRotY)) rigid.velocity = Quaternion.Euler(0, (rot.y - oldRotY)/10, 0) * rigid.velocity;
		//else rigid.velocity = Quaternion.Euler(0, (oldRotY - rot.y)/10, 0) * rigid.velocity;
		//oldRotY = rot.y;
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
