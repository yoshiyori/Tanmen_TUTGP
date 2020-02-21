using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InertiaPlayer2 : MonoBehaviour
{
	// 初期化とか必要な変数をそろえてる
	Vector3 Pold = new Vector3(0.0f,0.0f,0.0f);
	Vector3 Pnew = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 Vold = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 Vnew = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 A;
	Vector3 Direction;
	private float DeltaT = 0.05f;
	private float Accel = 0.0f;
	private bool OneTime = true;

	public Handle hd;
	//[SerializeField] Quaternion rot;

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

		//transform.Rotate(new Vector3(0, hd.GetControlllerAccel(), 0));
		var rot = transform.rotation.eulerAngles;
		rot.y = hd.GetControlllerAccel();
		transform.rotation = Quaternion.Euler(rot);
		
	}
	
}
