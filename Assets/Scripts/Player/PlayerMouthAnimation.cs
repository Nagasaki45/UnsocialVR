using UnityEngine;


public class PlayerMouthAnimation : MonoBehaviour {

    public float mouthClose;
    public float mouthOpen;
    public float speed;

    private PlayerTalking playerTalking;
    private float epsilon = 0.01f;
    private float target;


    private void Start()
    {
        playerTalking = GetComponentInParent<PlayerTalking> ();
        target = mouthClose;
    }


    private void Update ()
    {
        if (playerTalking.isTalking)
        {
            if (transform.localScale.y > mouthOpen - epsilon)
            {
                target = mouthClose;
            }
            else if (transform.localScale.y < mouthClose + epsilon)
            {
                target = mouthOpen;
            }
        }
        else
        {
            target = mouthClose;
        }
        Vector3 targetVector = new Vector3 (transform.localScale.x, target, transform.localScale.z);
        transform.localScale = Vector3.Lerp (transform.localScale, targetVector, speed * Time.deltaTime);
    }
}
