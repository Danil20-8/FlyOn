using UnityEngine;
using System.Collections;
using Assets.Other;

public class KeepTargetCameraBehaviour : MonoBehaviour {

    public Transform target { get { return _target; } set { _target = value; if (_target != null) enabled = true; else enabled = false;} }
    Transform _target;
    new Transform camera;

    void Awake()
    {
        enabled = false;
    }

	// Use this for initialization
	void Start () {
        camera = GetComponentInChildren<Camera>(true).transform;
	}

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.forward;
        Vector3 dir = target.position - transform.position;

        var d = dir.magnitude;

        var cosa = Vector3.Dot(forward, dir) / d;
        if (cosa > 1f) cosa = 1f;
        float k = d * Mathf.Sqrt(1f - cosa * cosa); // sin from cos


        camera.position = transform.position + forward * -k;
    }
}
