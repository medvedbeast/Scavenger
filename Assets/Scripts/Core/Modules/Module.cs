using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Module : Storable
{
    public float hp;
    public float maxHp;
    public float armor;
    public Modules type;

    public virtual void Update()
    {
        return;
    }

    public virtual bool IsUpdateable()
    {
        return false;
    }

}

