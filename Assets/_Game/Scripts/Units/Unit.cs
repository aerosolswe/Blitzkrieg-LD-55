using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup), typeof(Animation))]
public class Unit : MonoBehaviour
{
    public enum State
    {
        Moving, Attacking, Holding, Dead
    }

    // Serialized fields
    [SerializeField] protected Transform lookTransform;
    [SerializeField] protected SortingGroup sortingGroup;
    [SerializeField] protected new Animation animation;
    [SerializeField] protected UnitInfo info;
    [SerializeField] protected Collider2D overlapCollider;

    [SerializeField] protected LayerMask targetMask;

    [SerializeField] protected string animBaseName = "Anim_soldier_base";
    [SerializeField] protected string animFireName = "Anim_soldier_fire";
    [SerializeField] protected string animHitName = "Anim_soldier_hit";

    [SerializeField] protected float checkForTargetTime = 1.0f;

    protected ContactFilter2D overlapFilter = new ContactFilter2D();

    // Private fields
    protected State state = State.Moving;
    protected float lastTimeDamaged;
    protected float attackDelayTime;
    protected float lastTargetCheck = 0.0f;

    // Properties
    public int CurrentHealth
    {
        get; protected set;
    }

    public State CurrentState => state;

    public Unit Target
    {
        get; set;
    }

    public virtual void Init(Vector3 position)
    {
        ResetAnimation();
        state = State.Moving;
        CurrentHealth = info.health;
        transform.position = position;
        overlapFilter.SetLayerMask(targetMask);
    }

    public void OnEnable()
    {
        ResetAnimation();
    }

    public void Update()
    {
        if (!GameManager.Instance.Running)
            return;

        switch (state)
        {
            case State.Moving:
                MovingState();
                break;
            case State.Attacking:
                AttackingState();
                break;
            case State.Holding:
                HoldingState();
                break;
            case State.Dead:
                DeadState();
                break;
        }
    }

    protected virtual void MovingState()
    {
        LookDirection(info.faction == Faction.Allied ? Vector3.right : Vector3.left);
        CheckForTargets();
        Move();
    }

    protected virtual void AttackingState()
    {
        if (Target == null || Target.CurrentState == State.Dead)
        {
            state = State.Moving;
            Target = null;
            return;
        }

        LookDirection(Target.transform.position - transform.position);

        if (Time.realtimeSinceStartup - attackDelayTime > info.attackDelay)
        {
            TryToDamage();
        }
    }

    protected virtual void HoldingState()
    {
        CheckForTargets();
    }

    protected virtual void DeadState()
    {

    }

    protected virtual void Move()
    {
        bool canMove = true;

        if (info.faction == Faction.Allied)
        {
            if (transform.position.x > info.maxX)
            {
                canMove = false;
            }

            if (canMove)
            {
                List<Collider2D> colliders = new List<Collider2D>();
                if (Physics2D.OverlapCollider(overlapCollider, overlapFilter, colliders) > 0)
                {
                    foreach (Collider2D otherCollider in colliders)
                    {
                        if (transform.position.x < otherCollider.transform.position.x)
                        {
                            canMove = false;
                            break;
                        }
                    }
                }
            }
        } else if (info.faction == Faction.Axis)
        {
            if (transform.position.x < info.maxX)
            {
                GameManager.Instance.GameOver();
                canMove = false;
                gameObject.SetActive(false);
            }
        }


        if (canMove)
        {
            Vector3 moveDir = transform.right;
            moveDir *= info.speed * Time.deltaTime;
            Vector3 position = transform.localPosition + moveDir;
            transform.localPosition = position;
        }
    }

    protected virtual void CheckForTargets()
    {
        if (Time.realtimeSinceStartup - lastTargetCheck > checkForTargetTime)
        {
            lastTargetCheck = Time.realtimeSinceStartup;
            var colliders = Physics2D.OverlapCircleAll(transform.position, info.range, targetMask);

            foreach (var collider in colliders)
            {
                var unit = collider.GetComponent<Unit>();

                if (unit == null)
                {
                    unit = collider.GetComponentInParent<Unit>();
                }

                if (unit != null && unit != this && unit.info.faction != info.faction && unit.CurrentState != State.Dead)
                {
                    Target = unit;
                    state = State.Attacking;
                    attackDelayTime = Time.realtimeSinceStartup;
                    break;
                }
            }
        }
    }

    public virtual void TakeDamge(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            SetDead();
        } else
        {
            animation.Play(animHitName, AnimationPlayMode.Queue);
        }
    }


    public virtual void SetDead()
    {
        CurrentHealth = 0;
        state = State.Dead;

        GameManager.Numbers.Credits += info.creditsReward;
        GameManager.Numbers.Score += info.creditsReward;

        if (info.creditsReward > 0)
        {
            GameManager.Numbers.TotalKills += 1;
        }

        Unit corpse = UnitPool.GetUnit(info.corpseID);

        if (corpse != null)
        {
            ((UnitCorpse)corpse).Init(transform.position);
            corpse.lookTransform.right = lookTransform.right;
            corpse.gameObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }

    public virtual void TryToDamage()
    {
        float t = Time.realtimeSinceStartup - lastTimeDamaged;

        if (t > info.firerate)
        {
            animation.Play(animFireName, AnimationPlayMode.Queue);
            Target.TakeDamge(info.damage);

            if (info.aoe > 0.0f)
            {
                var additionalTargets = Physics2D.OverlapCircleAll(Target.transform.position, info.aoe, targetMask);

                foreach (var tar in additionalTargets)
                {
                    var unitTar = tar.GetComponent<Unit>();
                    if (unitTar != null)
                    {
                        unitTar.TakeDamge(info.damage);
                    }
                }
            }

            lastTimeDamaged = Time.realtimeSinceStartup;
        }
    }

    protected virtual void LookDirection(Vector3 dir)
    {
        lookTransform.right = Vector3.Lerp(lookTransform.right, dir.normalized, Time.deltaTime * info.turnSpeed);
    }

    public void ResetAnimation()
    {
        animation.Play(animBaseName);
    }

    public void Reset()
    {
        sortingGroup = GetComponent<SortingGroup>();
        animation = GetComponent<Animation>();

        if (lookTransform == null)
            lookTransform = transform;
    }
}
