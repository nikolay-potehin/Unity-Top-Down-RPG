using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    // Experience
    public int xpValue = 0;

    // Public fields
    public float triggerLength = 0.3f;
    public float chaseLength = 1.0f;
    public float moveSpeed = 0.5f;

    // Logic
    private bool isChasing;
    private bool isCollidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    // Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitbox;
    private List<Collider2D> hits = new List<Collider2D>();

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitbox = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        // Is the player in range?
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
        {
            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
                isChasing = true;

            if (isChasing)
            {
                if (!isCollidingWithPlayer)
                {
                    UpdateMotor((playerTransform.position - transform.position).normalized * moveSpeed);
                }
                else
                {
                    UpdateMotor(Vector3.zero);
                }
            }
            else
            {
                if ((startingPosition - transform.position).magnitude > 0.01)
                    UpdateMotor((startingPosition - transform.position).normalized * moveSpeed);
            }
        }
        else
        {
            if ((startingPosition - transform.position).magnitude > 0.01)
                UpdateMotor((startingPosition - transform.position).normalized * moveSpeed);
            isChasing = false;
        }

        // Check for overlaps
        isCollidingWithPlayer = false;

        boxCollider.OverlapCollider(filter, hits);
        foreach (var hit in hits)
        {
            if (hit == null)
                continue;

            if (hit.CompareTag("Fighter") && hit.name == "Player")
            {
                isCollidingWithPlayer = true;
            }
        }

        // The list is not cleaned up, so we do it ourselves
        hits.Clear();
    }

    protected override void Death()
    {
        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + " xp", 6, Color.magenta, transform.position, Vector3.up, 1.0f);
    }
}
