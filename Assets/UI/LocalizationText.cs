using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Global;

public class LocalizationText: MonoBehaviour {

    Text uiText;
    TextMesh text3D;

    [SerializeField]
    LString text;

	void Start () {
        string r = Localization.GetGlobalString(text.text);

        text3D = GetComponent<TextMesh>();
        uiText = GetComponent<Text>();
        if (text3D != null) text3D.text = r;
        if (uiText != null) uiText.text = r;
	}
}
