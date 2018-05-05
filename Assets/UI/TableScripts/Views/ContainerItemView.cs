using System;
using UnityEngine;
using System.Collections;

public class ContainerItemView : MonoBehaviour {

    object model;
    [SerializeField]
    string label;
    public string labelItem { get { return label; } }
    public void SetModel(object model)
    {
        RemoveFromModelView();

        this.model = model;
        OnSetModel(ref this.model);

        AddToModelView();

    }
    void AddToModelView()
    {
        if (model is ValueType)
            return;
        ModelViewBehaviour mv = null;// = GetComponentInParent<ModelViewBehaviour>();
        var parent = transform;
        while (parent != null)
        {
            mv = parent.GetComponent<ModelViewBehaviour>();
            if (mv != null)
                break;
            parent = parent.parent;
        }

        if (mv != null)
            mv.AddModelView(this.model, this);
    }
    void RemoveFromModelView()
    {
        if (model != null)
        {
            if (model is ValueType)
                return;
            var mv = GetComponentInParent<ModelViewBehaviour>();
            if (mv != null)
                mv.RemoveView(model, this);
        }
    }
    protected virtual void OnSetModel(ref object model)
    {
    }
    public virtual object GetModel()
    {
        return model;
    }
    public virtual T GetModel<T>()
    {
        return (T)model;
    }
    public void SetLabel(string label)
    {
        if (this.label != null)
            throw new System.Exception("ContainerItemView already has label");
        this.label = label;
    }
    public virtual void Release(System.Action<GameObject> action = null)
    {
        if (action == null)
            Destroy(gameObject);
        else
            action(gameObject);
    }

    protected void OnDestroy()
    {
        if (model != null)
        {
            var mv = GetComponentInParent<ModelViewBehaviour>();
            if (mv != null)
                mv.RemoveView(model, this);
        }
    }
}
