using UnityEngine;
using System.Collections;

public class Interactive : MonoBehaviour
{
    public Tooltips tooltipType;

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer != 10)
        {
            return;
        }
        Vector3 position = new Vector3(transform.position.x, GetComponent<Collider>().bounds.max.y + 5, transform.position.z);
        Tooltip.Create(tooltipType, position, this.gameObject).OnInteractionKeyPressed += OnInteractionKeyPressed;
    }

    public void OnTriggerExit(Collider c)
    {
        if ((tooltipType == Tooltips.LOOT || tooltipType == Tooltips.ENTER) && c.gameObject.layer == 10)
        {
            Tooltip.Destroy(tooltipType);
        }
    }

    public void OnInteractionKeyPressed()
    {
        switch (tooltipType)
        {
            case Tooltips.LOOT:
                {
                    GameObject.Find("Core").GetComponent<Events>().OnLootStart(this.gameObject);
                    break;
                }
        }
    }
}
