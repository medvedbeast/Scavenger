using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Message
{
    public string text;
    public float lifetime;
    public Color color;

    private GameObject link;

    public Message(string text, float lifetime, Color color)
    {
        this.text = text;
        this.lifetime = lifetime;
        this.color = color;
    }

    public Message(string text)
    {
        this.text = text;
        this.lifetime = 3.0f;
        this.color = Color.white;
    }

    public void Show()
    {
        GameObject container = GameObject.Find("GUI").transform.FindChild("Game").FindChild("Information").FindChild("Container").gameObject;
        GameObject textObject = GameObject.Instantiate(Resources.Load("Prefabs/UI/InformationRow") as GameObject);
        link = textObject;
        textObject.transform.SetParent(container.transform);
        textObject.name = "Message";
        textObject.transform.position = Vector3.zero;
        textObject.transform.localScale = new Vector3(1, 1, 1);
        textObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        textObject.GetComponent<Text>().color = color;
        textObject.GetComponent<Text>().text = this.text;
    }

    public void Destroy()
    {
        GameObject.Destroy(link);
    }
}
