using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Other;

public class TableView: MonoBehaviour, IScrollable
{
    [SerializeField]
    ContainerItemView lineView;
    int lineCount = 0;

    [SerializeField]
    TableColumn[] columns;

    ContainerItemView[] columnsView;

    float height;
    float width;

    [SerializeField]
    Transform content;

    float offset = 0;

    Func<object, object>[] selectors;

    protected Func<ContainerItemView, IComparable>[] keySelector;
    protected int? keyColumn = null;
    bool reverse = false;
    void Awake()
    {
        columnsView = columns.Select(c => c.view).ToArray();
        height = columnsView.Max(c => c.GetComponent<RectTransform>().rect.height);
        width = GetComponent<RectTransform>().rect.width / (columnsView.Length);

        UpdateColumns(lineView.GetComponent<RectTransform>(), columns.Select(c => c.name).ToArray(), columns.Select(c => c.titleView).ToArray());

        for (int i = 0; i < lineView.transform.childCount; i++)
        {
            int n = i;
            lineView.transform.GetChild(i).gameObject.AddComponent<PointerEventHandlerBehaviour>()
                .SetEvents(() => OrderBy(n, keyColumn == n ? !reverse : false));
        }
    }

    void Start()
    {
        Refresh();
    }

    void UpdateColumns(RectTransform line, object[] items, ContainerItemView[] views)
    {
        foreach (var c in line.childs())
            DestroyImmediate(c.gameObject);

        int n = 0;
        items.Select(views, (i, c) => {
            var column = (ContainerItemView)Instantiate(c, Vector3.zero, Quaternion.identity, line);

            column.GetComponent<RectTransform>().anchorMin = new Vector2(1 / items.Length * n, 0);

        Elf.With(column.GetComponent<RectTransform>(),
                rt => rt.anchorMin = new Vector2(1f / items.Length * n, 0),
                rt => rt.anchorMax = new Vector2(1f / items.Length * (n + 1), 1),
                rt => rt.anchoredPosition = Vector2.zero,
                rt => rt.sizeDelta =Vector2.Scale(rt.sizeDelta, new Vector2(0, 1))
                );

            column.SetModel(i);

            n++;
            return column;
        }).Count();
    }

    ContainerItemView AddLine(Transform parent, object[] items, ContainerItemView[] views)
    {
        if (items.Length != views.Length)
            throw new System.Exception("the number of items must be equal to the number of columns");

        var l = ((ContainerItemView)Instantiate(lineView, new Vector3(0, height + 5, 0) * lineCount, Quaternion.identity, parent)).GetComponent<RectTransform>();

        l.sizeDelta = new Vector2(0, height);
        
        //l.anchorMin = new Vector2(0, 1 - height / GetComponent<RectTransform>().rect.height);
        lineCount++;

        UpdateColumns(l, items, views);

        return l.GetComponent<ContainerItemView>();
    }

    public void SetModel<T>(params Func<T, object>[] selectors)
    {
        this.selectors = selectors.Select<Func<T, object>, Func<object, object>>(f => (o) => f((T)o)).ToArray();
    }

    public void AddLine<T>(T model)
    {
        AddLine(content, selectors.Select(f => f(model)).ToArray(), columnsView)
            .SetModel(model);
        Refresh();
    }
    public void AddRange<T>(IEnumerable<T> models)
    {
        foreach (var m in models)
            AddLine(content, selectors.Select(f => f(m)).ToArray(), columnsView)
                .SetModel(m);
        Refresh();
    }

    public void Clear()
    {
        foreach (var i in content.childs())
        {
            i.transform.SetParent(null);
            Destroy(i.gameObject);
        }
    }
    public void Refresh()
    {
        Transform[] childs;
        if (keyColumn.HasValue)
            if(reverse)
                childs = content.childs().OrderBy(c => keySelector[keyColumn.Value](c.GetComponent<ContainerItemView>())).Reverse().ToArray();
            else
                childs = content.childs().OrderBy(c => keySelector[keyColumn.Value](c.GetComponent<ContainerItemView>())).ToArray();
        else
            childs = content.childs();
        for (int i = 0; i < childs.Length; i++)
            childs[i].transform.position = GetPosition(i);
    }
    Vector3 GetPosition(int index)
    {
        return content.position + new Vector3(0f, -height * index, 0f);
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
    public void SetOrder<T>(params Func<T, IComparable>[] keySelector)
    {
        this.keySelector = keySelector.Select(k => new Func<ContainerItemView, IComparable>(t => k(t.GetModel<T>()))).ToArray();
    }
    public void OrderBy(int column, bool reverse = false)
    {
        this.keyColumn = column;
        this.reverse = reverse;
        Refresh();
    }

}

[Serializable]
public class TableColumn
{
    public ContainerItemView view;
    public ContainerItemView titleView;
    public string name;
}