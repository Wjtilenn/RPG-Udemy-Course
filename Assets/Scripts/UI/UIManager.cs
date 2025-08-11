using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("死亡动画相关")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject DeadText;
    [SerializeField] private GameObject restartButton;
    [Space]

    [SerializeField] private GameObject menuHeader;
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject gameInterfaceUI;

    [SerializeField] private UI_Character ui_Character;
    [SerializeField] private UI_Craft ui_Craft;
    
    
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;

    public UI_SkillToolTip skillToolTip;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    private void Start()
    {
        SwitchTo(gameInterfaceUI);

        fadeScreen.gameObject.SetActive(true);

        //itemToolTip.gameObject.SetActive(false);
        //statToolTip.gameObject.SetActive(false);
        skillToolTip.gameObject.SetActive(false);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(characterUI);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchWithKeyTo(craftUI);
        } 
        if(Input.GetKeyDown(KeyCode.K))
        {
            SwitchWithKeyTo(skillTreeUI);
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            SwitchWithKeyTo(optionsUI);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchWithKeyTo(gameInterfaceUI);
        }
        
    }

    #region 背包界面切换
    public void SwitchTo(GameObject _menu)
    {
        menuHeader.SetActive(false);
        characterUI.SetActive(false);
        skillTreeUI.SetActive(false);
        craftUI.SetActive(false);
        optionsUI.SetActive(false);
        gameInterfaceUI.SetActive(false);

        if (_menu != null)
        {
            if(_menu == gameInterfaceUI)
            {
                GameManager.instance.PauseGame(false);
            }   
            else
            {
                GameManager.instance.PauseGame(true);
                SetMenuActive(true);
            }
            _menu.SetActive(true);
        }

    }

    public void SwitchWithKeyTo(GameObject _meun)
    {
        if(_meun != null && _meun.activeSelf)
        {
            SwitchTo(gameInterfaceUI);
            return;
        }
        SwitchTo(_meun);
    }

    public void SetMenuActive(bool _flag)
    {
        menuHeader.SetActive(_flag);
    }
    #endregion

    public void SwithOnEndScreen()
    {
        SwitchTo(null);
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutione());

    }

    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(1f);
        DeadText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();

    #region 穿戴与卸下装备

    public void EquipItem(InventoryItem _item)
    {
        ui_Character.EquipItem(_item);
        UpdateEquipmentInventory();
        UpdatePlayerStatsUI();
    }

    public void UnequipItem(InventoryItem _item)
    {
        ui_Character.UnequipItem(_item);
        UpdateEquipmentInventory();
        UpdatePlayerStatsUI();
    }

    #endregion

    #region UI更新

    public void UpdateEquipmentInventory()
    {
        ui_Character.UpdateEquipmentInventy();
    }
    public void UpdatePlayerStatsUI()
    {
        ui_Character.UpdatePlayerStatsUI();
    }
    public void SetupCraftableItem(ItemData_Equipment _item)
    {
        ui_Craft.SetupCraftableItem(_item);
    }
    public void UpdateMaterialInventory()
    {
        ui_Craft.UpdateMaterialInventory();
    }

    #endregion
}
