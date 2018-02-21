using UnityEngine;


public class NaturalMovement : MonoBehaviour {

    public bool active = false;
    public float recordingPeriod = 0.1f;
    public int recordingFrames = 40;

    private EndlessLoopingBuffer<Vector3> positions;
    private EndlessLoopingBuffer<Quaternion> rotations;


    void Start()
    {
        positions = new EndlessLoopingBuffer<Vector3>(new Vector3[recordingFrames]);
        rotations = new EndlessLoopingBuffer<Quaternion>(new Quaternion[recordingFrames]);
        InvokeRepeating ("Repeatedly", 0, recordingPeriod);
    }


    private void Repeatedly()
    {
        if (active)
        {
            // TODO proper Lerping
            transform.localPosition = positions.Read();
            transform.localRotation = rotations.Read();
        }
        else
        {
            positions.Write(transform.localPosition);
            rotations.Write(transform.localRotation);
        }
    }
}
