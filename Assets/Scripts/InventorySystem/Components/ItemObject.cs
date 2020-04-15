using UnityEngine;

public class ItemObject : MonoBehaviour, ITarget
{
    public int ID;

    public int TypeIdentifier { get => ID; }

    private void Awake()
    {
        name = ResourceUtility.inventoryDatabase.GetItem(ID).Name;
    }
}
