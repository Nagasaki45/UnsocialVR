using UnityEngine;
using UnityEngine.Networking;

public class ClientsSync : NetworkBehaviour
{
    PlayerLogger logger;
    FlashScreen flashScreen;

    void Start()
    {
        logger = GetComponent<PlayerLogger>();
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
        logger.Event("Clients sync");
        flashScreen.Flash ();
    }
}
