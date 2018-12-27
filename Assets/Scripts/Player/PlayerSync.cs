using UnityEngine;
using UnityEngine.Networking;

public class PlayerSync : NetworkBehaviour
{
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

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
        Logger.Event("Syncing");
        ToggleLight();
        Invoke("ToggleLight", 0.5f);
    }


    void ToggleLight()
    {
        Light light = GameObject.FindGameObjectWithTag("Light").GetComponent<Light>();
        light.enabled = !light.enabled;
    }
}
