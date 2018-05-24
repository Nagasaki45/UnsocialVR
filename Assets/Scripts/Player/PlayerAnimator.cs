using UnityEngine;

class PlayerAnimator : MonoBehaviour
{
    public string[] noddingStates;
    public float animationCrossfadeSeconds;

    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public void Nod()
    {
        int i = Random.Range(0, noddingStates.Length);
        animator.CrossFade(noddingStates[i], animationCrossfadeSeconds);
    }
}
