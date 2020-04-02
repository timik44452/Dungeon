using UnityEngine;
using UnityEngine.UI;

using InventorySystem;

[RequireComponent(typeof(Inventory), typeof(PickUpController))]
public class InventoryUIController : MonoBehaviour
{
    public GameObject menuUIElement;
    public GameObject actionUIElement;

    private bool is_opened = false;

    private Inventory inventory;
    private PickUpController pickUpController;

    
    private void Start()
    {
        inventory = GetComponent<Inventory>();
        pickUpController = GetComponent<PickUpController>();
    }

    private void Update()
    {
        menuUIElement.transform.position = (Vector2)Camera.main.WorldToScreenPoint(transform.position) + Vector2.right * Screen.width * 0.05F;

        if (menuUIElement.activeSelf != is_opened)
        {
            menuUIElement.SetActive(is_opened);
        }

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
                is_opened = !is_opened;
            }
        }

        var image = actionUIElement.GetElement<Image>("circle");

        image.fillAmount = pickUpController.PickUpProgress;
    }
}
