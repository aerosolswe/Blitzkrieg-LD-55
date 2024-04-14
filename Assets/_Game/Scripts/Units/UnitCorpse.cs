using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCorpse : Unit
{
    [SerializeField] private Sprite[] spriteArray;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private string animDecayName = "Anim_dead_soldier_decay";
    [SerializeField] private float decayTime = 10.0f;

    private float timeOfDeath = 0.0f;
    private bool animating = false;

    public override void Init(Vector3 position)
    {
        animation.Stop();
        transform.position = position;

        spriteRenderer.sprite = spriteArray[Random.Range(0, spriteArray.Length)];

        SetDead();
        animating = false;
        timeOfDeath = Time.realtimeSinceStartup;
    }

    protected override void DeadState()
    {
        base.DeadState();

        if (Time.realtimeSinceStartup - timeOfDeath > decayTime + 2)
        {
            gameObject.SetActive(false);
        } else if (Time.realtimeSinceStartup - timeOfDeath > decayTime && !animating)
        {
            animating = true;
            animation.Play(animDecayName);
        }
    }

    public override void SetDead()
    {
        CurrentHealth = 0;
        state = State.Dead;
    }
}
