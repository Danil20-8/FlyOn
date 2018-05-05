using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {

    void Start()
    {
        enabled = false;
    }

    Vector3 force;

    [SerializeField]
    float spring = 5;
    [SerializeField]
    float fading = 10;
	void Update () {

        transform.localPosition += force * Time.deltaTime;

        //fading
        float fade = fading * Time.deltaTime;
        if (force.magnitude < fade)
        {
            force = Vector3.zero;
        }
        else
            force -= force.normalized * fade;
        //springing
        force += transform.localPosition * -(transform.localPosition.magnitude * spring * Time.deltaTime);
        if (transform.localPosition.magnitude < .1f && force.magnitude < .1f)
        {
            transform.localPosition = Vector3.zero;
            force = Vector3.zero;
            enabled = false;
        }
	}

    public void Stop()
    {
        transform.localPosition = Vector3.zero;
        force = Vector3.zero;
        enabled = false;
    }

    public void Shake(Vector3 force)
    {
        this.force += force;

        enabled = true;
    }
}
