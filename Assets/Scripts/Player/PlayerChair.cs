using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerChair : NetworkBehaviour {

    [SyncVar]
    int chair;


    [Command]
    public void CmdSetChair(int x)
    {
        chair = x;
    }


    public int GetChair()
    {
        return chair;
    }
}
