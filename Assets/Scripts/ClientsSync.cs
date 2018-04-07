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
        if (Input.GetButtonDown("ClientsSync"))
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
        Debug.Log("Clients sync (screen flashes)");
        flashScreen.Flash ();
    }
}
