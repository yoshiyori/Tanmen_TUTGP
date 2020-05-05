using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionControl : MonoBehaviour
{
    private GameObject sandPos;
    private GameObject player;
    private bool check;
    private Vector3 pos;
    // Update is called once per frame
    void Start()
    {
        sandPos = GameObject.FindGameObjectWithTag("Dust");
        player = GameObject.FindGameObjectWithTag("player");
        check = true;
    }
    void Update()
    {
        check = player.GetComponent<MovePlayer>().sandControl;
        if (check == true)
        {
            transform.position = sandPos.transform.position;
            pos = this.gameObject.transform.position;
        }
        else
        {
            transform.position = pos;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag.Equals("Road"))
        {
            check = false;
        }
   
    }
}
