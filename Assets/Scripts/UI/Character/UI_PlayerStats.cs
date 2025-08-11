using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStats : MonoBehaviour
{
    private PlayerStats playerStats;

    [SerializeField] private TextMeshProUGUI healthValue;
    [SerializeField] private TextMeshProUGUI attackValue;
    [SerializeField] private TextMeshProUGUI defenceValue;
    [SerializeField] private TextMeshProUGUI impartValue;
    [SerializeField] private TextMeshProUGUI critRateValue;
    [SerializeField] private TextMeshProUGUI critDamageValue;
    [SerializeField] private TextMeshProUGUI anomalyMasteryValue;
    [SerializeField] private TextMeshProUGUI anomalyProficencyValue;
    [SerializeField] private TextMeshProUGUI penetrationValue;
    [SerializeField] private TextMeshProUGUI penetrationRateValue;
    [SerializeField] private TextMeshProUGUI physcalDamageBounsValue;
    [SerializeField] private TextMeshProUGUI fireDamageBounsValue;
    [SerializeField] private TextMeshProUGUI iceDamageBounsValue;
    [SerializeField] private TextMeshProUGUI electricDamageBounsValue;



    private void Awake()
    {
        playerStats = PlayerManager.instance.player.stats as PlayerStats;
        
    }

    public void UpdateStatsValue()
    {
        healthValue.text = playerStats.health.GetValue().ToString();
        attackValue.text = playerStats.attack.GetValue().ToString();
        defenceValue.text = playerStats.defence.GetValue().ToString();
        impartValue.text = playerStats.impact.GetValue().ToString();
        critRateValue.text = playerStats.critRate.GetValue().ToString() + "%";
        critDamageValue.text = playerStats.critDamage.GetValue().ToString() + "%";
        anomalyMasteryValue.text = playerStats.anomalyMastery.GetValue().ToString();
        anomalyProficencyValue.text = playerStats.anomalyProficiency.GetValue().ToString();
        penetrationValue.text = playerStats.penetration.GetValue().ToString();
        penetrationRateValue.text = playerStats.penetrationRate.GetValue().ToString() + "%";
        physcalDamageBounsValue.text = playerStats.physicalDamageBouns.GetValue().ToString() + "%";
        fireDamageBounsValue.text = playerStats.fireDamageBouns.GetValue().ToString() + "%";
        iceDamageBounsValue.text = playerStats.iceDamageBouns.GetValue().ToString() + "%";
        electricDamageBounsValue.text = playerStats.electricDamageBouns.GetValue().ToString() + "%";

    }

}
