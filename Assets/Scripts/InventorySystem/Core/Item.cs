namespace InventorySystem
{
    [System.Serializable]
    public class Item
    {
        public string Name;
        public int ID;
        public int Count { get; set; }


        public Item(int ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
        }
    }
}
