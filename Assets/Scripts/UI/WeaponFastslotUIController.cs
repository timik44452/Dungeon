using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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
        BattleSystem battleSystem = SceneUtility.Player.GetComponent<BattleSystem>();

        List<Weapon> weapons = inventory.weapons;

        for (int index = 0; index < Mathf.Min(4, weapons.Count); index++)
        {
            if(weapons[index] == battleSystem.currentWeapon)
            {
                slots[index].transform.localScale = Vector3.one * 1.1F;
            }
            else
            {
                slots[index].transform.localScale = Vector3.one;
            }

            slots[index].GetElement<Image>("icon").sprite = weapons[index].icon;
            slots[index].SetActive(true);
        }
    }
}
