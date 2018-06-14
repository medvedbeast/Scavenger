using UnityEngine;

[System.Serializable]
class Consumable : Storable
{
    public Consumables type;
    public int charges;
    public int maxCharges;
    public float amount;

    public virtual void Consume()
    { }
}

