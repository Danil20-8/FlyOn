using UnityEngine;
using UnityEngine.Events;
using Assets.Models;
using Assets.Global;
public class Themeable : MonoBehaviour {

	void Awake () {
        GameResources.colorThemeObserver.AddListener(SetTheme);
	}

    protected virtual void SetTheme(ColorTheme theme)
    {

    }
}
