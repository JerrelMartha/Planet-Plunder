using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private Weapon weaponSlot1;
    [SerializeField] private Weapon weaponSlot2;
    [SerializeField] private Weapon weaponSlot3;

    [SerializeField] private InputActionProperty activeWeapon1;
    [SerializeField] private InputActionProperty activeWeapon2;
    [SerializeField] private InputActionProperty activeWeapon3;

    private void OnEnable()
    {
        activeWeapon1.action?.Enable();
        activeWeapon2.action?.Enable();
        activeWeapon3.action?.Enable();
    }

    private void OnDisable()
    {
        activeWeapon1.action?.Disable();
        activeWeapon2.action?.Disable();
        activeWeapon3.action?.Disable();
    }

    private void Start()
    {
        // Force switch to slot 1 at start
        AttemptSwitch(1);
    }

    private void Update()
    {
        if (activeWeapon1.action != null && activeWeapon1.action.WasPressedThisFrame()) AttemptSwitch(1);
        if (activeWeapon2.action != null && activeWeapon2.action.WasPressedThisFrame()) AttemptSwitch(2);
        if (activeWeapon3.action != null && activeWeapon3.action.WasPressedThisFrame()) AttemptSwitch(3);
    }

    private void AttemptSwitch(int slot)
    {
        Weapon targetWeapon = GetWeaponBySlot(slot);
        if (targetWeapon == null) return;

        string targetID = GetIDBySlot(slot);

        // Check if the ID is "Drill" (Slot 1) or if it's unlocked in the Manager
        bool isStartingWeapon = (slot == 1);
        bool isUnlocked = WeaponManager.instance != null && WeaponManager.instance.IsIDUnlocked(targetID);

        if (isStartingWeapon || isUnlocked)
        {
            SwitchWeapon(slot);
        }
    }

    private void SwitchWeapon(int activeSlot)
    {
        ToggleWeaponSlot(weaponSlot1, activeSlot == 1);
        ToggleWeaponSlot(weaponSlot2, activeSlot == 2);
        ToggleWeaponSlot(weaponSlot3, activeSlot == 3);
    }

    /// <summary>
    /// Sets the GameObject active state and the Weapon component enabled state.
    /// </summary>
    private void ToggleWeaponSlot(Weapon weapon, bool isActive)
    {
        if (weapon != null)
        {
            weapon.enabled = isActive;
            weapon.gameObject.SetActive(isActive);
        }
    }

    private string GetIDBySlot(int slot)
    {
        return slot switch
        {
            1 => "Drill",
            2 => "Missile",
            3 => "ClusterBomb",
            _ => ""
        };
    }

    private Weapon GetWeaponBySlot(int slot)
    {
        return slot switch
        {
            1 => weaponSlot1,
            2 => weaponSlot2,
            3 => weaponSlot3,
            _ => null
        };
    }
}