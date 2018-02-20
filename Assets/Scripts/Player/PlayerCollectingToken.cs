using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectingToken : MonoBehaviour {

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

            Debug.Log ("Local player collected token");
            // TODO do something about it
        }
    }
}
