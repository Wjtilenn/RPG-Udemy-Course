using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescraption;

    public void ShowToolTip(string _name, string _text)
    {
        gameObject.SetActive(true);
        skillName.text = _name;
        skillDescraption.text = _text;
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
    
}
