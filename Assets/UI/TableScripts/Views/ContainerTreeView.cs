using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Other;
using System;
using MyLib;
using MyLib.Algoriphms;

public class ContainerTreeView : MonoBehaviour, IScrollable {

    [SerializeField]
    ContainerNodeView view;
    [SerializeField]
    float width;
    [SerializeField]
    float height;
    Forest<ContainerNodeView> tree = new Forest<ContainerNodeView>();
    protected Forest<ContainerNodeView> forest { get { return tree; } }
    float offset;
    [SerializeField]
    Transform content;

    public void SetTree(Forest<TreeNode<object>> tree)
    {
        Clear();
        this.tree = tree.BuildForest(n => CreateView(n.item));
        foreach (var t in this.tree.trees)
            t.transform.SetParent(content.transform);
        foreach (var n in this.tree.ByElements())
            n.view = view;

        UpdateNodes();
    }
    public Forest<T> GetTree<T>(Func<object, T> selector) where T: ITreeable<T>
    {
        return tree.BuildForest(n => selector(n.GetModel()));
    }
    public void Clear()
    {
        foreach (var t in tree.trees)
            Destroy(t.gameObject);
        tree = new Forest<ContainerNodeView>();
    }
    ContainerNodeView CreateView(object item)
    {
        var node = Instantiate(view);
        node.SetModel(item);
        return node;
    }

    public void AddNode(object item)
    {
        var node = Instantiate(view);
        node.view = view;
        node.SetModel(item);
        AddNode(node);
    }
    public void AddNode(ContainerNodeView node)
    {
        node.transform.SetParent(content);
        tree.AddTree(node);
        UpdateNodes();
    }
    public ContainerNodeView GetNode(object model)
    {
        foreach(var n in tree.ByElements())
            if (n.GetModel() == model)
                return n;
        return null;
    }
    public bool RemoveNode(ContainerNodeView node)
    {
        return tree.RemoveTree(node);
    }

    public void UpdateNodes()
    {
        int level = 0;
        
        foreach (var ns in tree.ByLevels())
        {
            int size = ns.Count();
            int index = 0;
            foreach(var n in ns)
            {
                n.transform.position = GetPosition(size, index, level);
                index++;
            }
            level++;
        }
        OnUpdateNodes();
    }
    protected virtual void OnUpdateNodes()
    {

    }
    Vector3 GetPosition(int levelSize, int levelIndex, int level)
    {
        float x = -(width * (levelSize - 1) / 2) + width * levelIndex;
        return content.position + new Vector3(x, level * height, 0);
    }

    public void Scroll(float value)
    {
        Vector3 start = content.transform.position - new Vector3(0, offset);
        offset += value;
        if (offset > 0)
            offset = 0;
        content.transform.position = start + new Vector3(0, offset);
    }
    public int Count()
    {
        return tree.ByElements().Count();
    }
}
