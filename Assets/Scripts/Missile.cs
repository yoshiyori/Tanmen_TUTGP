using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class Missile : MonoBehaviour
{
	Rigidbody rigid;
    Vector3 velocity;
	Vector3 position;
	private Vector3 acceleration;
	private GameObject player;
	float period = 2f;

	public GameSEPlayer missileHit;

	//ポジションの取得と初速を与えている。
	void Start()
	{

		position = transform.position;
		rigid = this.GetComponent<Rigidbody>();
		velocity = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), 0);

	}

	//ここで加速度などを計算しているhttps://youtu.be/t_4MbV2zIwg　ここ見た。
	void Update()
	{
		
		player = GameObject.Find("TenporaryPlayer");
		acceleration = Vector3.zero;
		var diff = player.transform.position - transform.position;
		acceleration += (diff - velocity * period) * 2f	/ (period * period);

		if (acceleration.magnitude > 100f)
		{
			acceleration = acceleration.normalized * 100f;
		}
		period -= Time.deltaTime;
		velocity += acceleration * Time.deltaTime;
	}
	//実際に力を与えているのはここ

	void FixedUpdate()
	{
		rigid.MovePosition(transform.position + velocity * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other){
		if(other.name.Equals("TenporaryPlayer")){
			missileHit.PlaySEOneShot3D(0);
		}
	}
}
