using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InertiaPlayer2 : MonoBehaviour
{
	// 初期化とか必要な変数をそろえてる
	Vector3 Pold = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 Pnew = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 Vold = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 Vnew = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 A;
	Vector3 Direction;
	private float DeltaT = 0.05f;
	private float Accel = 0.0f;
	private bool OneTime = true;


	GameObject coneCtrl;
	GameObject mudCtrl;
	Cone cnScr;     //Cone.cs参照
	Mud mdScr;      //Mud.cs参照
	private int slipTime; //泥踏んで回転する処理のループ回数

	void Start()
	{
		coneCtrl = GameObject.Find("ConeCtrl");
		mudCtrl = GameObject.Find("MudCtrl");
		cnScr = coneCtrl.GetComponent<Cone>();
		mdScr = mudCtrl.GetComponent<Mud>();
		slipTime = 0;
	}

	// Update is called once per frame
	void Update()
	{
		/*	気にしたら負け
		 *	if (OneTime == true)
			{
				Direction = Quaternion.Euler(Pold) * Vector3.right;
				OneTime = false;
			}
			else if (OneTime == false)
			{
				Direction = Quaternion.Euler(Direction) * Vector3.right;
			}
			*/

		//ここで慣性計算してるである。（渡辺先生のパクった）

		Direction = Quaternion.Euler(Vnew) * Vector3.right;
		if (cnScr.coneFlag == true) Direction = Quaternion.Euler(Vnew)
				 * new Vector3(Mathf.Cos(slipTime * Mathf.PI / 12), 0.0f, Mathf.Cos(slipTime * Mathf.PI / 12));
		A = Direction * Accel;

		if (cnScr.coneFlag == true)
		{
			Vnew *= 0;
			A = Vnew * 0;
		}

		Pold = Pnew;
		Vold = Vnew;

		Vnew = Vold + A * DeltaT;
		Pnew = Pold + Vold * DeltaT;

		if (mdScr.mudFlag == true)
		{
			Vnew = Vold / 10;
			Pnew = Pold + Vold * DeltaT;
		}

		this.gameObject.transform.Translate(Pnew);
		//加速度の初期化
		Accel = 0.0f;

		if (cnScr.coneFlag == false)
		{
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
				Accel = Accel - 0.001f;
			}
			else if (Input.GetKey(KeyCode.DownArrow))
			{
				Accel = Accel + 0.03f;
			}

		}



		if (cnScr.coneFlag == true)
		{

			transform.Rotate(new Vector3(0, 15, 0));//30にすると2周廻る
			slipTime++;
			if (slipTime > 23)
			{
				cnScr.coneFlag = false;
				slipTime = 0;
			}
		}


	}



}
