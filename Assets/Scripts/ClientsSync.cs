using UnityEngine;
using UnityEngine.Networking;

public class ClientsSync : NetworkBehaviour
{
    FlashScreen flashScreen;

    void Start()
    {
        flashScreen = GameObject.FindGameObjectWithTag ("FlashScreen").GetComponent<FlashScreen> ();
    }


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
        flashScreen.Flash ();
    }
}
