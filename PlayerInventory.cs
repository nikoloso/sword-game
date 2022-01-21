using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;

    public WeaponItem handWeapon;
    public WeaponItem offhandWeapon;

    public WeaponItem unarmedWeapon;

    public WeaponItem[] weaponsInHandSlots = new WeaponItem[2];

    public int currentWeaponIndex = 0;

    private void Awake()
    {
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    private void Start()
    {
        //handWeapon = weaponsInHandSlots[currentWeaponIndex];
        weaponSlotManager.LoadWeaponOnSlot(handWeapon, false);
        weaponSlotManager.LoadWeaponOnSlot(offhandWeapon, true);
        
    }

    public void SwitchWeapon()
    {
        currentWeaponIndex = currentWeaponIndex + 1;

        if (currentWeaponIndex == 0 && weaponsInHandSlots[0] != null)
        {
            handWeapon = weaponsInHandSlots[currentWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInHandSlots[currentWeaponIndex], false);

        }
        else if (currentWeaponIndex == 1 && weaponsInHandSlots[1] != null)
        {
            handWeapon = weaponsInHandSlots[currentWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInHandSlots[currentWeaponIndex], false);
        }
        else 
        {
            currentWeaponIndex = currentWeaponIndex + 1;
        }

        if (currentWeaponIndex > weaponsInHandSlots.Length - 1)
        {
            currentWeaponIndex = -1;
            handWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(unarmedWeapon, false);
        }
    }
}
