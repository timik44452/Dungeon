using UnityEngine;

public class ItemObject : MonoBehaviour, ITarget
{
    public int ID;

    private Vector3 box_size = Vector3.one * 5;

    private GameObject icon;

    private void Start()
    {
        GameObject icon = ResourceUtility.resourceDatabase.actionIconPrefab;

        this.icon = Instantiate(icon, transform);
    }

    private void FixedUpdate()
    {
        Vector3 playerPosition = SceneUtility.Player.transform.position;

        bool is_active = IsBox(playerPosition, box_size);

        if (icon.activeSelf != is_active)
        {
            icon.SetActive(is_active);
        }
    }

    private bool IsBox(Vector3 point, Vector3 box)
    {
        Vector3 delta = transform.position - point;

        return
            Mathf.Abs(delta.x) <= box.x &&
            Mathf.Abs(delta.y) <= box.y &&
            Mathf.Abs(delta.z) <= box.z;
    }
}
