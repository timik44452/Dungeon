using UnityEngine;
using System.Collections.Generic;

using InventorySystem;

[CreateAssetMenu]
public class InventoryDatabase : ScriptableObject
{
    public List<Item> items;
}
