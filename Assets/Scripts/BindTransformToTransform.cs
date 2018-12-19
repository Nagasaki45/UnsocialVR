using UnityEngine;


public class BindTransformToTransform : MonoBehaviour {

    public Transform trackedTransform;


    void Update ()
    {
        transform.position = trackedTransform.position;
        transform.rotation = trackedTransform.rotation;
    }
}
