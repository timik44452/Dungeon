using UnityEngine;

namespace InventorySystem
{
    [System.Serializable]
    public class Item
    {
        public int ID { get => id; }
        public string Name { get => name; }

        public int Count;

        [SerializeField]
        private int id;
        [SerializeField]
        private string name;

        public Item(int ID, string Name)
        {
            id = ID;
            name = Name;
        }
    }
}
