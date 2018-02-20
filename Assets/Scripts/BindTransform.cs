using UnityEngine;


public class BindTransform : MonoBehaviour {

    public string bindToTag;
    public Transform bindedTransform;

    private Transform trackedTransform;


    void Start ()
    {
        trackedTransform = GameObject.FindGameObjectWithTag (bindToTag).GetComponent<Transform> ();
    }


    void Update ()
    {
        bindedTransform.position = trackedTransform.position;
        bindedTransform.rotation = trackedTransform.rotation;
    }
}
