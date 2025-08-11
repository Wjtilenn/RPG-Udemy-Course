using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_GameInerfaceSkillBar : MonoBehaviour
{
    private Image image;
    [SerializeField] Sprite defaultIcon;
    public SkillType skillType;

    public KeyCode key;
    
    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = defaultIcon;
    }

    public void SkillBarUpdate(UI_SkillBar _skillBar) 
    {
        image.sprite = _skillBar.image.sprite;
        skillType = _skillBar.skillType;
    }

}