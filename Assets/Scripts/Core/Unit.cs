using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public Units type;

    public float capacity
    {
        get
        {
            float tmp = 0;
            for (int i = 0; i < modules.Count; i++)
            {
                if (modules[i].referencedObject.hp > 0)
                    tmp += modules[i].referencedObject.weight;
            }
            for (int i = 0; i < inventory.itemList.Count; i++)
            {
                tmp += inventory.itemList[i].weight;
            }
            return tmp;
        }
    }
    public float maxCapacity
    {
        get
        {
            float tmp = 0;
            float tmp2 = 0;
            for (int i = 0; i < modules.Count; i++)
            {
                if (modules[i].referencedObject.type == Modules.BODY)
                {
                    if (modules[i].referencedObject.hp > 0)
                        tmp += (modules[i].referencedObject as Body).maxCapacity;
                }
                else
                {
                    if (modules[i].referencedObject.type == Modules.FREIGHT)
                    {
                        if (modules[i].referencedObject.hp > 0)
                            tmp += (modules[i].referencedObject as Freight).maxCapacity;
                    }
                }
                if (modules[i].referencedObject.type == Modules.ENGINE)
                {
                    tmp2 += (modules[i].referencedObject as Engine).carryingPower;
                }
            }
            return tmp < tmp2 ? tmp : tmp2;
        }
    }
    public float moveSpeed
    {
        get
        {
            float tmp = 0;
            for (int i = 0; i < modules.Count; i++)
            {
                if (modules[i].referencedObject.type == Modules.ENGINE)
                {
                    if (modules[i].referencedObject.hp > 0)
                        tmp += (modules[i].referencedObject as Engine).moveSpeed;
                }
            }
            return tmp;
        }
    }
    public float fuel
    {
        get
        {
            float tmp = 0;
            for (int i = 0; i < modules.Count; i++)
            {
                if (modules[i].referencedObject.type == Modules.ENGINE)
                {
                    if (modules[i].referencedObject.hp > 0)
                        tmp += (modules[i].referencedObject as Engine).fuel;
                }
            }
            return tmp;
        }
        set
        {
            for (int i = 0; i < modules.Count; i++)
            {
                if (modules[i].referencedObject.type == Modules.ENGINE)
                {
                    if (modules[i].referencedObject.hp > 0 && (modules[i].referencedObject as Engine).fuel > 0)
                    {
                        float amount = fuel - value;
                        (modules[i].referencedObject as Engine).fuel -= amount;
                        return;
                    }
                }
            }
            if (OnFuelAmountChanged != null)
            {
                OnFuelAmountChanged();
            }
        }
    }
    public float maxFuel
    {
        get
        {
            float tmp = 0;
            for (int i = 0; i < modules.Count; i++)
            {
                if (modules[i].referencedObject.type == Modules.ENGINE)
                {
                    if (modules[i].referencedObject.hp > 0)
                        tmp += (modules[i].referencedObject as Engine).maxFuel;
                }
            }
            return tmp;
        }
    }
    public float fuelSpendRate
    {
        get
        {
            float tmp = 0;
            for (int i = 0; i < modules.Count; i++)
            {
                if (modules[i].referencedObject.type == Modules.ENGINE)
                {
                    if (modules[i].referencedObject.hp > 0)
                        tmp += (modules[i].referencedObject as Engine).fuelSpendRate;
                }
            }
            return tmp;
        }
    }
    public float carryingPower
    {
        get
        {
            float tmp = 0;
            for (int i = 0; i < modules.Count; i++)
            {
                if (modules[i].referencedObject.type == Modules.ENGINE)
                {
                    if (modules[i].referencedObject.hp > 0)
                        tmp += (modules[i].referencedObject as Engine).carryingPower;
                }
            }
            return tmp;
        }
    }
    public float turnSpeed
    {
        get
        {
            float tmp = 0;
            for (int i = 0; i < modules.Count; i++)
            {
                if (modules[i].referencedObject.type == Modules.FLIGHT_CONTROL)
                {
                    if (modules[i].referencedObject.hp > 0)
                        tmp += (modules[i].referencedObject as FlightControl).turnSpeed;
                }
            }
            return tmp;
        }
    }
    public float maxHp
    {
        get
        {
            float tmp = 0;
            for (int i = 0; i < modules.Count; i++)
            {
                if (modules[i].referencedObject.hp > 0)
                    tmp += modules[i].referencedObject.maxHp;
            }
            return tmp;
        }
    }
    public float hp
    {
        get
        {
            float tmp = 0;
            for (int i = 0; i < modules.Count; i++)
            {
                if (modules[i].referencedObject.hp > 0)
                    tmp += modules[i].referencedObject.hp;
            }
            return tmp;
        }
    }
    public float armor
    {
        get
        {
            float tmp = 0;
            for (int i = 0; i < modules.Count; i++)
            {
                if (modules[i].referencedObject.hp > 0)
                    tmp += modules[i].referencedObject.armor;
            }
            return tmp;
        }
    }

    public List<Destructible> modules = new List<Destructible>();
    public Inventory inventory;

    public float focusRange = 150;
    public float minFocusRange = 35;
    public float maxFocusRange = 100;

    public event System.Action OnFuelAmountChanged;
    public event System.Action OnHpAmountChanged;


    void Start()
    {
        inventory = new Inventory(100);
        if (this.type != Units.LOOTABLE)
        {
            for (int i = 0; i < modules.Count; i++)
            {
                modules[i].referencedObject = Game.Deserialize<Module>(string.Format("{0}/{1}.module", gameObject.name, modules[i].name));
                modules[i].root = gameObject;
                modules[i].OnDestroy += OnModuleDestroyed;
                modules[i].OnHpAmountChanged += OnModuleDamaged;
                inventory.Add(Game.Clone(modules[i].referencedObject));
            }
        }
        else
        {
            List<string> f = new List<string>();
            string[] files = System.IO.Directory.GetFiles(System.Environment.CurrentDirectory + Game.pathToItems + "\\Items\\Modules\\SCA-01", "*.module");
            for (int i = 0; i < files.Length; i++)
            {
                f.Add(files[i].Substring(files[i].LastIndexOf('\\') + 1));
            }
            System.Random r = new System.Random();
            for (int i = 0; i < r.Next(1, 4); i++)
            {
                inventory.Add(Game.Deserialize<Module>("SCA-01/" + f[r.Next(0, f.Count - 1)]));
            }
            Fuel fuel = Game.Deserialize<Consumable>("fuel_tank.consumable") as Fuel;
            fuel.amount = r.Next(0, 451);
            inventory.Add(fuel);
        }
    }

    public void OnModuleDestroyed(Destructible sender)
    {
        if (sender.referencedObject.type == Modules.BODY)
        {
            if (this.GetComponent<ShipController>() != null)
            {
                Application.LoadLevel(0);
            }
            GameObject explosion = GameObject.Instantiate(Resources.Load("Prefabs/Particles/Explosion") as GameObject);
            explosion.transform.position = this.gameObject.transform.position;
            explosion.name = "Explosion";
            GameObject.Destroy(this.gameObject);
            explosion.GetComponentInChildren<ParticleSystem>().Play();
        }

        UI.AddMessage(new Message(sender.referencedObject.type + " destroyed!", 3.0f, Color.red));
    }

    public void OnModuleDamaged(Destructible subject)
    {
        if (OnHpAmountChanged != null)
        {
            OnHpAmountChanged();

            if (this.GetComponent<ShipController>() != null)
            {
                string name = subject.referencedObject.type.ToString().ToLower();
                name = char.ToUpper(name[0]) + name.Substring(1);
                if (name.IndexOf("_") >= 0)
                {
                    name = name.Substring(0, name.IndexOf('_')) + char.ToUpper(name[name.IndexOf('_') + 1]) + name.Substring(name.IndexOf('_') + 2);
                }
                float hp = 0;
                float maxHp = 0;
                for (int i = 0; i < modules.Count; i++)
                {
                    if (modules[i].referencedObject.type == subject.referencedObject.type)
                    {
                        hp += modules[i].referencedObject.hp;
                        maxHp += modules[i].referencedObject.maxHp;
                    }
                }
                GameObject module = GameObject.Find("GUI").transform.FindChild("Game").FindChild("Modules").FindChild(name).gameObject;
                if (!module.activeSelf)
                {
                    module.SetActive(true);
                }
                GameObject.Find("GUI").transform.FindChild("Game").FindChild("Modules").FindChild(name).GetComponent<Text>().text = (int)(hp / (maxHp / 100)) + "%";
            }
        }
    }
}
