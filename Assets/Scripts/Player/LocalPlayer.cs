using UnityEngine;
using UnityEngine.Networking;


public class LocalPlayer : NetworkBehaviour {

    public static GameObject localPlayer;

    public void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        localPlayer = gameObject;

        // General local player scripts
        GetComponent<PlayerAutopilot>().enabled = true;
        GetComponent<PlayerAccuses>().enabled = true;
        GetComponent<PlayerMovementControl>().SetControl (true);
        GetComponent<PlayerLogger>().enabled = true;

        // PubSub
        GetComponent<PlayerHeadNod>().enabled = true;
        GetComponent<PlayerExpectsBackchannel>().enabled = true;
        GetComponent<PlayerDisfluent>().enabled = true;

        // Faking generators
        GetComponent<LookAtSpeaker>().enabled = true;
        foreach(var naturalMovement in GetComponentsInChildren<NaturalMovement>())
        {
            naturalMovement.enabled = true;
        }
    }
}
