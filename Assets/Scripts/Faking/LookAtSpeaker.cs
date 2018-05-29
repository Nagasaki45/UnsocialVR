using UnityEngine;

public class LookAtSpeaker : MonoBehaviour {

    public bool active = false;
    public float speed;
    public float jitterFreq;
    public float jitterLerp;
    public float jitterScale;
    public float blankGazeDistance;

    private Transform myTransform;
    private Vector3 randomValue;
    private Vector3 randomTarget;

    void Start()
    {
        myTransform = GetComponent<Transform>();
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
            if (speaker != null)
            {
                target = speaker.transform.Find("HeadController").position;
            }
            else
            {
                target = myTransform.position + myTransform.forward * blankGazeDistance;
            }
            SlowlyRotateTowards(target);
        }
    }

    void SlowlyRotateTowards(Vector3 target)
    {
        Vector3 direction = (target - myTransform.position + randomValue * jitterScale).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, lookRotation, Time.deltaTime * speed);
    }
}
