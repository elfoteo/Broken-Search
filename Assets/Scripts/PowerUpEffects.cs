using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEffects
{
    public void HealPlayer(Player player)
    {
        player.Damage(-20);
    }

    public void IncreaseDamageDelt(Player player)
    {
        player.IncreaseDamageDelt(5.0F);
    }

    public void HitSpeed(Player player)
    {
        player.IncreaseHitSpeed(150);
    }
}
