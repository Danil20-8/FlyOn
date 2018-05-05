using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;


[ExecuteInEditMode]
public class AtosphereView : MonoBehaviour
{
    void Reset()
    {
        AtmosphereBehaviour a;
        var t = transform.Find("Atmosphere");
        if (t != null)
            a = t.GetComponent<AtmosphereBehaviour>();
        else
        {
            var at = Resources.Load<AtmosphereBehaviour>(@"Environment\Atmosphere");
            a = (AtmosphereBehaviour)Instantiate(at, transform.position, transform.rotation);
            a.name = "Atmosphere";
            a.transform.SetParent(transform);
        }

        a.Initialize(Color.white, transform.position + Vector3.right * 300, 100, 50, new System.Random());
    }
    
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 50, 25), "Update"))
            Reset();
    }
}

