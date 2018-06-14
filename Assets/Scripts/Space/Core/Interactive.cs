using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace Space
{
    public class Interactive : MonoBehaviour
    {
        static GameObject target = null;
        static GameObject interaction;
        static Text interactionText;

        public Tooltips type;

        public void Start()
        {
            interaction = GameObject.Find("GUI").transform.FindChild("Game").FindChild("Interaction").gameObject;
            interactionText = interaction.transform.FindChild("Container").FindChild("Text").GetComponent<Text>();
        }

        public void OnTriggerEnter(Collider c)
        {
            if (target == null && c.gameObject.layer == 10)
            {
                StartInteraction(c, type, gameObject);
            }
        }

        public void OnTriggerStay(Collider c)
        {
            if (target == null && c.gameObject.layer == 10)
            {
                StartInteraction(c, type, gameObject);
            }
        }

        public void OnTriggerExit(Collider c)
        {
            if (target == c.gameObject)
            {
                EndInteraction();
            }
        }

        public static void StartInteraction(Collider c, Tooltips type, GameObject source)
        {
            ShipController.interacting = true;
            interaction.SetActive(true);
            ShipController.ResetInteractionEvent();
            switch (type)
            {
                case Tooltips.LOOT:
                    {
                        interactionText.text = "Press [F] to loot!";
                        ShipController.OnInteractionKeyPressed += () => { GameObject.Find("Core").GetComponent<Events>().OnLootStart(source.gameObject); };
                        break;
                    }
            }
            target = c.gameObject;
        }

        public static void EndInteraction()
        {
            ShipController.interacting = false;
            interaction.SetActive(false);
            ShipController.ResetInteractionEvent();
            GameObject.Find("Core").GetComponent<Events>().OnLootEnd();
            target = null;
        }
    }
}