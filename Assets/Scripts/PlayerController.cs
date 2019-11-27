using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.5f;
    public float kasokuSpeed = 1.5f;
    public float jumpSpeed = 0.1f;
	public bool catchSwitch = false;
	public GameObject enemyHund;
	[SerializeField] int catchCount;

    private Rigidbody rB;

    void Start()
    {
        rB = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
		if (catchSwitch == false)
		{
			float x = Input.GetAxis("Horizontal");
			float y = 0f;
			float oldSpeed = speed;

			if (Input.GetKeyDown(KeyCode.Space) == true)
			{
				y = 1.0f;
			}

			if (Input.GetKeyDown(KeyCode.Z) == true)
			{
				speed = kasokuSpeed;
			}
			rB.AddForce(x * speed, y * jumpSpeed, 0, ForceMode.Impulse);

			if (speed == kasokuSpeed)
			{
				speed = oldSpeed;
			}
			
		}

		else if (catchSwitch == true)
		{
			catchCount += 1;
			transform.position = enemyHund.transform.position;
			if (catchCount == 100)
			{
				catchSwitch = false;
			}

		}
    }
	 void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "EnemyHund")
		{
			catchSwitch = true;
		}
		Debug.Log("触ったよ");
	}
}
