using UnityEngine;
using System.Collections;

namespace Common
{
    public class Projectile : MonoBehaviour
    {

        public GameObject owner;
        public Weapon ownerWeapon;
        public Vector3 direction;
        public float lifetime;
        public bool penetrative;

        public void Start()
        {

        }

        public void FixedUpdate()
        {
            if (lifetime > 0)
            {
                lifetime -= Time.deltaTime;
            }
            else
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}