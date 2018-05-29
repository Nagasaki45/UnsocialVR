using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectingToken : MonoBehaviour {

    public int tokenWorth;

    private AudioSource collectSound;


    private void Start()
    {
        collectSound = GetComponent<AudioSource> ();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Token")
        {
            Destroy (other.gameObject);
            collectSound.Play ();
            LocalPlayer.localPlayer.GetComponent<PlayerState>().CmdAddScore(tokenWorth);
            LocalPlayer.localPlayer.GetComponent<PlayerLogger>().Event("Collecting token");
        }
    }
}
