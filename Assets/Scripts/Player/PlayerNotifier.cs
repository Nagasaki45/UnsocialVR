using UnityEngine;
using UnityEngine.Networking;


public class PlayerNotifier : NetworkBehaviour {

    private PlayerApproachedText playerApproachedText;
    private PlayerAutopilot playerAutopilot;


    private void Start ()
    {
        playerApproachedText = GameObject.FindGameObjectWithTag ("PlayerApproachedText").GetComponent<PlayerApproachedText> ();
        playerAutopilot = gameObject.GetComponent<PlayerAutopilot> ();
    }


    private void Update () {
        if (isLocalPlayer && playerAutopilot.isFaking)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
            foreach (var player in players)
            {
                if (player != gameObject) {
                    PlayerTalking playerTalking = player.GetComponent<PlayerTalking> ();
                    PlayerGaze playerGaze = player.GetComponent<PlayerGaze> ();
                    if (playerTalking.isTalking && (playerGaze.gazedObj == gameObject))
                    {
                        Debug.Log ("Someone is talking to the local player");
                        playerApproachedText.Flash ();
                    }
                }
            }
        }
    }
}
