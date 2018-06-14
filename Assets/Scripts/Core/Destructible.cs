using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour
{
    public Module referencedObject;
    public GameObject root;

    public event System.Action OnUpdate;
    public event System.Action<Destructible> OnDestroy;
    public event System.Action<Destructible> OnHpAmountChanged;

    public void Start()
    {

    }

    public void Update()
    {
        if (OnUpdate != null)
        {
            OnUpdate();
        }
        else
        {
            if (referencedObject != null)
            {
                if (referencedObject.IsUpdateable())
                {
                    OnUpdate += referencedObject.Update;
                }
            }
        }
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 9)
        {
            Projectile p = c.GetComponent<Projectile>();
            if (p.owner == this.root || referencedObject.hp <= 0)
            {
                return;
            }

            Weapon w = p.ownerWeapon;
            referencedObject.hp -= w.damage * (100 / (100 - referencedObject.armor));

            if (OnHpAmountChanged != null)
            {
                OnHpAmountChanged(this);
            }

            if (referencedObject.hp <= 0)
            {
                if (OnDestroy != null)
                {
                    OnDestroy(this);
                }
            }
            if (!p.penetrative)
            {
                GameObject.Destroy(c.gameObject);
            }
        }
    }
}
