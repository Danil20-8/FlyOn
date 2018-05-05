using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.GameModels.Campaign;
using Assets.Other;
using MyLib;

public class EditorStageView : ContainerItemView {

    [SerializeField]
    Transform enterPoint;

    [SerializeField]
    Transform exitPoint;

    [SerializeField]
    StageConnector arrow;

    [SerializeField]
    Text nameText;

    [SerializeField]
    Image battleIndicator;

    [SerializeField]
    Image backAble;

    HashSet<EditorStageView> forwardStages = new HashSet<EditorStageView>();
    HashSet<EditorStageView> refStages = new HashSet<EditorStageView>();

    GameObjectPool<StageConnector> arrows;

    Campaign.Stage stage;

    void Awake()
    {
        arrows = new GameObjectPool<StageConnector>(arrow);
    }

    DragBehaviour dragBehaviour;
    void Start()
    {
        dragBehaviour = GetComponent<DragBehaviour>();
        dragBehaviour.onDragOverride = OnDrag;
    }
    void OnDrag(Vector3 position)
    {
        dragBehaviour.BaseOnDrag(position);
        foreach (var d in dependenceds)
            d.UpdateView();
        UpdateView();
    }
    protected override void OnSetModel(ref object model)
    {
        stage = (Campaign.Stage)model;

        nameText.text = stage.actionString;
        battleIndicator.gameObject.SetActive(stage.battle);

        backAble.gameObject.SetActive(!stage.backAble);
    }

    List<EditorStageView> dependenceds = new List<EditorStageView>();

    public void SetBackAble()
    {
        stage.backAble = !stage.backAble;
        backAble.gameObject.SetActive(!stage.backAble);
    }

    public void SetRef(EditorStageView stage)
    {
        if (stage != this)
        {
            if (this.stage.forwardStages.Contains(stage.stage) || this.stage.refStages.Contains(stage.stage) || this.stage.IsRoot(stage.stage))
                return;
            this.stage.refStages.Add(stage.stage);

            refStages.Add(stage);
            stage.dependenceds.Add(this);

            UpdateView();
        }
    }
    public void AddForward(EditorStageView stage)
    {
        if (stage != this)
        {
            if (this.stage.forwardStages.Contains(stage.stage))
            {
                Debug.Log("Already contains");
                return;
            }

            this.stage.AddNode(stage.stage);
            //this.stage.forwardStages.Add(stage.stage);

            if (this.stage.refStages.Contains(stage.stage))
            {
                this.stage.refStages.Remove(stage.stage);
                refStages.Remove(stage);
            }

            forwardStages.Add(stage);
            stage.dependenceds.Add(this);

            UpdateView();
        }
        else
            Debug.Log("this");
    }

    public bool Remove(EditorStageView stage, bool destroy = false)
    {
        if (forwardStages.Contains(stage))
        {

            if (destroy && (stage.stage.forwardStages.Count > 0 || stage.stage.refStages.Count > 0))
                return false;

            forwardStages.Remove(stage);
            this.stage.RemoveNode(stage.stage);
            stage.dependenceds.Remove(this);

            if (destroy)
                stage.Destroy();

        }
        else if (refStages.Remove(stage))
        {
            this.stage.refStages.Remove(stage.stage);
        }
        else
            throw new System.Exception(this + " not contains " + stage);

        UpdateView();
        return true;
    }

    public void Destroy()
    {
        foreach (var d in dependenceds.ToArray())
            d.Remove(this);

        arrows.Clear();
        FindObjectOfType<CampaignEditorBehaviourView>().RemoveStage(this);

        Destroy(gameObject);
    }

    public void UpdateView()
    {
        using (var arrs = arrows.Get(stage.forwardStages.Count + stage.refStages.Count).GetEnumerator())
        {
            foreach (var s in forwardStages)
            {
                arrs.MoveNext();
                var sc = arrs.Current;

                sc.transform.SetParent(transform, false);
                sc.SetLine(s.enterPoint.position, exitPoint.position);
                sc.color = Color.yellow;
                sc.origin = this;
                sc.end = s;
                sc.isMain = true;
            }
            foreach (var s in refStages)
            {
                arrs.MoveNext();
                var sc = arrs.Current;

                sc.transform.SetParent(transform, false);
                sc.SetLine(s.enterPoint.position, exitPoint.position);
                sc.color = Color.blue;
                sc.origin = this;
                sc.end = s;
                sc.isMain = false;
            }
        }
    }
    public static void ResetCouples(IEnumerable<EditorStageView> stages)
    {
        foreach(var s in stages)
        {
            s.dependenceds.Clear();
            s.forwardStages.Clear();
            s.refStages.Clear();
        }


        foreach (var c in stages)
            foreach (var s in stages)
            {
                if (s == c) continue;

                if (c.stage.forwardStages.Contains(s.stage))
                {
                    c.forwardStages.Add(s);
                    s.dependenceds.Add(c);
                }
                else if (c.stage.refStages.Contains(s.stage))
                {
                    c.refStages.Add(s);
                    s.dependenceds.Add(c);
                }
            }

        foreach (var s in stages)
            s.UpdateView();
    }
}

