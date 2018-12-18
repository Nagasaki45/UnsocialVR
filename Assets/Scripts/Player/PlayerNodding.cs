using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNodding : MonoBehaviour {

    public string[] animations;
    public float animationCrossfadeSeconds;
    public Animator animator;

    PlayerAutomation playerAutomation;


    void Start()
    {
        playerAutomation = GetComponent<PlayerAutomation>();
    }


    public void Nod(string theory)
    {
        if (playerAutomation.fakingTheory == theory)
        {
            int i = Random.Range(0, animations.Length);
            string animation = animations[i];
            Logger.Event("Nodding fake " + animation);
            animator.CrossFade(animation, animationCrossfadeSeconds);
        }
    }
}
