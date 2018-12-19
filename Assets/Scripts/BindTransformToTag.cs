using UnityEngine;


public class BindTransformToTag : MonoBehaviour {

    public string bindToTag;

    private Transform trackedTransform;


    void Start ()
    {
        trackedTransform = GameObject.FindGameObjectWithTag (bindToTag).GetComponent<Transform> ();
    }


    void Update ()
    {
        transform.position = trackedTransform.position;
        transform.rotation = trackedTransform.rotation;
    }
}
