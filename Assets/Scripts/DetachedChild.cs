using UnityEngine;


public class DetachedChild : MonoBehaviour {

    public GameObject child;

    void Start()
    {
        child.transform.parent = null;
    }


    public void SetVisibility(bool onOff)
    {
        foreach (var r in child.GetComponentsInChildren<MeshRenderer> ()) {
            r.enabled = onOff;
        }
    }


    void OnDestroy()
    {
        Destroy(child);
    }
}
