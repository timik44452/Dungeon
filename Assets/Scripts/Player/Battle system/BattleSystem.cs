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
        for (int i = 0; i < 4; i++)
        {
            int keyIndex = i + 1;

            if (i >= m_inventory.weapons.Count)
            {
                break;
            }

            if (Input.GetKeyDown($"{keyIndex}"))
            {
                currentWeapon = m_inventory.weapons[keyIndex];
                Attack();
            }
        }

        if(Input.GetMouseButtonDown(1))
        {
            Attack();
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
