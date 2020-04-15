using UnityEngine;
using UnityEngine.UI;

using InventorySystem;
using System.Collections.Generic;

[RequireComponent(typeof(Inventory), typeof(PickUpController))]
public class InventoryUIController : MonoBehaviour
{
    public UIContextMenu mainContexMenu;

    #region UIElements
    public GameObject actionUIElement;
    public GameObject healthBar;
    public GameObject powerBar;

    private Image m_healthImage;
    private Image m_powerBarImage;
    private Image m_actionUIElementImage;
    #endregion

    #region Context menu items
    private ContextMenuItem inventoryItem;
    private ContextMenuItem skillsItem;
    private ContextMenuItem weaponsItem;

    private ContextMenuItem[] items;
    #endregion

    #region Components
    private Health health;
    private Inventory inventory;
    private PickUpController pickUpController;
    #endregion

    private void Start()
    {
        health = GetComponent<Health>();
        inventory = GetComponent<Inventory>();
        pickUpController = GetComponent<PickUpController>();

        inventory.OnInventoryChanged += OnInventoryChanged;

        inventoryItem = new ContextMenuItem("Inventory", ResourceUtility.resourceDatabase.inventoryIconSprite);
        skillsItem = new ContextMenuItem("Skills", ResourceUtility.resourceDatabase.skillIconSprite);
        weaponsItem = new ContextMenuItem("Weapons", ResourceUtility.resourceDatabase.weaponIconSprite);

        m_actionUIElementImage = actionUIElement.GetElement<Image>("circle");
        m_healthImage = healthBar.GetElement<Image>("value");
        m_powerBarImage = powerBar.GetElement<Image>("value");

        items = new ContextMenuItem[]
        {
            inventoryItem,
            weaponsItem,
            skillsItem
        };
    }

    private void OnInventoryChanged(Component sender)
    {
        List<ContextMenuItem> inventoryMenuItems = new List<ContextMenuItem>();
        List<ContextMenuItem> weaponMenuItems = new List<ContextMenuItem>();

        foreach (var item in inventory.items)
        {
            ContextMenuItem menuItem = new ContextMenuItem(item.Name);

            inventoryMenuItems.Add(menuItem);
        }

        foreach(var item in inventory.weapons)
        {
            ContextMenuItem menuItem = new ContextMenuItem(item.Name);

            weaponMenuItems.Add(menuItem);
        }

        inventoryItem.menuItems = inventoryMenuItems.ToArray();
        weaponsItem.menuItems = weaponMenuItems.ToArray();

        mainContexMenu.SetMenuItems(items);
    }

    private void LateUpdate()
    {
        mainContexMenu.transform.position = (Vector2)Camera.main.WorldToScreenPoint(transform.position) + Vector2.right * Screen.width * 0.05F;
    }

    private void Update()
    {
        if (actionUIElement.activeSelf != pickUpController.currentItem)
        {
            actionUIElement.SetActive(pickUpController.currentItem);

            if (pickUpController.currentItem)
            {
                actionUIElement.GetElement<Text>("text").text = pickUpController.currentItem.name;
            }
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

        m_actionUIElementImage.fillAmount = pickUpController.PickUpProgress;
    }
}
