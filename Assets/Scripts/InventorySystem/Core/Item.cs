using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class Item
    {
        public int ID;
        public string name;

        public int Count;


        public Item(int ID, string Name)
        {
            this.ID = ID;
            name = Name;
        }
    }
}
