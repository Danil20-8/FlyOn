using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using Assets.GameModels;
using Assets.Other;
using MyLib;

public class LinksGroupViewBehaviour : ContainerItemView {

    [SerializeField]
    Text groupName;
    [SerializeField]
    Text groupKey;
    [SerializeField]
    Image groupHealth;
    [SerializeField]
    Image groupState;

    [SerializeField]
    MonoBehaviour markingBehaviour;

    SystemLink link;

    GameObject links;

    protected override void OnSetModel(ref object model)
    {
        var g = (LinksGroup)model;

        groupName.text = g.name;
        groupKey.text = g.key.ToUpper();

        link = g.link;
        model = g.link;

        Elf.With(GetComponent<ContainerListView>(),
            l => l.GetComponent<ContainerListView>().AddRange(link.ByElements().Skip(1)),
            l => links = l.gameObject
        );

        Refresh();
    }

    public void Refresh()
    {
        groupHealth.color = Color.Lerp(Color.red, Color.green, link.health);

        groupState.color = link.enabled && link.enabledInBranch ? Color.yellow : Color.gray;
    }

    void Update()
    {
        Refresh();
    }

    public void HideLinks()
    {
        links.SetActive(false);
    }
    public void ShowLinks()
    {
        links.SetActive(true);
    }

    public void MarkAsSelected()
    {
        markingBehaviour.enabled = true;
    }
    public void UnMarkAsSelected()
    {
        markingBehaviour.enabled = false;
    }
}

public struct LinksGroup
{
    public string name;
    public string key;
    public SystemLink link;
}