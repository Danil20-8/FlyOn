using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MyLib;
using Assets.GameModels;
using Assets.Other;
[RequireComponent(typeof(LineRenderer))]
public class SystemLinkView : MonoBehaviour, ITreeable<SystemLinkView>
{
    SystemLink link;
    public IEnumerable<SystemLinkView> childs
    {
        get
        {
            return transform.childs<SystemLinkView>();
        }
    }

    public SystemLinkView root
    {
        get
        {
            return transform.parent == null ? null : transform.parent.GetComponent<SystemLinkView>();
        }

        set
        {
            if (value == null)
            {
                GetComponent<LineRenderer>().SetVertexCount(0);
                transform.SetParent(null);
            }
            else
            {
                GetComponent<LineRenderer>().SetVertexCount(2);
                GetComponent<LineRenderer>().SetPositions(new Vector3[] { transform.position, value.transform.position });
                transform.SetParent(value.transform);
            }
        }
    }

    public void AddChild(SystemLinkView child)
    {
    }

    public void RemoveChild(SystemLinkView child)
    {
        child.root = null;
    }
    public void SetLink(SystemLink link)
    {
        this.link = link;
    }
    void Start()
    {
        if (GetComponent<LineRenderer>() == null)
            this.enabled = false;
    }
    void Update()
    {
        if (transform.parent == null)
            return;
        GetComponent<LineRenderer>().SetVertexCount(2);
        GetComponent<LineRenderer>().SetPositions(new Vector3[] { transform.position, transform.parent.transform.position });
        var info = link.GetInfo();
        Color c;
        if (info.enabled)
            c = Color.Lerp(Color.red, Color.blue, Mathf.Clamp(link.GetInfo().lastEnergy, 0f, 1f));
        else
            c = Color.gray;
        GetComponent<LineRenderer>().material.color = c;
    }
}

