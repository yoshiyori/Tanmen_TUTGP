using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandControl : MonoBehaviour
{
    public GameObject player;
    public bool rodeIn;
    [SerializeField] GameObject sand;
    private GameObject sandObj;
    private int count = 0;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        rodeIn = player.GetComponent<MovePlayer>().sandControl;

        if (rodeIn == true)
        {
            count++;
            if (count == 1)
            {
               sandObj =  Instantiate(sand, this.gameObject.transform.position, Quaternion.identity);
            }
            
        }
        else
        {
            Destroy(sandObj, 2f); ;
            count = 0;
        }
    }
}
