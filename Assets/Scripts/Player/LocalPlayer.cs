using UnityEngine;
using UnityEngine.Networking;


public class LocalPlayer : NetworkBehaviour {

    public void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        // General local player scripts
        GetComponent<PlayerAutopilot>().enabled = true;
        GetComponent<PlayerAccuses>().enabled = true;
        GetComponent<PlayerMovementControl>().SetControl (true);

        // Faking generators
        GetComponent<Notifier>().enabled = true;
        GetComponent<LookAtSpeaker>().enabled = true;

        // PubSub
        GetComponent<PubSubClient>().enabled = true;
        GetComponent<PlayerHeadNod>().enabled = true;
        GetComponent<PlayerExpectsBackchannel>().enabled = true;

        foreach(var naturalMovement in GetComponentsInChildren<NaturalMovement>())
        {
            naturalMovement.enabled = true;
        }
    }
}
