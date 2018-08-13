using UnityEngine;

public class LookAtSpeaker : MonoBehaviour {

    public bool active = false;
    public float speed;
    public float jitterFreq;
    public float jitterLerp;
    public float jitterScale;
    public float blankGazeDistance;
    public Transform trackedTransform;
    public float lerp;

    private Vector3 randomValue;
    private Vector3 randomTarget;

    void Start()
    {
        InvokeRepeating("Repeatedly", 0, 1 / jitterFreq);
    }


    void Repeatedly()
    {
        randomTarget = Random.insideUnitSphere;
    }


    void Update()
    {
        if (active)
        {
            randomValue = Vector3.Lerp(randomValue, randomTarget, Time.deltaTime * jitterLerp);
            GameObject speaker = PlayerTalking.speaker;
            Vector3 target;
            if (speaker != null && speaker != transform.parent.parent.gameObject)
            {
                target = speaker.transform.Find("Performative/Head").position;
            }
            else
            {
                target = transform.position + transform.forward * blankGazeDistance;
            }
            SlowlyRotateTowards(target);
        } else {
            transform.position = Vector3.Lerp(transform.position, trackedTransform.position, lerp * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, trackedTransform.rotation, lerp * Time.deltaTime);
        }
    }

    void SlowlyRotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position + randomValue * jitterScale).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
    }
}
