using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using Unity.VisualScripting;

public class UI_SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
    IBeginDragHandler, IDragHandler, IEndDragHandler, ISaveManager
{
    private UIManager ui;

    private GameObject skillImageDragPreview;

    public SkillType skillType;
    public bool unlocked {  get; private set; }
    [SerializeField] private Color lockedSkillColor;

    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;

    [SerializeField] private UI_SkillSlot[] shouldBeUnlocked;

    [HideInInspector] public Image skillImage;
    [HideInInspector] public UnityEvent unlockSkill;

    private void OnValidate()
    {
        gameObject.name = "SkillSlot - " + skillName;
    }

    private void Start()
    {

        ui = GetComponentInParent<UIManager>();
        skillImage = GetComponent<Image>();

        GetComponent<Button>().onClick.AddListener(UnlockSkill);

        if (unlocked)
        {
            unlockSkill.Invoke();
            skillImage.color = Color.white;
        }
    }

    public void UnlockSkill()
    {
        if (unlocked) return;

        for(int i = 0;i < shouldBeUnlocked.Length;i++)
        {
            if(!shouldBeUnlocked[i].unlocked)
            {
                Debug.Log("前置技能未解锁");
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
        unlockSkill.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillName, skillDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(skillType == SkillType.None || !unlocked) return;
       
        skillImageDragPreview = new GameObject("skillImageDragPreview");
        skillImageDragPreview.transform.SetParent(ui.transform);
        skillImageDragPreview.transform.position = Input.mousePosition;
        skillImageDragPreview.transform.localScale = new Vector3(1, 1, 1);

        Image image = skillImageDragPreview.AddComponent<Image>();
        image.sprite = skillImage.sprite;
        image.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        image.raycastTarget = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (skillType == SkillType.None || !unlocked) return;

        Vector3 mousePositon = Input.mousePosition;
        skillImageDragPreview.transform.position = mousePositon;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (skillType == SkillType.None || !unlocked) return;
        
        Destroy(skillImageDragPreview);
    }

    public void LoadData(GameData _data)
    {
        if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData _data)
    {
         if(_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else
        {
            _data.skillTree.Add(skillName, unlocked);
        }
    }
}
