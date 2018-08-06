using UnityEngine;
using UnityEngine.Networking;

public class ClientsSync : NetworkBehaviour
{
    public Light lights;


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
        Logger.Event("Clients sync");
        ToggleLights();
        Invoke("ToggleLights", 0.5f);
    }


    void ToggleLights()
    {
        lights.enabled = !lights.enabled;
    }
}
