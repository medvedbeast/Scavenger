using UnityEngine;

namespace Common
{
    [System.Serializable]
    class FuelAdditive : Consumable
    {
        public override bool Consume(GameObject subject)
        {
            return false;
        }
    }

}