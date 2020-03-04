using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudDumplingDivision : MonoBehaviour
{
    //バラバラになるときに使う変数
    [SerializeField] float radiuce = 5.0f; //爆発の影響範囲（円の半径）
    [SerializeField] float force = 15.0f;　//爆発の力

    private void OnCollisionEnter(Collision other)
    {
        foreach(Transform debris in GetComponentInChildren<Transform>())
        {
            debris.transform.parent = null;
            debris.GetComponent<MeshCollider>().enabled = true;
            Rigidbody rb = debris.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddExplosionForce(force, debris.transform.position, radiuce, 0f);
        }
    }
}
