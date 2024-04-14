using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Unit
{
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private SpriteRenderer loadingSpriteRenderer;

    private float buildTimer = 0.0f;

    public override void Init(Vector3 position)
    {
        ResetAnimation();
        state = State.Holding;
        CurrentHealth = info.health;
        transform.position = position;
        overlapFilter.SetLayerMask(targetMask);
        buildTimer = Time.realtimeSinceStartup;
    }

    new public void Update()
    {
        if (!GameManager.Instance.Running)
            return;

        switch (state)
        {
            case State.Holding:
                HoldingState();
                break;
            case State.Dead:
                DeadState();
                break;
        }
    }

    protected override void HoldingState()
    {
        //float nt = (Time.realtimeSinceStartup - buildTimer) / info.buildTime;
        //loadingSpriteRenderer.

        if(Time.realtimeSinceStartup - buildTimer > info.buildTime)
        {
            BuildUnit();
            buildTimer = Time.realtimeSinceStartup;
        }
    }

    public void BuildUnit()
    {
        var unit = UnitPool.GetUnit(info.buildUnit);
        unit.Init(spawnTransform.position);
        unit.gameObject.SetActive(true);
    }
}
