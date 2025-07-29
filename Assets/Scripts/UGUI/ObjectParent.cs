using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectParent
{
    public GameObject m_GameObject { get; set; }

    public Transform m_Transform { get; set; }

    public string Name { get; set; }

    public string HotFixClassName { get; set; }

    public virtual void Awake(object param1 = null, object param2 = null, object param3 = null)
    {

    }

    public virtual void OnShow(object param1 = null, object param2 = null, object param3 = null)
    {
    }

    public virtual void OnDisable() { }

    public virtual void OnUpdate() { }

    public virtual void OnFixUpdate() { }

    public virtual void OnLateUpdate() { }

    public virtual void OnClose()
    {

    }
}
