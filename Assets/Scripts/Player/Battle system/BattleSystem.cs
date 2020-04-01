using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    private TargetSystem targetSystem;

    private Weapon currentWeapon;
    private Weapon[] weapons;

    private float time = 0.0F;

    private void Start()
    {
        targetSystem = FindObjectOfType<TargetSystem>();
        weapons = transform.GetComponentsInChildren<Weapon>();

        if (weapons.Length > 0)
        {
            currentWeapon = weapons[0];
        }
    }

    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            int keyIndex = i + 1;

            if (i >= weapons.Length)
            {
                break;
            }

            if (Input.GetKeyDown($"{keyIndex}"))
            {
                SwapWeapon(weapons[i]);
                Attack(currentWeapon);
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            Attack(currentWeapon);
        }
    }

    private void SwapWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
    }

    private void Attack(Weapon weapon)
    {
        if (weapon == null || targetSystem == null || targetSystem.Target == null)
        {
            return;
        }

        weapon.Invoke(targetSystem.Target, null);
    }

    private void OnGUI()
    {
        if (Input.GetMouseButton(2))
        {
            float an = 360;
            float horizontal = Input.GetAxis("Mouse X");
            float vertical = Input.GetAxis("Mouse Y");

            for (int i = 0; i < weapons.Length; i++)
            {
                float alpha = i / (float)weapons.Length;
                float skillBoxSize = Mathf.Min(Screen.width, Screen.height) * 0.085F;

                if(weapons[i] == currentWeapon)
                {
                    skillBoxSize *= 1.35F;
                }

                float x = 2F * skillBoxSize * Mathf.Sin((alpha + time) * Mathf.PI * 2);
                float y = 2F * skillBoxSize * Mathf.Cos((alpha + time) * Mathf.PI * 2);

                Vector2 direction = new Vector2(x, y);
                Vector2 inputDirection = new Vector2(horizontal, vertical);

                if (inputDirection.magnitude > 0.1F)
                {
                    float angle = Vector2.Angle(inputDirection, direction);

                    if (angle < an)
                    {
                        currentWeapon = weapons[i];

                        an = angle;
                    }
                }

                Rect rect = new Rect(Screen.width / 2 + x, Screen.height / 2 - y, skillBoxSize, skillBoxSize);

                GUI.Box(rect, weapons[i].defaultSkill.icon);
            }
        }
    }
}
