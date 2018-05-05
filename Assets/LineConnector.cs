using UnityEngine;
using System.Collections;

public class LineConnector : MonoBehaviour {

    public Transform connector;

    LineRenderer lr;

    void Awake()
    {
        lr = gameObject.AddComponent<LineRenderer>();
        lr.SetWidth(.5f, .5f);
        lr.SetVertexCount(0);
        lr.material = GetComponent<MeshRenderer>().material;
        Color c = Color.gray;
        lr.material.color = c;
    }
    void LateUpdate()
    {
        UpdateLine();
    }
    public void SetConnector(Transform connector)
    {
        this.connector = connector;
    }

    public void UpdateLine()
    {
        if (connector != null)
        {
            lr.SetVertexCount(2);
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, connector.transform.position);
        }
        else
            lr.SetVertexCount(0);
    }
}
