using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Character : MonoBehaviour
{
    [SerializeField] private GameObject equipmentInventory;
    [SerializeField] private GameObject playerEquipment;
    [SerializeField] private GameObject playerStatsUI;

    public UI_PlayerEquipmentSlot[] playerEquipmentSlots;

    private void Awake()
    {
        playerEquipmentSlots = GetComponentsInChildren<UI_PlayerEquipmentSlot>(true);
    }

    private void OnEnable()
    {
        SwitchToPlayerEquipment();

        UpdatePlayerStatsUI();
        UpdateEquipmentInventy();
        
    }

    public void SwitchToPlayerEquipment()
    {
        playerEquipment.SetActive(true);
        playerStatsUI.SetActive(false);
    }

    public void SwitchToPlayerStats()
    {
        playerEquipment.SetActive(false);
        playerStatsUI.SetActive(true);
    }

    #region 穿戴与卸下装备
    public void EquipItem(InventoryItem _item)
    {
        for (int i = 0; i < playerEquipmentSlots.Length; i++)
        {
            if (playerEquipmentSlots[i].item == null && playerEquipmentSlots[i].equipmentType == ((ItemData_Equipment)_item.data).equipmentType)
            {
                playerEquipmentSlots[i].SetupSlot(_item);
                InventoryManager.instance.EquipItem(_item);
                break;
            }
        }
    }
    public void UnequipItem(InventoryItem _item)
    {
        for (int i = 0; i < playerEquipmentSlots.Length; i++)
        {
            if (playerEquipmentSlots[i].item != null && playerEquipmentSlots[i].item == _item)
            {
                playerEquipmentSlots[i].CleanUpSlot();
                InventoryManager.instance.UnequipItem(_item);
                break;
            }
        }
    }
    #endregion
    public void UpdatePlayerStatsUI()
    {
        playerStatsUI.GetComponent<UI_PlayerStats>()?.UpdateStatsValue();
    }

    public void UpdateEquipmentInventy()
    {
        equipmentInventory.GetComponent<UI_EquipmentInventory>()?.UpdateItemSlotUI();
    }
}
