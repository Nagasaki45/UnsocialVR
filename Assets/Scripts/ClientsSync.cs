using UnityEngine;
using UnityEngine.Networking;

public class ClientsSync : NetworkBehaviour
{
    void Update()
    {
        if (isLocalPlayer && Input.GetButtonDown("ClientsSync"))
        {
            CmdSync();
        }
    }


    [Command]
    void CmdSync()
    {
        RpcSync();
    }


    [ClientRpc]
    void RpcSync()
    {
        Logger.Event("Clients sync");
        // TODO visual indication
    }
}
