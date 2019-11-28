﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;							//サウンド追加分1/3

public class EnemyAttck : MonoBehaviour
{
	public GameObject bullet;
	public GameObject player;
	[SerializeField] float bulletSpeed;
	private int attackCount = 0;

	//SoundSystem
	public GameSEPlayer bulletShotSE;		//サウンド追加分2/3

    // Update is called once per frame
    void Update()
    {
		attackCount += 1;

		if (attackCount % 20 == 0 && attackCount <= 100)
		{
			BulletAttck();
		}
		else if (attackCount == 160)
		{
			attackCount = 0;
		}
    }
	//これが攻撃の照準とかのやーつ
	void BulletAttck()
	{
			GameObject enemyBullet = Instantiate(bullet, transform.position, Quaternion.identity);
			Rigidbody bulletRd = enemyBullet.GetComponent<Rigidbody>();
			enemyBullet.transform.LookAt(player.transform); 
			bulletRd.velocity = enemyBullet.transform.forward.normalized * bulletSpeed;

			//マシンガン発射音再生
			bulletShotSE.PlaySEOneShot3D(0);	//サウンド追加分3/3

			Destroy(enemyBullet, 5.0f);
	}

}
