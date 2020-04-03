using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SoundSystem;

public class MudDumplingDivision : MonoBehaviour
{
    //バラバラになるときに使う変数
    [SerializeField] float radiuce = 5.0f; //爆発の影響範囲（円の半径）
    [SerializeField] float force = 15.0f;　//爆発の力

    //サウンド追加分 1/2
    [SerializeField] private CuePlayer crayBallSound;
    [SerializeField] private Collider crayBallCollider;
    private MeshFilter crayBallMesh = null;
    private bool broken = false;

    void Start(){
        crayBallSound.Play("CrayBall");
    }
    //サウンド追加分 1/2 終了

    private void OnCollisionEnter(Collision other)
    {
        foreach (Transform debris in GetComponentInChildren<Transform>())
        {
            debris.transform.parent = null;
            debris.GetComponent<MeshCollider>().enabled = true;
            Rigidbody rb = debris.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddExplosionForce(force, debris.transform.position, radiuce, 0f);
        }

        //サウンド追加分 2/2
        //Debug.Log(other.gameObject.name);
        if (!broken)
        {
            crayBallSound.Stop();
            //crayBallSound.Play("CrayBall", 0, 1f);
            crayBallSound.PlayAndDestroy("CrayBall", ref crayBallMesh, ref crayBallCollider, 0, 1f);
            broken = true;
        }
        //サウンド追加分 2/2 終了
    }
}
