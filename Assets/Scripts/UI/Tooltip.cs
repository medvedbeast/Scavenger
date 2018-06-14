using UnityEngine;
using System.Collections.Generic;

public class Tooltip : MonoBehaviour
{
    public Tooltips type;
    public GameObject owner;
    public event System.Action OnInteractionKeyPressed;

    static List<Tooltip> list = new List<Tooltip>();

    public void Assign(Tooltips type, GameObject owner)
    {
        this.type = type;
        this.owner = owner;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (OnInteractionKeyPressed != null)
            {
                OnInteractionKeyPressed();
            }
        }
    }

    public static Tooltip Create(Tooltips type, Vector3 position, GameObject owner)
    {
        GameObject tooltip = null;

        switch (type)
        {
            case Tooltips.LOOT:
                {
                    tooltip = GameObject.Instantiate(Resources.Load("Prefabs/UI/ExamineTooltip")) as GameObject;
                    tooltip.name = "ExamineTooltip";
                    break;
                }
            case Tooltips.USE:
                {
                    break;
                }
            case Tooltips.ENTER:
                {
                    tooltip = GameObject.Instantiate(Resources.Load("Prefabs/UI/EnterTooltip")) as GameObject;
                    tooltip.name = "EnterTooltip";
                    break;
                }
            case Tooltips.HEALTH:
                {
                    break;
                }
        }

        tooltip.transform.position = new Vector3(position.x, position.y, position.z);
        tooltip.transform.SetParent(GameObject.Find("GUI").transform);
        tooltip.transform.rotation = Quaternion.identity;
        tooltip.transform.localRotation = Quaternion.identity;
        tooltip.AddComponent<Tooltip>();
        tooltip.GetComponent<Tooltip>().Assign(type, owner);
        list.Add(tooltip.GetComponent<Tooltip>());
        return tooltip.GetComponent<Tooltip>();
    }

    public static void Destroy(Tooltips type)
    {
        GameObject gui = GameObject.Find("GUI");
        for (int i = 0; i < gui.transform.childCount; i++)
        {
            Tooltip t = gui.transform.GetChild(i).GetComponent<Tooltip>();
            if (t != null && t.type == type)
            {
                list.Remove(t);
                GameObject.Destroy(t.gameObject);
            }
        }
    }

    public static void Destroy(GameObject owner)
    {
        GameObject gui = GameObject.Find("GUI");
        for (int i = 0; i < gui.transform.childCount; i++)
        {
            Tooltip t = gui.transform.GetChild(i).GetComponent<Tooltip>();
            if (t != null && t.owner == owner)
            {
                list.Remove(t);
                GameObject.Destroy(t.gameObject);
            }
        }
    }


}
