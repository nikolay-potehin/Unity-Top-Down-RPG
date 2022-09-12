using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviour
{
    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    private List<Collider2D> hits = new List<Collider2D>();

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        // Collision work
        boxCollider.OverlapCollider(filter, hits);
        foreach (var hit in hits)
        {
            if (hit == null)
                continue;

            OnCollide(hit);
        }

        // The list is not cleaned up, so we do it ourselves
        hits.Clear();
    }

    protected virtual void OnCollide(Collider2D coll)
    {
        Debug.Log("OnCollide was not implemented in " + name);
    }
}
