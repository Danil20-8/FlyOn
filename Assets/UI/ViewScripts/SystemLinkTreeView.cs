using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Other;
using System;
using MyLib;
using MyLib.Algoriphms;
using Assets.GameModels;
public class SystemLinkTreeView : MonoBehaviour, IScrollable
{

    [SerializeField]
    SystemLinkView view;
    [SerializeField]
    float width;
    [SerializeField]
    float height;
    float offset;
    [SerializeField]
    Transform content;
    Forest<SystemLinkView> system;
    public void SetTree(Forest<SystemLink> tree)
    {
        system = tree.BuildForest(n => CreateView(n));
        foreach (var c in system.trees)
            c.transform.SetParent(content);
        UpdateNodes();
    }
    SystemLinkView CreateView(SystemLink item)
    {
        var node = Instantiate(view);
        node.SetLink(item);
        return node;
    }

    public void UpdateNodes()
    {
        int level = 0;

        foreach (var ns in system.ByLevels())
        {
            int size = ns.Count();
            int index = 0;
            foreach (var n in ns)
            {
                n.transform.position = GetPosition(size, index, level);
                index++;
            }
            level++;
        }
    }
    Vector3 GetPosition(int levelSize, int levelIndex, int level)
    {
        var w = width * levelSize;
        float x = w / levelSize * levelIndex - (levelSize > 1 ? w * .5f : 0);
        return content.position + new Vector3(x, level * height, 0);
    }

    public void Scroll(float value)
    {
        Vector3 start = content.position - new Vector3(0, offset);
        offset += value;
        if (offset > 0)
            offset = 0;
        content.position = start + new Vector3(0, offset);
    }
}
