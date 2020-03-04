using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class _test : MonoBehaviour
{
    public ADX_CueBank cueBank;

    void Start()
    {
        cueBank.play("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
