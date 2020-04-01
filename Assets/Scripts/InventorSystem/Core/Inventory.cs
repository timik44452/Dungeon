using UnityEngine;
using InventorySystem;

public class Inventory : MonoBehaviour
{
    public Transform UITransform;
    
    private bool IsOpened = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            IsOpened = !IsOpened;
        }

        UITransform.position = (Vector2)Camera.main.WorldToScreenPoint(transform.position) + Vector2.right * Screen.width * 0.05F;

        if (UITransform.gameObject.activeSelf != IsOpened)
        {
            UITransform.gameObject.SetActive(IsOpened);
        }
    }
}
