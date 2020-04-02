using UnityEngine;

public class Inventory : MonoBehaviour
{
    public float size = 1.0F;
    public float payload = 1.0F;


    public bool TryAddItem(ItemObject itemObject)
    {
        return true;
    }
}
