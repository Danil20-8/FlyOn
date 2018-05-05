using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.GameModels;
using Assets.UI.BattleView.PlayerUIView;

[RequireComponent(typeof(ModelViewBehaviour))]
public class PlayerUIBehaviour : SlideBehaviour {

    [SerializeField]
    ContainerListView linkGroups;

    [SerializeField]
    ContainerListView fragList;

    [SerializeField]
    GameObject[] dependeceds;

    LinksGroupViewBehaviour[] groups;

    ModelViewBehaviour mvb;

    ThemeListener theme;

    public TargetView targetView { get; private set; }

    void Awake()
    {
        mvb = GetComponent<ModelViewBehaviour>();
        theme = GetComponent<ThemeListener>();
        targetView = new TargetView(GetComponent<Canvas>());
    }

    public void AddFrag()
    {
        fragList.AddItem(new object());
    }

    public void SetUI(LinksGroup[] groups)
    {
        linkGroups.Clear();

        linkGroups.AddRange(groups);

        this.groups = groups.Select(g => mvb.GetView<LinksGroupViewBehaviour>(g.link)).ToArray();

        theme.UpdateTheme();
    }
    public void UpdateGroup(SystemLink link)
    {
        mvb.GetView<LinksGroupViewBehaviour>(link).Refresh();
    }
    public void UpdateGroup(SystemLink link, Action<LinksGroupViewBehaviour> action)
    {
        action(mvb.GetView<LinksGroupViewBehaviour>(link));
    }


    bool linksHided = false;
    void HideShowLinks()
    {
        if(linksHided)
            foreach (var g in groups)
                g.ShowLinks();
        else
            foreach (var g in groups)
            g.HideLinks();

        linksHided = !linksHided;
    }

    bool groupsHided;
    void HideShowGroups()
    {
        if (groupsHided)
            foreach (var g in groups)
                g.gameObject.SetActive(true);
        else
            foreach (var g in groups)
                g.gameObject.SetActive(false);

        groupsHided = !groupsHided;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            HideShowLinks();
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            HideShowGroups();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            screenSlider.MoveTo("MenuView");
        if (Input.GetKeyDown(KeyCode.Pause))
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
    }

    public override void OnSlide(params object[] args)
    {
        BattleBehaviour.current.paused = false;
        Cursor.visible = false;

        foreach (var d in dependeceds)
            d.SetActive(true);
    }
    public override void OnOut()
    {
        foreach (var d in dependeceds)
            d.SetActive(false);
    }
}
