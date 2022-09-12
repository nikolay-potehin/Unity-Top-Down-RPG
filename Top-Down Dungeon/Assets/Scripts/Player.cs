using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    public OldWeapon weapon;
    public bool AndroidMovement;
    public Joystick movementJoystick;
    public Joystick attackJoystick;
    public int OnStartMaxHitpoint;

    public bool IsAttacking;

    protected override void Start()
    {
        base.Start();
        immuneTime = 0.5f;
    }

    private void FixedUpdate()
    {
        if (!isAlive)
            return;

        float x, y;

        if (AndroidMovement)
        {
            x = movementJoystick.Horizontal;
            y = movementJoystick.Vertical;
        }
        else
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
        }

        //x = x >= 0.5 ? 1 : 0;
        //y = y >= 0.5 ? 1 : 0;

        UpdateMotor(new Vector3(x, y, 0));
    }

    protected virtual void Update()
    {
        if (!isAlive)
            return;

        if (IsAttacking)
            weapon.Attack();
    }

    public void Respawn()
    {
        isAlive = true;
    }
    public void SwapSprite(int skinId)
    {
        if (spriteRenderer == null)
            return;

        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
        Debug.Log(spriteRenderer);
    }
    public void SwapAttackMode() => IsAttacking = !IsAttacking;
    public void OnLevelUp()
    {
        maxHitpoint++;
        Heal(maxHitpoint);
    }
    public void SetLevel(int level)
    {
        maxHitpoint = OnStartMaxHitpoint;
        hitpoint = OnStartMaxHitpoint;
        for (int i = 1; i < level; i++)
            OnLevelUp();
    }
    public void Heal(int healingAmount)
    {
        if (hitpoint == maxHitpoint || !isAlive)
            return;

        if (hitpoint + healingAmount > maxHitpoint)
            healingAmount = maxHitpoint - hitpoint;

        hitpoint += healingAmount;
        GameManager.instance.ShowText("+" + healingAmount.ToString() + "hp", 6, Color.green, 
            transform.position, Vector3.up, 1.0f);
        GameManager.instance.OnHitpointChange();
    }
    protected override void ReceiveDamage(Damage dmg)
    {
        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitpointChange();
    }
    protected override void Death()
    {
        if (!isAlive)
            return;

        base.Death();

        GameManager.instance.OnPlayerDeath();
    }
}
