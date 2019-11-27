using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileEnemyAttck : MonoBehaviour
{
	   
	
	/*public GameObject target; 
	[SerializeField] float speed = 6.0f;    // 1秒間に進む距離
	[SerializeField] float rotSpeed = 180.0f;  // 1秒間に回転する角度*/
	public GameObject Missile;
	private int attackCount = 0;

	// Update is called once per frame
	void Update()

	{
		attackCount += 1;
		if (attackCount % 50 == 0)
		{
			GameObject enemyMissile = Instantiate(Missile, transform.position, Quaternion.identity);
			Debug.Log("発射");

			Destroy(enemyMissile, 5.0f);
			//残骸だから気にしたら負け
		/*	Vector3 vecTarget = target.transform.position - enemyMissile.transform.position; 
			Vector3 vecForward = enemyMissile.transform.TransformDirection(Vector3.forward);  
			float angleDiff = Vector3.Angle(vecForward, vecTarget);           
			float angleAdd = (rotSpeed * Time.deltaTime);                   
			Quaternion rotTarget = Quaternion.LookRotation(vecTarget);              
			if (angleDiff <= angleAdd)
			{
				enemyMissile.transform.rotation = rotTarget;
			}
			else
			{
				float t = (angleAdd / angleDiff);
				enemyMissile.transform.rotation = Quaternion.Slerp(enemyMissile.transform.rotation, rotTarget, t);
			}

			
			enemyMissile.transform.position += enemyMissile.transform.forward.normalized * speed * Time.deltaTime;

			Destroy(enemyMissile, 5.0f);
		*/	
		}
	}
}

