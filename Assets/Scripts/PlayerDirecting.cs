using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirecting : MonoBehaviour
{
	[SerializeField] float willieSpeed;
	[SerializeField] float willieTime;
	private Rigidbody rigid;
	public GameObject player;
	public bool willieFlg;
	private float startDetaTime;
	private float willieSTime;

	[SerializeField] private CuePlayer playerSound;					//サウンド追加分 1/2

	// Start is called before the first frame update
	void Start()
    {
		rigid = player.GetComponent<Rigidbody>();
		willieFlg = false;
	}

    // Update is called once per frame
    void Update()
    {
		
		startDetaTime = Time.time;

		if (willieFlg == true)
		{
			rigid.AddRelativeForce(-willieSpeed, 0, 0);
		}

		//ウィリー
		
		if (Input.GetKeyDown(KeyCode.S)&&willieFlg == false)
		{
			willieFlg = true;
			willieSTime = startDetaTime;
			this.gameObject.transform.Rotate(new Vector3(0, 0, 40));
			playerSound.Play("Willy");									//サウンド追加分 2/2
		}

		if (startDetaTime > willieTime + willieSTime)
		{
			if (willieFlg == true)
			{
				this.gameObject.transform.Rotate(new Vector3(0, 0, -40));
			}
			willieFlg = false;
			willieSTime = 0;
		}
		
	}
	
}
