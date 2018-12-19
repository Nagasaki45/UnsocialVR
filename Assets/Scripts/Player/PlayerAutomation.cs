using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutomation : MonoBehaviour {

    public string fakingTheory;
    public Transform trueHead;
    public Transform remoteHead;


    void Start()
    {
    }


    void Update()
    {
        if (true)  // TODO While not automated
        {
            remoteHead.position = trueHead.position;
            remoteHead.rotation = trueHead.rotation;
        }
    }
}
