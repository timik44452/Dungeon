using UnityEngine;
using System.Collections.Generic;

public class WeaponFastslotUIController : MonoBehaviour
{
    public GameObject[] slots;

    private void Awake()
    {
        foreach (var slot in slots)
        {
            slot.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (SceneUtility.Player == null)
        {
            return;
        }

        Inventory inventory = SceneUtility.Player.GetComponent<Inventory>();

        List<Weapon> weapons = inventory.weapons;

        for (int index = 0; index < Mathf.Min(4, weapons.Count); index++)
        {
            slots[index].SetActive(true);
        }
    }
}
