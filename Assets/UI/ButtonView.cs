using UnityEngine;
using System.Collections;
using Assets.Models;
using Assets.Global;
public class ButtonView : MonoBehaviour {

    [SerializeField]
    Renderer back;
    [SerializeField]
    TextMesh text;

	// Use this for initialization
	void Start () {
        GameResources.colorThemeObserver.AddListener(SetTheme);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetTheme(ColorTheme theme)
    {
        back.material.color = theme.ContentColor;
        text.color = theme.ItemColor;
    }
}
