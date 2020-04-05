using UnityEngine;
using System.Collections.Generic;

using InventorySystem;

public class Inventory : MonoBehaviour
{
    public event UnityEventHandler OnInventoryChanged;

    public float size = 1.0F;
    public float payload = 1.0F;

    public List<Item> items { get; private set; }

    //TODO: optimize it !!!
    public List<Weapon> weapons
    {
        get
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach (var item in items)
            {
                var weapon = ResourceUtility.weaponDatabase.weapons.Find(x => x.Name == item.Name);

                if (weapon != null)
                {
                    weapons.Add(weapon);
                }
            }

            return weapons;
        }
    }

    private void Start()
    {
        items = new List<Item>();
    }

    public bool TryAddItem(ItemObject itemObject)
    {
        Item item = items.Find(x => x.ID == itemObject.ID);

        if (item == null)
        {
            item = ResourceUtility.inventoryDatabase.GetItem(itemObject.ID);

            items.Add(item);
        }

        item.Count++;

        OnInventoryChanged?.Invoke(this);

        return true;
    }
}
