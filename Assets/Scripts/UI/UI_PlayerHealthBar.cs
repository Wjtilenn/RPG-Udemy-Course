using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class UI_PlayerHealthBar : MonoBehaviour
{
    PlayerStats playerStats;

    private Slider slider;

    private void Start()
    {
        playerStats = PlayerManager.instance.player.stats as PlayerStats;
        slider = GetComponent<Slider>();

        playerStats.onHealthChanged += UpdateHealthUI;

    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealth();
        slider.value = playerStats.currentHealth;
    }

    private void OnDisable()
    {
        playerStats.onHealthChanged -= UpdateHealthUI;
    }

    private void OnDestroy()
    {
        playerStats.onHealthChanged -= UpdateHealthUI;
    }

}
