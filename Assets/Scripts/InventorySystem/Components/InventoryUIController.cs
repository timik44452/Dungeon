using UnityEngine;
using UnityEngine.UI;

using InventorySystem;
using System.Collections.Generic;

[RequireComponent(typeof(Inventory), typeof(PickUpController))]
public class InventoryUIController : MonoBehaviour
{
    public UIContextMenu mainContexMenu;
    public GameObject actionUIElement;

    private Inventory inventory;
    private PickUpController pickUpController;

    #region Context menu items
    private ContextMenuItem inventoryItem = new ContextMenuItem("Inventory");
    private ContextMenuItem skillsItem = new ContextMenuItem("Skills");
    private ContextMenuItem weaponsItem = new ContextMenuItem("Weapons");

    private ContextMenuItem[] items;
    #endregion

    private void Start()
    {
        inventory = GetComponent<Inventory>();
        pickUpController = GetComponent<PickUpController>();

        inventory.OnInventoryChanged += OnInventoryChanged;

        items = new ContextMenuItem[]
        {
            inventoryItem,
            skillsItem,
            weaponsItem
        };
    }

    private void OnInventoryChanged(Component sender)
    {
        List<ContextMenuItem> menuItems = new List<ContextMenuItem>();

        foreach (var item in inventory.items)
        {
            ContextMenuItem menuItem = new ContextMenuItem(item.name);

            menuItems.Add(menuItem);
        }

        inventoryItem.menuItems = menuItems.ToArray();

        mainContexMenu.SetMenuItems(items);
    }

    private void Update()
    {
        mainContexMenu.transform.position = (Vector2)Camera.main.WorldToScreenPoint(transform.position) + Vector2.right * Screen.width * 0.05F;

        if (actionUIElement.activeSelf != pickUpController.currentItem)
        {
            actionUIElement.SetActive(pickUpController.currentItem);
        }

        if (pickUpController.currentItem)
        {
            actionUIElement.transform.position = Camera.main.WorldToScreenPoint(pickUpController.currentItem.transform.position);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (mainContexMenu.IsOpened)
                {
                    mainContexMenu.Hide();
                }
                else
                {
                    mainContexMenu.Show(items);
                }
            }
        }

        var image = actionUIElement.GetElement<Image>("circle");

        image.fillAmount = pickUpController.PickUpProgress;
    }
}
