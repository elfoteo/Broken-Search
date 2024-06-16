using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEffects: MonoBehaviour
{
    public GameObject uiUpgrade;
    public void HealPlayer(Player player)
    {
        player.Damage(-20);
        uiUpgrade.SetActive(false);
    }

    public void IncreaseDamageDelt(Player player)
    {
        player.IncreaseDamageDelt(5.0F);
        uiUpgrade.SetActive(false);
    }

    public void HitSpeed(Player player)
    {
        player.IncreaseHitSpeed(150);
        uiUpgrade.SetActive(false);
    }
}
