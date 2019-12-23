using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineGizmos : MonoBehaviour
{
    [ColorUsage (true)]
    public Color color = Color.red;
    void OnDrawGizmos ()
    {
        Gizmos.color = color;
        Transform[] targets = this.gameObject.GetComponentsInChildrenWithoutSelf<Transform> ();
        for (int i = 0; i < targets.Length - 1; i++)
        {
            Vector3 to = transform.TransformPoint (targets[i + 1].localPosition);
            Vector3 from = transform.TransformPoint (targets[i].localPosition);
            Gizmos.DrawLine (from, to);
        }
    }
}