using UnityEngine;
using Assets.GameModels;
using Assets.Global;
using System.Collections.Generic;
using System;
using System.Collections;

public abstract class DirectableComponentBehaviour: ShipComponentBehaviour
{
    Quaternion parentRotation;
    Quaternion localClipedDirection;


    protected abstract IEnumerator<Quaternion> GetDirections();

    public bool IsDirecting()
    {
        var dir = GetDirections();
        while (dir.MoveNext())
        {
            var localDirection = Quaternion.Inverse(parentRotation) * dir.Current;

            localClipedDirection = ClipLocalRotation(localDirection);
            //if(Quaternion.Dot(localDirection, localClipedDirection) > Constants.deg_15)
            if (Quaternion.Angle(localDirection, localClipedDirection) < 15)
            {
                dir.Dispose();
                return true;
            }
        }
        dir.Dispose();
        return false;
    }
    public bool IsAble(Quaternion rotation)
    {
        var localDirection = Quaternion.Inverse(parentRotation) * rotation;

        var localClipedRotation = ClipLocalRotation(localDirection);
        //if(Quaternion.Dot(localDirection, localClipedDirection) > Constants.deg_15)
        if (Quaternion.Angle(localDirection, localClipedRotation) < 15)
            return true;
        else
            return false;
    }
    public void Direct(float speed)
    {
        this.localRotation = Quaternion.RotateTowards(localRotation, localClipedDirection, speed);
    }
    protected override void InitializeUpdate()
    {
        parentRotation = driver.ship.transform.rotation;
        
        tempTransform.position = transform.position;
        tempTransform.rotation = transform.rotation;
        
    }
    /*protected override void SlowUpdate()
    {
        transform.localRotation = localRotation;
    }*/

    protected Quaternion ClipLocalRotation(Quaternion localRotation)
    {
        float angle = Quaternion.Angle(tacklePlace.clipRotation, localRotation);
        if (angle > tacklePlace.clipAngle)
            return Quaternion.Slerp(tacklePlace.clipRotation, localRotation, tacklePlace.clipAngle / angle);
        else
            return localRotation;
    }
}