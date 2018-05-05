using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class CanvasLineBeahviour : MonoBehaviour {

    new RectTransform transform;


    public Color color { get { return GetComponent<Image>().color; } set { GetComponent<Image>().color = value; } }

	// Use this for initialization
	void Awake () {
        transform = GetComponent<RectTransform>();
	}
	

    public void SetLine(Vector2 origin, Vector2 end)
    {
        var dir = origin - end;
        var dist = dir.magnitude;
        float cosa = Vector2.Dot(dir, Vector2.up) / dist;

        transform.eulerAngles = new Vector3(0, 0, Mathf.Acos(cosa) * Mathf.Rad2Deg * (origin.x > end.x ? -1 : 1));
        transform.position = (origin + end) / 2;

        transform.sizeDelta = Vector2.Scale(transform.sizeDelta, new Vector2(1, 0)) + new Vector2(0, dist);
    }
}
