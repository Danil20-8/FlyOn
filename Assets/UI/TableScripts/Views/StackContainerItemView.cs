using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

public class StackContainerItemView: ContainerItemView
{

    Stack model { get { return (Stack)base.GetModel(); } }

    protected override void OnSetModel(ref object model)
    {
        if (!(model is Stack))
            throw new Exception("StackContainerItemView has to have Stack type data");


    }
    public override object GetModel()
    {
        return model.Peek();
    }
    public override T GetModel<T>()
    {
        return (T) model.Peek();
    }
    public int Count()
    {
        return model.Count;
    }
    public bool IsEmpty()
    {
        return Count() == 0;
    }
    public override void Release(Action<GameObject> action = null)
    {
        ((Stack)model).Pop();
        if (IsEmpty())
            base.Release(action);
    }
}

