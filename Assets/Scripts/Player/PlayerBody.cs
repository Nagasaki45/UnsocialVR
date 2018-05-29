using UnityEngine;


public class PlayerBody : MonoBehaviour {

    private GameObject body;

    void Start()
    {
        body = transform.Find("Body").gameObject;
        body.transform.parent = null;
    }


    public void SetVisibility(bool onOff)
    {
        foreach (var r in body.GetComponentsInChildren<MeshRenderer> ()) {
            r.enabled = onOff;
        }
    }


    void OnDestroy()
    {
        Destroy(body);
    }
}
