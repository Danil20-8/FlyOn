using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.GameModels;
using System.Text.RegularExpressions;
public class SearchBehaviour : MonoBehaviour
{

    [SerializeField]
    ContainerListView list;
    [SerializeField]
    InputField nameInput;
    [SerializeField]
    InputField classInput;

    public void Search()
    {
        if(classInput.text != "")
            list.Order<SystemComponent>(c => SearchString(c.name, nameInput.text) + SearchInt(c.shipClass, int.Parse(classInput.text), 5));
        else
            list.Order<SystemComponent>(c => SearchString(c.name, nameInput.text));

    }
    float SearchString(string l, string r)
    {
        if (r.Length == 0)
            return 0;
        if (l == r)
            return 1;
        var m = Regex.Match(l.ToLower(), r.ToLower());
        if (m.Success)
        {
            if (m.Index == 0)
                return .5f;
            else
                return .5f - m.Index / l.Length * .5f;
        }
        else
            return - Mathf.Abs(l.Length - r.Length) / l.Length;
    }
    float SearchInt(int l, int r, int bound)
    {
        return  l == r ? 1 : - Mathf.Abs(r - l) / bound;
    }

}
