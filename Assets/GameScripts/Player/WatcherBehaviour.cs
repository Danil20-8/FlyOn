using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class WatcherBehaviour: MonoBehaviour
{
    [SerializeField]
    KeepTargetCameraBehaviour cameraFoot;

    Transform cameraTransform;

    public Transform target { get { return _target; } set { _target = value;  cameraFoot.target = value; OnSetTaget(); } }
    Transform _target;

    public Vector2 sensetive = Vector2.one;
    public float distance = 10;
    Vector3 basePosition = new Vector3(0f, 2f, -1f);
    float cameraZoom = 5;
    const float minZoom = 5f;
    const float maxZoom = 15f;
    bool freeFly = false;

    void Awake()
    {
        enabled = false;
        cameraTransform = cameraFoot.transform;
    }

    public Camera GetCamera()
    {
        return cameraFoot.GetComponentInChildren<Camera>(true);
    }

    public void ReleaseCamera()
    {
        //enabled = true;
        target = null;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            freeFly = !freeFly;

        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0f)
        {
            cameraZoom -= scroll;
            cameraZoom = Mathf.Clamp(cameraZoom, minZoom, maxZoom);
        }

        if (freeFly)
        {
            cameraTransform.RotateAround(_target.position, cameraTransform.right, UInput.RotateY() * sensetive.y);
            cameraTransform.RotateAround(_target.position, _target.forward, UInput.RotateX() * - sensetive.x);
        }
        else
        {
            cameraTransform.RotateAround(_target.position, cameraTransform.right, UInput.RotateY() * sensetive.y);
            cameraTransform.RotateAround(_target.position, _target.up, UInput.RotateX() * sensetive.x);
        }

        NormalizeCamera();
    }

    public void MoveToOrigin(float angle)
    {
        var fq = target.rotation;
        cameraTransform.localPosition = Vector3.Slerp(cameraTransform.localPosition, basePosition * cameraZoom, angle / Vector3.Angle(cameraTransform.localPosition, basePosition));
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, fq, angle / Quaternion.Angle(cameraTransform.rotation, fq));
    }

    void NormalizeCamera()
    {
        cameraTransform.position = target.transform.position + (cameraTransform.position - target.transform.position).normalized * (cameraZoom * distance);
    }

    void OnSetTaget()
    {
        cameraTransform.SetParent(target);
        if (target != null)
        {
            cameraTransform.localPosition = basePosition;
            cameraTransform.rotation = target.rotation;
        }
    }
}

