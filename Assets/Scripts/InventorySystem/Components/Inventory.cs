using UnityEngine;
using System.Collections.Generic;

using InventorySystem;

public class Inventory : MonoBehaviour
{
    public float size = 1.0F;
    public float payload = 1.0F;

    public List<Item> items { get; private set; }

    private void Start()
    {
        items = new List<Item>();
    }

    public bool TryAddItem(ItemObject itemObject)
    {
        Item item = items.Find(x => x.ID == itemObject.ID);

        if (item == null)
        {
            var tempItem = ResourceUtility.inventoryDatabase.items.Find(x => x.ID == itemObject.ID);

            if (tempItem == null)
            {
                item = new Item(itemObject.ID, "Secret item");
            }
            else
            {
                item = new Item(itemObject.ID, tempItem.Name);
            }

            items.Add(item);
        }

        item.Count++;

        return true;
    }
}
