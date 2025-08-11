using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_SkillBar : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    public SkillType skillType;
    public Image image;
    [SerializeField] private Sprite defaultIcon;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = defaultIcon;
    }

    public void OnDrop(PointerEventData eventData)
    {   
        UI_SkillSlot skillSlot = eventData.pointerDrag.GetComponent<UI_SkillSlot>();
        if(skillSlot == null || skillSlot.skillType == SkillType.None)
        {
            image.sprite = defaultIcon;
            skillType = SkillType.None;
        }
        else
        {
            image.sprite = skillSlot.skillImage.sprite;
            skillType = skillSlot.skillType;
        }
        SkillManager.instance.SkillBarUpdate();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        image.sprite = defaultIcon;
        skillType = SkillType.None;
        SkillManager.instance.SkillBarUpdate();
    }
}
