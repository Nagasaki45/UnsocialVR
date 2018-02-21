using UnityEngine;
using UnityEngine.Networking;


public class LocalPlayer : NetworkBehaviour {

    public void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        GetComponent<PlayerAutopilot>().enabled = true;
        GetComponent<PlayerAccuses>().enabled = true;
        GetComponent<PlayerNotifier>().enabled = true;
        GetComponent<PlayerMovementControl>().SetControl (true);

        foreach(var naturalMovement in GetComponentsInChildren<NaturalMovement>())
        {
            naturalMovement.enabled = true;
        }
    }
}
