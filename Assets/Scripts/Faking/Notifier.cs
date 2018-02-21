using UnityEngine;
using UnityEngine.Networking;


public class Notifier : MonoBehaviour {

    public bool active = false;

    private PlayerApproachedText playerApproachedText;


    private void Start ()
    {
        playerApproachedText = GameObject.FindGameObjectWithTag ("PlayerApproachedText").GetComponent<PlayerApproachedText> ();
    }


    private void Update () {
        if (active)
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
