using UnityEngine;
using System.Collections;

namespace Common
{
    [System.Serializable]
    public class Engine : Module
    {
        public float moveSpeed;
        public float fuel;
        public float maxFuel;
        public float fuelSpendRate;
        public float carryingPower;
    }
}