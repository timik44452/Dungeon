using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName ="Weapons/Weapon database", order = 0)]
public class WeaponDatabase : ScriptableObject
{
    public List<Weapon> weapons;
}
