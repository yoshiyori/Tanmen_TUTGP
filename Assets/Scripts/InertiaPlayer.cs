using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InertiaPlayer : MonoBehaviour
{
	// Start is called before the first frame update

	[SerializeField] float MaxSpeed = 0.1f;
	Vector3 Pold = new Vector3(0.0f,0.0f,0.0f);
	Vector3 Pnew = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 Vold = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 Vnew = new Vector3(0.0f, 0.0f, 0.0f);
	Vector3 A;
	Vector3 Direction;
	private float DeltaT = 0.05f;
	private float Accel = 0.0f;
	private bool OneTime = true;
	public void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
    {
		if (OneTime == true)
		{
			Direction = Quaternion.Euler(Pold) * Vector3.right;
			OneTime = false;
		}
		else if (OneTime == false)
		{
			Direction = Quaternion.Euler(Direction) * Vector3.right;
		}

		A = Direction * Accel;	

		Pold = Pnew;
		Vold = Vnew;

		Vnew = Vold + A * DeltaT;
		Pnew = Pold + Vold * DeltaT;

		this.gameObject.transform.Translate(Pnew);

		Accel = 0.0f;

		if (Input.GetKey(KeyCode.RightArrow))
		{
			//transform.Rotate(new Vector3(0, 15, 0) * Time.deltaTime);
			Direction.y += 0.1f;
		}
		else if (Input.GetKey(KeyCode.LeftArrow))
		{
			//transform.Rotate(new Vector3(0, -15, 0) * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.UpArrow) && Accel > -MaxSpeed)
		{
			Accel = Accel - 0.01f;
		}
		else if (Input.GetKey(KeyCode.DownArrow) && Accel <= 0.0f)
		{
			Accel = Accel + 0.05f;	
		}

		Debug.Log(Accel);
	}
	
}
