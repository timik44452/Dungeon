using UnityEngine;
using UnityEngine.UI;

public class UIContextMenu : MonoBehaviour
{ 
    public RectTransform container;

    public Button buttonPrefab;

    public bool IsOpened
    {
        get => isOpened;
        set
        {
            if(isOpened == value)
            {
                return;
            }

            if(value)
            {
                Show();
            }
            else
                {
                Hide();
            }

            isOpened = value;
        }
    }

    private bool isOpened = false;

    public void Show(ContextMenuItem[] menuItems)
    {
        SetMenuItems(menuItems);

        Show();
    }

    public void Show()
    {
        container.gameObject.SetActive(true);

        isOpened = true;
    }

    public void Hide()
    {
        container.gameObject.SetActive(false);

        isOpened = false;
    }

    public void SetMenuItems(params ContextMenuItem[] menuItems)
    {
        Clear();

        foreach (ContextMenuItem menuItem in menuItems)
        {
            CreateMenuItemTree(container, menuItem);
        }
    }

    public void Clear()
    {
        for (int i = 0; i < container.childCount; i++)
            Destroy(container.GetChild(i).gameObject);
    }

    private void CreateMenuItemTree(Transform root, ContextMenuItem item)
    {
        var tempNode = Instantiate(buttonPrefab.gameObject, root);
        var currentRoot = tempNode.GetElement<Transform>("content");

        tempNode.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (item.menuItems.Length > 0)
            {
                currentRoot.gameObject.SetActive(!currentRoot.gameObject.activeSelf);
            }

            item.Invoke();
        });

        tempNode.GetElement<Text>("text").text = item.Text;
        tempNode.GetElement<Image>("icon").sprite = item.Icon;

        foreach (ContextMenuItem menuItem in item.menuItems)
        {
            CreateMenuItemTree(currentRoot, menuItem);
        }
    }
}
