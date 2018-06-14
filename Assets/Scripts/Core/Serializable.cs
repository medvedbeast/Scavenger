using UnityEngine;
using System.Collections;
using System;

[AttributeUsage(AttributeTargets.All)]
public class Serializable : Attribute
{
    private bool state = false; 

    public Serializable(bool state)
    {
        this.state = state;
    }
}
