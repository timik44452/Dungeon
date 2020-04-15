using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class WeaponFastslotUIController : MonoBehaviour
{
    public GameObject[] slots;

    private Color alphaColor;
    private Color color;

    private void Awake()
    {
        for (int index = 0; index < slots.Length; index++)
        {
            if (slots[index] == null)
            {
                continue;
            }

            color = slots[index].GetElement<Image>("icon").color;
        }

        alphaColor = color * new Color(1F, 1F, 1F, 0.35F);
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

        for (int index = 0; index < slots.Length; index++)
        {
            var icon = slots[index].GetElement<Image>("icon");

            if (index < weapons.Count)
            {
                if (weapons[index] == battleSystem.currentWeapon)
                {
                    slots[index].transform.localScale = Vector3.one * 1.1F;
                }
                else
                {
                    slots[index].transform.localScale = Vector3.one;
                }

                icon.sprite = weapons[index].icon;
                icon.color = color;
            }
            else
            {
                icon.color = alphaColor;
            }
        }
    }
}
