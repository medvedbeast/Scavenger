using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Common
{
    public class Inventory
    {

        public int slots = 0;
        public int maxSlots = 0;
        public List<Storable> itemList = new List<Storable>();

        public Inventory(int slots)
        {
            this.slots = 0;
            this.maxSlots = slots;
        }

        public int Add(Storable item)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].name == item.name && itemList[i].maxStack - itemList[i].stack >= item.stack)
                {
                    itemList[i].stack += item.stack;
                    return itemList[i].slot;
                }
            }
            if ((maxSlots - slots) > 0)
            {
                int slot = FindEmptySlot();
                if (slot != -1)
                {
                    item.slot = slot;
                    slots++;
                    itemList.Add(item);
                    return slot;
                }
            }
            return -1;
        }

        public Storable Get(int slot)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].slot == slot)
                {
                    return itemList[i];
                }
            }
            return null;
        }

        public int GetIndex(int slot)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].slot == slot)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool Remove(int slot)
        {
            int index = -1;
            for (int i = 0; i < itemList.Count; i++)
            {
                if (itemList[i].slot == slot)
                {
                    index = i;
                }
            }
            if (index == -1)
            {
                return false;
            }
            else
            {
                itemList.RemoveAt(index);
                slots--;
                return true;
            }
        }

        private int FindEmptySlot()
        {
            List<int> slots = new List<int>();
            for (int i = 0; i < itemList.Count; i++)
            {
                slots.Add(itemList[i].slot);
            }
            for (int i = 0; i < maxSlots; i++)
            {
                if (!slots.Contains(i))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
