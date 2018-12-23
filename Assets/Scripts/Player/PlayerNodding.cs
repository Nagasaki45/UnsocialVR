using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNodding : MonoBehaviour {

    public string[] animations;
    public float animationCrossfadeSeconds;
    public Animator animator;


    public void Nod()
    {
        int i = Random.Range(0, animations.Length);
        string animation = animations[i];
        Logger.Event("Nodding fake " + animation);
        animator.CrossFade(animation, animationCrossfadeSeconds);
    }
}
