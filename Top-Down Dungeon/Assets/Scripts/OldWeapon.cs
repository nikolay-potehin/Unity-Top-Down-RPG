using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldWeapon : Collidable
{
    // Damage struct
    public int[] damagePoint = { 10, 15, 20, 25, 30, 35, 40, 50 };
    public float[] pushForce = { 2.0f, 2.25f, 2.5f, 2.75f, 3f, 3.25f, 3.5f, 4f };

    // Upgrade
    public int weaponLevel = 0;
    public SpriteRenderer spriteRenderer;

    // Swing
    private Animator animator;
    private float cooldown = 0.5f;
    private float lastSwing;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (Time.time - lastSwing > cooldown)
        {
            lastSwing = Time.time;
            animator.SetTrigger("Swing");
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.CompareTag("Fighter"))
        {
            if (coll.name == "Player")
                return;

            // Create a new damage object, then we'll send it to the fighter we've hit
            Damage dmg = new()
            {
                damageAmount = damagePoint[weaponLevel],
                origin = GetComponentInParent<Transform>().position,
                pushForce = pushForce[weaponLevel]
            };

            coll.SendMessage("ReceiveDamage", dmg);
        }
    }

    public void UpgradeWeapon()
    {
        SetWeaponLevel(weaponLevel + 1);
    }
    
    public void SetWeaponLevel(int level)
    {
        if (level >= 0 && level < damagePoint.Length)
        {
            weaponLevel = level;
            spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
        }
    }
}
