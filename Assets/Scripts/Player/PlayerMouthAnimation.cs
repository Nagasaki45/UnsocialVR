using UnityEngine;


public class PlayerMouthAnimation : MonoBehaviour {

    public Transform mouth;
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
            if (mouth.localScale.y > mouthOpen - epsilon)
            {
                target = mouthClose;
            }
            else if (mouth.localScale.y < mouthClose + epsilon)
            {
                target = mouthOpen;
            }
        }
        else
        {
            target = mouthClose;
        }
        Vector3 targetVector = new Vector3 (mouth.localScale.x, target, mouth.localScale.z);
        mouth.localScale = Vector3.Lerp (mouth.localScale, targetVector, speed * Time.deltaTime);
    }
}
