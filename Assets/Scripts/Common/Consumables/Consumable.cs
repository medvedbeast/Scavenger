using UnityEngine;

namespace Common
{
    [System.Serializable]
    class Consumable : Storable
    {
        public Consumables type;
        public int charges;
        public int maxCharges;
        public float amount;

        public virtual bool Consume(GameObject subject)
        {
            return false;
        }
    }

}