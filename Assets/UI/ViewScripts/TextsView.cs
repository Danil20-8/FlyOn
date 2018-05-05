using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.GameModels;
public class TextsView : MonoBehaviour {

    [SerializeField]
    GameObject panel;
    [SerializeField]
    Text aliveText;
    [SerializeField]
    Text enabledText;
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text speedText;
	// Use this for initialization
	void Start () {
	
	}
    public void SetAlive(bool value)
    {
        aliveText.text = value ? "Alive" : "Dead";
    }
    public void SetEnabled(bool value)
    {
        enabledText.text = value ? "Enabled" : "Disabled";
    }
    public void SetName(string name)
    {
        nameText.text = name;
    }
    public void SetSpeed(string speed)
    {
        speedText.text = speed;
    }
    public void SetData(SystemLink.Info info)
    {
        SetName(info.name);
        SetAlive(info.alive);
        SetEnabled(info.enabled);
    }
}
