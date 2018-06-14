using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public static List<Message> messageList = new List<Message>();
    private static int maxMessageCount = 5;


    public static float speedPointerLimit = 85;

    void Start()
    {

    }

    void Update()
    {
        List<Message> toDelete = new List<Message>();
        for (int i = 0; i < messageList.Count; i++)
        {
            if (messageList[i].lifetime > 0)
            {
                messageList[i].lifetime -= Time.deltaTime;
            }
            else
            {
                messageList[i].Destroy();
                toDelete.Add(messageList[i]);
            }
        }
        for (int i = 0; i < toDelete.Count; i++)
        {
            messageList.Remove(toDelete[i]);
        }
    }

    public static void AddMessage(Message m)
    {
        for (int i = 0; i < messageList.Count; i++)
        {
            if (messageList[i].text == m.text)
            {
                messageList[i].lifetime = m.lifetime;
                return;
            }
        }
        if (messageList.Count + 1 >= maxMessageCount)
        {
            messageList[0].Destroy();
            messageList.RemoveAt(0);
        }
        messageList.Add(m);
        m.Show();
    }

    public static void Hide()
    {
        GameObject ui = GameObject.Find("GUI");
        for (int i = 0; i < ui.transform.childCount; i++)
        {
            if (ui.transform.GetChild(i).name != "Background")
            {
                ui.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public static void Show()
    {
        GameObject ui = GameObject.Find("GUI");
        for (int i = 0; i < ui.transform.childCount; i++)
        {
            if (ui.transform.GetChild(i).name != "Background")
            {
                ui.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public static void Hide(string name)
    {
        GameObject ui = GameObject.Find("GUI");
        ui.transform.FindChild(name).gameObject.SetActive(false);
    }

    public static void Show(string name)
    {
        GameObject ui = GameObject.Find("GUI");
        ui.transform.FindChild(name).gameObject.SetActive(true);
    }

    public static bool IsActive(string name)
    {
        GameObject ui = GameObject.Find("GUI");
        return ui.transform.FindChild(name).gameObject.activeInHierarchy;
    }

    public static void SetItemIcon(GameObject holder, Storable item)
    {
        string name = "";
        if (item is Module)
        {
            name = (item as Module).type.ToString();
            name = name.ToLower();
        }
        if (item is Consumable)
        {
            name = (item as Consumable).type.ToString();
            name = name.ToLower();
        }
        Texture2D texture = Resources.Load("Prefabs/Icons/" + name) as Texture2D;
        holder.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), new Vector2(0.5f, 0.5f));
        RectTransform itemRect = holder.GetComponent<RectTransform>();
        itemRect.pivot = new Vector2(0.5f, 0.5f);
        itemRect.anchorMin = new Vector2(0, 0);
        itemRect.anchorMax = new Vector2(1, 1);
        itemRect.sizeDelta = new Vector2(0, 0);
    }

    public static void SetLootItemIcon(GameObject holder, Storable item)
    {
        string name = "";
        object type = item.GetType();
        if (item is Module)
        {
            name = (item as Module).type.ToString();
            name = name.ToLower();
        }
        if (item is Consumable)
        {
            name = (item as Consumable).type.ToString();
            name = name.ToLower();
        }
        Texture2D texture = Resources.Load("Prefabs/Icons/" + name) as Texture2D;
        holder.transform.FindChild("Image").FindChild("Image").GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(Vector2.zero, new Vector2(texture.width, texture.height)), new Vector2(0.5f, 0.5f));
        RectTransform itemRect = holder.GetComponent<RectTransform>();
        itemRect.pivot = new Vector2(0.5f, 0.5f);
        itemRect.anchorMin = new Vector2(0, 0);
        itemRect.anchorMax = new Vector2(1, 1);
        itemRect.sizeDelta = new Vector2(0, 0);
    }

}
