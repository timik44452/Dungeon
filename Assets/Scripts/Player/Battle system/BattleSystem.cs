using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class BattleSystem : MonoBehaviour
{
    public event UnityEventHandler OnWeaponChanged;

    public Weapon currentWeapon
    {
        get => m_currentWeapon;
        set
        {
            m_currentWeapon = value;
            OnWeaponChanged?.Invoke(this);
        }
    }

    private Inventory m_inventory;
    private TargetSystem m_targetSystem;
    private Weapon m_currentWeapon;


    private void Start()
    {
        m_inventory = GetComponent<Inventory>();
        m_targetSystem = FindObjectOfType<TargetSystem>();
    }

    private void Update()
    {
        var weapons = m_inventory.weapons;

        for (int i = 0; i < 4; i++)
        {
            int keyIndex = i + 1;

            if (i >= weapons.Count)
            {
                break;
            }

            if (Input.GetKeyDown($"{keyIndex}"))
            {
                currentWeapon = weapons[i];
                Attack();
            }
        }

        if (Input.GetMouseButton(2))
        {
            float x_direction = Input.GetAxis("Mouse X");
            float y_direction = Input.GetAxis("Mouse Y");

            if (Mathf.Abs(x_direction) < 0.1F)
                x_direction = 0;

            if (Mathf.Abs(y_direction) < 0.1F)
                y_direction = 0;

            if (Mathf.Abs(x_direction) > Mathf.Abs(y_direction))
            {
                if (x_direction > 0)
                    SelectWeapon(1);
                else
                    SelectWeapon(0);
            }
            else
            {
                if (y_direction > 0)
                    SelectWeapon(3);
                else
                    SelectWeapon(2);
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            Attack();
        }
    }

    private void SelectWeapon(int index)
    {
        if (index < m_inventory.weapons.Count)
        {
            currentWeapon = m_inventory.weapons[index];
        }
    }

    public void Attack()
    {
        if (currentWeapon == null || m_targetSystem == null)
        {
            return;
        }

        currentWeapon.Invoke(this, m_targetSystem.Target, null);
    }
}
