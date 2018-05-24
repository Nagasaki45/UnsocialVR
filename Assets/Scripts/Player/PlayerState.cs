using UnityEngine;
using UnityEngine.Networking;

public class PlayerState : NetworkBehaviour {

    [SyncVar]
    public bool isFaking = false;


    void Start() {}


    [Command]
    public void CmdSetFakingState (bool onOff)
    {
        isFaking = onOff;
    }
}
