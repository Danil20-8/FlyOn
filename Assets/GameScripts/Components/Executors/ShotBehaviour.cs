using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Other.Special;
using Assets.Other;

public abstract class ShotBehaviour : MonoBehaviour
{
    public static void ClearBehaviour() { shots = new ShotBehaviour[100]; length = 0; }
    static ShotBehaviour[] shots = new ShotBehaviour[100];
    static int length = 0;
    int _index;


    Vector3 _forward;
    protected Vector3 _back;
    float _distance;
    float _speed;

    Transform _friend;
    float _startTime;
    float _lifeTime;

    protected abstract void HitSomething(RaycastHit hit);
    protected abstract void Destroy();
    protected virtual void EndUpdate()
    {

    }
    protected void AddShot(float speed, float lifeTime, Transform friend)
    {
        //Add Shot to Update cicle
        if (length == shots.Length)
        {
            ShotBehaviour[] ss = new ShotBehaviour[shots.Length * 2];
            for (int i = 0; i < shots.Length; i++)
                ss[i] = shots[i];
            shots = ss;
        }
        shots[length] = this;
        _index = length;
        length++;

        // ShotInit
        _speed = speed;
        _lifeTime = lifeTime;
        _friend = friend;

        _forward = transform.forward;
        _back = transform.position;
        _startTime = Time.time;

    }
    protected void RemoveShot()
    {
        //Remove shot from Update cicle
        shots[_index] = shots[length - 1];
        shots[_index]._index = _index;
        length -= 1;
    }
    public static void UpdateAll()
    {
        for (int i = 0; i < length; i++)
            if (shots[i].gameObject.activeSelf)
                shots[i].UpdateShot();
    }
    // Update is called once per frame
    void UpdateShot()
    {
        _back = transform.position;
        _distance = _speed * Time.deltaTime;
        transform.position += _forward * _distance;
        if (CheckCollision())
            return;
        EndUpdate();
        if (Time.time - _startTime > _lifeTime)
            Destroy();
    }

    bool CheckCollision()
    {

        RaycastHit[] hits = Physics.RaycastAll(_back, transform.position - _back, _distance);
        
        if (hits.Length == 0)
            return false;
        RaycastHit hit;
        if (hits.WithMin(h => (h.point - _back).sqrMagnitude, h => !h.transform.IsOne(_friend), out hit))
        {
            HitSomething(hit);
            return true;
        }
        return false;
    }
}

