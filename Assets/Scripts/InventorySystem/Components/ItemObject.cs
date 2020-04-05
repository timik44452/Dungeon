using UnityEngine;

public class ItemObject : MonoBehaviour, ITarget
{
    public int ID;

    private void Awake()
    {
        name = ResourceUtility.inventoryDatabase.GetItem(ID).Name;
    }
}
