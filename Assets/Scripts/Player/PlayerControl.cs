using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerControl : NetworkBehaviour {

    public void Start() {
        if (isLocalPlayer)
        {
            GetComponent<PlayerMovementControl> ().SetControl (true);
        }
    }
}
