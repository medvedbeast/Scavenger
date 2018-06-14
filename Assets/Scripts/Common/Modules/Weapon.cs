using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

namespace Common
{
    [System.Serializable]
    public class Weapon : Module
    {
        public float damage;
        public float attackSpeed;
        public string projectileName;
        public Weapons weaponType;
        float rechargeTime = 0;

        public override void Update()
        {
            if (rechargeTime > 0)
            {
                rechargeTime -= Time.deltaTime;
            }
        }

        public override bool IsUpdateable()
        {
            return true;
        }

        public void Shoot(Destructible subject, GameObject rootObject)
        {
            if (rechargeTime <= 0)
            {
                switch (weaponType)
                {
                    case Weapons.MACHINE_GUN:
                        {
                            GameObject p = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/" + projectileName)) as GameObject;
                            p.name = projectileName;
                            p.GetComponent<Projectile>().owner = rootObject;
                            p.GetComponent<Projectile>().ownerWeapon = this;
                            p.transform.position = new Vector3(subject.transform.position.x, 2, subject.transform.position.z);

                            p.transform.Rotate(0, subject.transform.rotation.eulerAngles.y, subject.transform.rotation.eulerAngles.z);
                            p.transform.SetParent(GameObject.Find("Projectiles").transform);

                            System.Random r = new System.Random();
                            float focusRange = rootObject.GetComponent<Unit>().focusRange;
                            Vector3 direction = (rootObject.transform.position + (rootObject.transform.forward * focusRange)) - subject.transform.position;

                            p.GetComponent<Rigidbody>().AddForce(direction.normalized * 150, ForceMode.VelocityChange);
                            rechargeTime = 60 / attackSpeed;
                            break;
                        }
                    case Weapons.ROCKET_LAUNCHER:
                        {
                            GameObject p = GameObject.Instantiate(Resources.Load("Prefabs/Projectiles/" + projectileName)) as GameObject;
                            p.name = projectileName;
                            p.GetComponent<Projectile>().owner = rootObject;
                            p.GetComponent<Projectile>().ownerWeapon = this;
                            p.transform.position = new Vector3(subject.transform.position.x, 2, subject.transform.position.z);

                            p.transform.Rotate(0, subject.transform.rotation.eulerAngles.y, subject.transform.rotation.eulerAngles.z);
                            p.transform.SetParent(GameObject.Find("Projectiles").transform);

                            System.Random r = new System.Random();

                            Vector3 direction = subject.transform.forward.normalized * 150;
                            direction = new Vector3(direction.x + (1 - r.Next(0, 3)), 1, direction.z + (1 - r.Next(0, 3)));

                            p.GetComponent<Rigidbody>().AddForce(direction, ForceMode.VelocityChange);
                            rechargeTime = 60 / attackSpeed;
                            break;
                        }
                }
            }
        }
    }
}