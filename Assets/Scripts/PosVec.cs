﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PosVec : MonoBehaviour
{
    public GameObject player;
    private Vector3 playerVec;
    private Vector3 unit;
    private Vector3 nowPos;

    private float a;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        playerVec = rb.velocity;

    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        playerVec = rb.velocity;
        unitVec(playerVec);
        nowPos = this.gameObject.transform.localPosition;

        this.gameObject.transform.localPosition = new Vector3(nowPos.x * unit.x, nowPos.y, nowPos.z * unit.z);
    }
    void unitVec(Vector3 Vec)
    {
        float a = (float)Math.Sqrt(Vec.x * Vec.x + Vec.y * Vec.y + Vec.z * Vec.z);
        if (a != 0)
        {
            unit = new Vector3(Vec.x / a, Vec.y / a, Vec.z / a);
        }
        else
        {
            unit = Vec;
        }
    }
}
