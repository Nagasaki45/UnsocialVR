using UnityEngine;


public class NaturalMovement : MonoBehaviour {

    public bool active = false;
    public Transform trackedTransform;
    public float recordingPeriod = 0.1f;
    public int recordingFrames = 40;
    public float lerp;

    private EndlessLoopingBuffer<Vector3> positions;
    private EndlessLoopingBuffer<Quaternion> rotations;
    private Vector3 targetPos;
    private Quaternion targetRot;


    void Start()
    {
        positions = new EndlessLoopingBuffer<Vector3>(new Vector3[recordingFrames]);
        rotations = new EndlessLoopingBuffer<Quaternion>(new Quaternion[recordingFrames]);
        InvokeRepeating ("Repeatedly", 0, recordingPeriod);
    }


    void Repeatedly()
    {
        if (active)
        {
            targetPos = positions.Read();
            targetRot = rotations.Read();
        }
        else
        {
            positions.Write(trackedTransform.position);
            rotations.Write(trackedTransform.rotation);
            targetPos = trackedTransform.position;
            targetRot = trackedTransform.rotation;
        }
    }


    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, lerp * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerp * Time.deltaTime);
    }
}
