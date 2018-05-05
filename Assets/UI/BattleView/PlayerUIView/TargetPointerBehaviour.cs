using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class TargetPointerBehaviour : MonoBehaviour {

    [SerializeField]
    Image gear;
    [SerializeField]
    Image pointer;

    public RectTransform rectTransform { get; private set; }

    public float gearSpeed { get { return _gearSpeed.z; } set { _gearSpeed.z = value; } }
    public Color color { get { return pointer.color; } set { pointer.color = value; gear.color = value; } }

    Vector3 _gearSpeed = new Vector3(0, 0, 180);
	
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

	// Update is called once per frame
	public void UpdateGear () {
        gear.transform.eulerAngles += _gearSpeed * Time.deltaTime;
	}
}
