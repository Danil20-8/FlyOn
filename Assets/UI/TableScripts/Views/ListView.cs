using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using Assets.Other;

public class ListView : View, IScrollable {


    [SerializeField]
    float height;
    [SerializeField]
    float width;
    [SerializeField]
    protected Transform content;
    float offset = 0;
    public Func<Transform, float> keySelector;
    void Start()
    {
        Refresh();
    }
    public void AddItem(Transform item)
    {
        item.gameObject.AddComponent<View>();
        item.transform.SetParent(content);
        Refresh();
    }
    public void AddRange(IEnumerable<Transform> items)
    {
        foreach(var i in items)
        {
            i.gameObject.AddComponent<View>();
            i.transform.SetParent(content);
        }
        Refresh();
    }

    public Transform GetItem(int index)
    {
        return content.GetChild(index);
    }

    public void Clear()
    {
        foreach (var i in content.childs())
        {
            i.transform.SetParent(null);
            Destroy(i.gameObject);
        }
    }
    public override void Refresh()
    {
        Transform[] childs;
        if (keySelector != null)
            childs = content.childs().OrderBy(keySelector).Reverse().ToArray();
        else
            childs = content.childs();
        for (int i = 0; i < childs.Length; i++)
            childs[i].transform.localPosition = GetPosition(i);
    }
    Vector3 GetPosition(int index)
    {
        return new Vector3(width * index, - height * index, 0f);
    }

    public void Scroll(float value)
    {
        content.position = content.position - new Vector3(0, offset);
        offset += value;
        if (offset > content.childCount * height)
            offset = content.childCount * height;
        else if (offset < 0)
            offset = 0;
        content.position = content.position + new Vector3(0, offset);
    }
}
