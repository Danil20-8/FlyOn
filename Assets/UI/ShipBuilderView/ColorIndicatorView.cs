using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColorIndicatorView : MonoBehaviour {

    [SerializeField]
    Color maxColor;

    [SerializeField]
    Color minColor;


    Image image;



	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}
	
	public void SetValue(float t)
    {
        image.color = Color.Lerp(minColor, maxColor, Mathf.Clamp01(t));
    }
}
