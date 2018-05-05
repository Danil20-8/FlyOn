using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Other;
using System;
using System.Collections;
using MyLib;

public class ContainerNodeView : ContainerItemView, ITreeable<ContainerNodeView> {


    public ContainerNodeView view;
    public ContainerNodeView root{ get { return transform.parent == null ? null : transform.parent.GetComponent<ContainerNodeView>(); } set { transform.SetParent(value == null ? null : value.transform); GetTree(t => t.UpdateNodes()); } }

    public IEnumerable<ContainerNodeView> childs{ get { return transform.childs<ContainerNodeView>(); } }


    public CanvasLineBeahviour line;

    void Update()
    {
        if (root != null)
        {
            line.gameObject.SetActive(true);
            line.SetLine(transform.position, root.transform.position);
        }
        else
        {
            line.gameObject.SetActive(false);
        }
    }

    public ContainerTreeView GetTree()
    {
        return transform.GetComponentInParents<ContainerTreeView>();
    }
    public void GetTree(Action<ContainerTreeView> action)
    {
        var t = transform.GetComponentInParents<ContainerTreeView>();
        if (t != null)
            action(t);
    }
    public void AddChild(ContainerNodeView child)
    {
        GetTree(t => t.RemoveNode(child));
    }

    public void RemoveChild(ContainerNodeView child)
    {
        child.transform.SetParent(null);
        GetTree().UpdateNodes();
    }
    public bool ReleaseNode()
    {
        if (childs.Any())
            return false;
        if (GetTree().RemoveNode(this))
            return true;
        this.Release<ContainerNodeView>();
        return true;
    }
    public void AddNode(object item)
    {
        var node = Instantiate(view);
        node.view = view;
        node.SetModel(item);
        this.AddNode<ContainerNodeView>(node);
    }
}
