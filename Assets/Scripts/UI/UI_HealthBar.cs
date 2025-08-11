using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStats;
    private RectTransform myTransrorm;
    private Slider slider;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        
        myStats = GetComponentInParent<CharacterStats>();
        myTransrorm = GetComponent<RectTransform>();
        
        slider = GetComponentInChildren<Slider>();

        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;
    }

    private void Start()
    {
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealth();
        slider.value = myStats.currentHealth;
    }

    private void FlipUI() => myTransrorm.Rotate(0, 180, 0);

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
