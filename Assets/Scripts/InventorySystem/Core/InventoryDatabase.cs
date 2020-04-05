using UnityEngine;
using System.Collections.Generic;

using InventorySystem;

[CreateAssetMenu]
public class InventoryDatabase : ScriptableObject
{
    public List<Item> items;

    public Item GetItem(int ID)
    {
        var item = items.Find(x => x.ID == ID);

        if (item == null)
        {
            item = new Item(ID, "Secret item");
        }
        else
        {
            item = new Item(item.ID, item.Name);
        }

        return item;
    }
}
