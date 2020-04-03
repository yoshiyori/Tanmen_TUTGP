using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class ConeAttck : MonoBehaviour
{
    public bool triggerObsFlag;
    private Rigidbody rigid;
    public GameObject playerObject;
    public float downSpeed;
    private Vector3 nowSpeed;

    //サウンド追加分 1/2
    [SerializeField] private CuePlayer cornSound;
    [SerializeField] private MeshFilter cornMesh;
    [SerializeField] private Collider cornCollider;
    //サウンド追加分 1/2 終了

    private void Start()
    {
        triggerObsFlag = false;
        rigid = playerObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            triggerObsFlag = true;
            nowSpeed = rigid.velocity;

            rigid.velocity = Vector3.zero;
            cornSound.PlayAndDestroy("Corn", ref cornMesh, ref cornCollider);                               //サウンド追加分 2/2
            //Destroy(this.gameObject);                                                                     //サウンド変更分 1/1
        }
    }

 
}
