using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EnergyIndicator : MonoBehaviour {

    [SerializeField]
    Text text;

	// Use this for initialization
	void Start () {
        GetComponentInParent<PointerListenerBehaviour>()
            .AddClickListener<SystemComponentView>(b => UpdateView((SystemComponentView)b));
	}

    void UpdateView(SystemComponentView component)
    {

        var info = ((SystemDiagnoseableTrees)(component.GetTree())).GetInfo(component);

        GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.green, info.lastEnergy);

        text.text = info.lastEnergy.ToString();
    }
}
