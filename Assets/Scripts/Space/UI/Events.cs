using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Common;

namespace Space
{
    public class Events : MonoBehaviour
    {
        GameObject gui;

        public void Start()
        {
            gui = GameObject.Find("GUI");
        }

        public void OnStartButtonClick()
        {
            UI.Hide();
            UI.Show("Loading");
            Game.gameState = GameStates.LOADING;
            Game.sector = new Sector(1000, 1000);
            Game.sector.GenerateScene();
            UI.Hide();
            UI.Show("Game");
        }

        public void OnExitButtonClick()
        {
            Application.Quit();
        }

        public void OnShowInventoryClick()
        {
            if (UI.IsActive("Inventory"))
            {
                OnCloseInventoryClick();
                return;
            }

            UI.Show("Inventory");
            GameObject scroller = gui.transform.FindChild("Inventory").FindChild("Container").transform.FindChild("Content").FindChild("Scroller").gameObject;
            GameObject ship = ShipController.controlledObject;
            Unit u = ship.GetComponent<Unit>();
            for (int i = 0; i < u.inventory.maxSlots; i++)
            {
                CreateInventorySlot(i, scroller.transform);
            }
            for (int i = 0; i < u.inventory.itemList.Count; i++)
            {
                CreateInventoryItem(i, scroller.transform, u);
            }
            SetInventoryInfo();
        }

        public void OnItemPointerEnter(Storable subject)
        {
            UI.Show("ItemDescription");
            Vector3 p = new Vector3(Input.mousePosition.x - (Screen.width / 2), Input.mousePosition.y - (Screen.height / 2), 0);
            GameObject container = gui.transform.FindChild("ItemDescription").FindChild("Container").gameObject;

            Storable item = subject;
            container.transform.FindChild("Name").GetComponent<Text>().text = item.name;
            container.transform.FindChild("Description").GetComponent<Text>().text = item.description;
            container.transform.FindChild("Weight").GetComponent<Text>().text = item.weight + " kg";
            container.transform.FindChild("Price").GetComponent<Text>().text = item.price + " $";

            string prefab = "Prefabs/UI/ItemDescriptionRow";
            List<System.Reflection.FieldInfo> fields = new List<System.Reflection.FieldInfo>();
            fields.AddRange(item.GetType().GetFields());
            for (int i = 0; i < fields.Count; i++)
            {

                if (fields[i].GetCustomAttributes(typeof(Serializable), false).Length == 0 && !Functions.Contains<string>(Storable.baseFields, fields[i].Name))
                {
                    GameObject g = GameObject.Instantiate(Resources.Load(prefab), Vector3.zero, Quaternion.identity) as GameObject;
                    g.transform.SetParent(container.transform);
                    g.transform.localPosition = Vector3.zero;
                    g.transform.localScale = Vector3.one;
                    g.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    g.name = char.ToUpper(fields[i].Name[0]) + fields[i].Name.Substring(1);
                    string label = ConvertToLabel(fields[i].Name).ToUpper();
                    string text = fields[i].GetValue(item).ToString();
                    g.GetComponent<Text>().text = label + ":\n<color=yellow>" + text + "</color>";
                }
            }
            container.GetComponent<RectTransform>().localPosition = p;
        }

        public void OnItemPointerExit()
        {
            GameObject container = gui.transform.FindChild("ItemDescription").FindChild("Container").gameObject;
            for (int i = 0; i < container.transform.childCount; i++)
            {
                if (!Functions.Contains<string>(Storable.baseFields, container.transform.GetChild(i).name))
                {
                    GameObject.Destroy(container.transform.GetChild(i).gameObject);
                }
            }
            UI.Hide("ItemDescription");
        }

        public void OnItemClick(BaseEventData eventData, GameObject owner, string element, Storable subject, Actions action)
        {
            if ((eventData as PointerEventData).dragging)
            {
                return;
            }
            OnItemPointerExit();
            Inventory i = ShipController.controlledObject.GetComponent<Unit>().inventory;
            switch (action)
            {
                case Actions.LOOT:
                    {
                        int targetSlot = i.Add(Functions.Clone(subject));
                        if (targetSlot >= 0)
                        {
                            GameObject scroller = gui.transform.FindChild("Loot").FindChild("Container").FindChild("Content").FindChild("Scroller").gameObject;
                            GameObject.Destroy(scroller.transform.FindChild(element).gameObject);
                            owner.GetComponent<Unit>().inventory.Remove(subject.slot);
                            if (owner.GetComponent<Unit>().inventory.itemList.Count == 0)
                            {
                                UI.Hide("Loot");
                            }
                            if (UI.IsActive("Inventory"))
                            {
                                RenewInventoryItem(targetSlot);
                            }
                        }
                        break;
                    }
            }

            SetInventoryInfo();
        }

        public void OnItemClick(BaseEventData eventData, Storable subject, Actions action)
        {
            if ((eventData as PointerEventData).dragging)
            {
                return;
            }
            OnItemPointerExit();
            Inventory i = ShipController.controlledObject.GetComponent<Unit>().inventory;
            switch (action)
            {
                case Actions.USE:
                    {
                        int slot = subject.slot;
                        bool consumed = (subject as Consumable).Consume(ShipController.controlledObject);
                        if (consumed)
                        {
                            RenewInventoryItem(slot);
                        }
                        break;
                    }
                case Actions.EQUIP:
                    {
                        Debug.Log(2);
                        break;
                    }
            }

            SetInventoryInfo();
        }

        public void OnItemDropped(GameObject subject, object position)
        {
            Unit unit = ShipController.controlledObject.GetComponent<Unit>();
            int slot = GetSlotIndex(subject.GetComponent<Dragable>().startParent.name);
            if (position == null)
            {
                unit.inventory.Remove(slot);
                //Debug.Log("Destroyed " + subject.name);
                GameObject.Destroy(subject);
            }
            else
            {
                GameObject scroller = gui.transform.FindChild("Inventory").FindChild("Container").transform.FindChild("Content").FindChild("Scroller").gameObject;
                for (int i = 0; i < scroller.transform.childCount; i++)
                {
                    Transform child = scroller.transform.GetChild(i);
                    if (RectTransformUtility.RectangleContainsScreenPoint(child.GetComponent<RectTransform>(), Input.mousePosition, Camera.main))
                    {
                        int targetSlot = GetSlotIndex(child.name);
                        if (child.childCount > 0)
                        {
                            GameObject swaped = child.GetChild(0).gameObject;
                            swaped.transform.SetParent(subject.GetComponent<Dragable>().startParent.transform);
                            swaped.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                            unit.inventory.Get(targetSlot).slot = slot;
                        }
                        subject.transform.SetParent(child.transform);
                        subject.GetComponent<Dragable>().rectTransform.anchoredPosition = new Vector2(0, 0);
                        unit.inventory.Get(slot).slot = targetSlot;
                        return;
                    }
                }
                subject.transform.SetParent(subject.GetComponent<Dragable>().startParent.transform);
                subject.GetComponent<Dragable>().rectTransform.anchoredPosition = new Vector2(0, 0);
            }
            UI.Hide("ItemDescription");
            OnItemPointerExit();

            SetInventoryInfo();
        }

        public void OnCloseInventoryClick()
        {
            GameObject scroller = gui.transform.FindChild("Inventory").FindChild("Container").transform.FindChild("Content").FindChild("Scroller").gameObject;
            for (int i = 0; i < scroller.transform.childCount; i++)
            {
                GameObject.Destroy(scroller.transform.GetChild(i).gameObject);
            }

            UI.Hide("Inventory");
        }

        public void OnLootStart(GameObject subject)
        {
            if (UI.IsActive("Loot"))
            {
                OnLootEnd();
            }
            UI.Show("Loot");

            GameObject scroller = gui.transform.FindChild("Loot").FindChild("Container").FindChild("Content").FindChild("Scroller").gameObject;

            for (int i = 0; i < subject.GetComponent<Unit>().inventory.itemList.Count; i++)
            {
                CreateLootItem(subject, i, scroller.transform);
            }
        }

        public void OnLootEnd()
        {
            GameObject scroller = gui.transform.FindChild("Loot").FindChild("Container").FindChild("Content").FindChild("Scroller").gameObject;
            for (int i = 0; i < scroller.transform.childCount; i++)
            {
                GameObject.Destroy(scroller.transform.GetChild(i).gameObject);
            }
            UI.Hide("Loot");
        }

        public void OnHealthChanged()
        {
            GameObject health = gui.transform.FindChild("Game").FindChild("Center").FindChild("Health").gameObject;
            Unit u = ShipController.controlledObject.GetComponent<Unit>();
            float result = (u.hp / (u.maxHp * 0.01f)) / 100;
            health.GetComponent<Image>().fillAmount = result;
            health.GetComponentInChildren<Text>().text = (int)(result * 100) + "%";
        }

        public void OnFuelChanged()
        {
            GameObject fuel = gui.transform.FindChild("Game").FindChild("Center").FindChild("Fuel").gameObject;
            Unit u = ShipController.controlledObject.GetComponent<Unit>();
            float result = (u.fuel / (u.maxFuel * 0.01f)) / 100;
            fuel.GetComponent<Image>().fillAmount = result;
            fuel.GetComponentInChildren<Text>().text = (int)(result * 100) + "%";
        }

        //

        private int GetSlotIndex(string name)
        {
            return int.Parse(name.Split(new char[] { '_' })[1]);
        }

        private string ConvertToLabel(string source)
        {
            string target = "";
            for (int i = 0; i < source.Length; i++)
            {
                if (char.IsUpper(source[i]))
                {
                    target += " ";
                }
                target += source[i];
            }
            return target;
        }

        private void CreateInventorySlot(int index, Transform parent)
        {
            string name = "Prefabs/UI/InventorySlot";
            GameObject image = GameObject.Instantiate(Resources.Load(name), Vector3.zero, Quaternion.identity) as GameObject;
            image.transform.SetParent(parent);
            image.transform.localPosition = Vector3.zero;
            image.transform.localScale = Vector3.one;
            image.transform.localRotation = Quaternion.Euler(0, 0, 0);
            image.transform.name = "InventorySlot_" + index;
        }

        private void CreateInventoryItem(int itemIndex, Transform parent, Unit u)
        {
            Storable targetItem = u.inventory.itemList[itemIndex];
            if (targetItem == null)
            {
                return;
            }
            GameObject item = GameObject.Instantiate(Resources.Load("Prefabs/UI/InventoryItem"), Vector3.zero, Quaternion.identity) as GameObject;
            item.transform.SetParent(parent.transform.GetChild(targetItem.slot));
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.Euler(0, 0, 0);
            item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            item.GetComponent<RectTransform>().sizeDelta = new Vector2(parent.GetComponent<GridLayoutGroup>().cellSize.x, parent.GetComponent<GridLayoutGroup>().cellSize.y);
            item.transform.name = "Item_" + itemIndex;

            UI.SetItemIcon(item, targetItem);

            item.transform.FindChild("Stack").GetComponent<Text>().text = targetItem.maxStack > 1 ? "x" + targetItem.stack : "";
            if (targetItem is Consumable && (targetItem as Consumable).maxCharges > 1)
            {
                item.transform.FindChild("Quantity").GetComponent<Text>().text = (targetItem as Consumable).charges + "(" + (targetItem as Consumable).maxCharges + ")";
            }
            else
            {
                item.transform.FindChild("Quantity").GetComponent<Text>().text = "";
            }

            Dragable itemDragable = item.GetComponent<Dragable>();
            itemDragable.boundingObject = gui.transform.FindChild("Inventory").FindChild("Container").GetComponent<RectTransform>();
            itemDragable.temporalParent = gui.transform.FindChild("Inventory").gameObject;
            itemDragable.OnDragEnded += OnItemDropped;

            EventTrigger.Entry enter = new EventTrigger.Entry();
            enter.eventID = EventTriggerType.PointerEnter;
            enter.callback.AddListener((eventData) => { OnItemPointerEnter(targetItem); });
            item.GetComponent<EventTrigger>().triggers.Add(enter);

            EventTrigger.Entry exit = new EventTrigger.Entry();
            exit.eventID = EventTriggerType.PointerExit;
            exit.callback.AddListener((eventData) => { OnItemPointerExit(); });
            item.GetComponent<EventTrigger>().triggers.Add(exit);

            EventTrigger.Entry click = new EventTrigger.Entry();
            click.eventID = EventTriggerType.PointerClick;
            Actions a = Actions.NONE;
            if (targetItem is Module)
            {
                a = Actions.EQUIP;
            }
            else if (targetItem is Consumable)
            {
                a = Actions.USE;
            }
            click.callback.AddListener((eventData) => { OnItemClick(eventData, targetItem, a); });
            item.GetComponent<EventTrigger>().triggers.Add(click);
        }

        private void CreateLootItem(GameObject subject, int index, Transform parent)
        {
            Storable targetItem = subject.GetComponent<Unit>().inventory.itemList[index];

            GameObject item = GameObject.Instantiate(Resources.Load("Prefabs/UI/LootItem"), Vector3.zero, Quaternion.identity) as GameObject;
            item.transform.SetParent(parent);
            item.transform.localRotation = Quaternion.identity;
            item.transform.localScale = Vector3.one;
            item.transform.name = "LootItem_" + index;
            item.transform.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            item.GetComponentInChildren<Text>().text = targetItem.name;

            UI.SetLootItemIcon(item, targetItem);

            EventTrigger.Entry enter = new EventTrigger.Entry();
            enter.eventID = EventTriggerType.PointerEnter;
            enter.callback.AddListener((eventData) => { OnItemPointerEnter(targetItem); });
            item.GetComponent<EventTrigger>().triggers.Add(enter);

            EventTrigger.Entry exit = new EventTrigger.Entry();
            exit.eventID = EventTriggerType.PointerExit;
            exit.callback.AddListener((eventData) => { OnItemPointerExit(); });
            item.GetComponent<EventTrigger>().triggers.Add(exit);

            EventTrigger.Entry click = new EventTrigger.Entry();
            click.eventID = EventTriggerType.PointerClick;
            click.callback.AddListener((eventData) => { OnItemClick(eventData, subject, item.name, targetItem, Actions.LOOT); });
            item.GetComponent<EventTrigger>().triggers.Add(click);
        }

        private void SetInventoryInfo()
        {
            Unit u = ShipController.controlledObject.GetComponent<Unit>();
            GameObject footer = gui.transform.FindChild("Inventory").FindChild("Container").FindChild("Footer").gameObject;
            footer.transform.FindChild("Slots").GetComponent<Text>().text = "Slots: " + u.inventory.slots + "/" + u.inventory.maxSlots;
            footer.transform.FindChild("Capacity").GetComponent<Text>().text = "Capacity: " + u.capacity + "/" + u.maxCapacity;
        }

        private void RenewInventoryItem(int slot)
        {
            GameObject scroller = gui.transform.FindChild("Inventory").FindChild("Container").FindChild("Content").FindChild("Scroller").gameObject;
            GameObject inventorySlot = scroller.transform.FindChild("InventorySlot_" + slot).gameObject;
            for (int i = 0; i < inventorySlot.transform.childCount; i++)
            {
                GameObject.Destroy(inventorySlot.transform.GetChild(i).gameObject);
            }
            Unit u = ShipController.controlledObject.GetComponent<Unit>();
            if (u.inventory.GetIndex(slot) != -1)
            {
                CreateInventoryItem(u.inventory.GetIndex(slot), scroller.transform, u);
            }
        }
    }
}