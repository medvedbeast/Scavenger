using UnityEngine;
using Common;

namespace Space
{
    [System.Serializable]
    class Fuel : Consumable
    {
        public override bool Consume(GameObject subject)
        {

            Unit u = subject.GetComponent<Unit>();
            float fuelLack = u.maxFuel - u.fuel;

            if (amount < fuelLack)
            {
                u.fuel += amount;
                amount = 0;
                u.inventory.Remove(this.slot);

                GameObject.Find("Core").GetComponent<Events>().OnFuelChanged();
                return true;
            }
            else
            {
                u.fuel = u.maxFuel;
                amount -= fuelLack;

                GameObject.Find("Core").GetComponent<Events>().OnFuelChanged();
                return false;
            }
        }
    }
}