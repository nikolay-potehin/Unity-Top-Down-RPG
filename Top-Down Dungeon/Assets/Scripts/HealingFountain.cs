using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFountain : Collidable
{
    public int healingAmount;

    public float healCooldown = 1.5f;
    private float lastHealTime;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name != "Player")
            return;

        if (Time.time - lastHealTime > healCooldown)
        {
            lastHealTime = Time.time;
            GameManager.instance.player.Heal(healingAmount);
        }
    }
}
