using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShowText : Collidable
{
    public string message;

    private float cooldown = 4.0f;
    private float lastShout = -4.0f;

    protected override void OnCollide(Collider2D coll)
    {
        if (Time.time - lastShout > cooldown)
        {
            lastShout = Time.time;
            GameManager.instance.ShowText(message, 6, Color.white, transform.position + Vector3.up, Vector3.zero, cooldown);
        }
    }
}
