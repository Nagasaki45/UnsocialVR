using UnityEngine;

public class LookAtSpeaker : MonoBehaviour {

    public bool active = false;
    public float speed;

    private Transform myTransform;

    void Start()
    {
        myTransform = GetComponent<Transform>();
    }


    void Update()
    {
        if (active)
        {
            GameObject speaker = PlayerTalking.speaker;
            if (speaker != null)
            {
                SlowlyRotateTowards(speaker.GetComponent<Transform>());
            }
        }
    }

    void SlowlyRotateTowards(Transform target)
    {
        Vector3 direction = (target.position - myTransform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, lookRotation, Time.deltaTime * speed);
    }
}
