using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorUnit : MonoBehaviour
{
    [SerializeField] private new Collider2D collider;
    [SerializeField] private SpriteRenderer forbidden;
    [SerializeField] private SpriteRenderer[] spriteRenderers;

    [SerializeField] LayerMask layerMask;

    public int Cost
    {
        get; set;
    }

    public UnitButton Button
    {
        get; set;
    }

    public SpriteRenderer BoundsArea
    {
        get; set;
    }

    public bool CanBePlaced
    {
        get; private set;
    }

    void FixedUpdate()
    {
        CheckIfCanPlace();

        forbidden.gameObject.SetActive(!CanBePlaced);
    }

    private void CheckIfCanPlace()
    {
        CanBePlaced = true;

        if (Button == null || Button.OnCooldown)
        {
            CanBePlaced = false;
            return;
        }

        if (BoundsArea == null || !IsInsideBounds())
        {
            CanBePlaced = false;
            return;
        }

        if (Cost > GameManager.Numbers.Credits)
        {
            CanBePlaced = false;
            return;
        }

        List<Collider2D> collisionList = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(layerMask);
        int overlapAmount = Physics2D.OverlapCollider(collider, filter, collisionList);

        if (overlapAmount > 0)
        {
            CanBePlaced = false;
        }
    }

    public bool IsInsideBounds()
    {
        return BoundsArea.bounds.Contains(transform.position);
    }
}
