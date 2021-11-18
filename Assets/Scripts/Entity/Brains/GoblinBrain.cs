using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class GoblinBrain : HostileEntity
{
    public float Speed = 10.0f;

    public float DetectionRange = 30f;
    public float AttackRange = 1f;
    [Layer]
    public List<int> TargetLayerMaskIndexes;

    public float AttackDamage = 10f;
    public float AttacksPerSecond = 0.5f;
    public float Knockback = 1.0f;

    private int _targetLayerMask = 0;
    public Entity _target = null;

    private float _lastAttackTime = 0.0f;

    public override void Awake()
    {
        base.Awake();

        Name = "Goblin";

        foreach (var index in TargetLayerMaskIndexes)
        {
            _targetLayerMask |= 1 << index;
        }
    }

    public override void Update()
    {
        base.Update();

        if (_target == null)
        {
            _target = FindTarget();
        }
        else if (Vector3.Distance(transform.position, _target.transform.position) > DetectionRange)
        {
            _target = null;
        }

        if (_target)
        {
            var direction = (_target.transform.position - transform.position).normalized;
            if (Vector3.Distance(transform.position, _target.transform.position) > AttackRange)
            {
                var velocity = direction * Speed;
                _rigidbody.velocity = velocity;
            }
            else if (AttacksPerSecond >= 0.0f && ((Time.time - _lastAttackTime) > (1.0f / AttacksPerSecond)))
            {
                _lastAttackTime = Time.time;
                _target.TakeDamage(AttackDamage, direction * Knockback);
            }
        }
    }

    private Entity FindTarget()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, DetectionRange, _targetLayerMask);

        Collider2D closestCollider = null;
        float closestDistance = float.MaxValue;

        foreach (var collider in colliders)
        {
            var distance = Vector2.Distance(transform.position, collider.transform.position);

            if (distance < closestDistance)
            {
                closestCollider = collider;
                closestDistance = distance;
            }
        }

        return closestCollider?.GetComponent<Entity>();
    }

    public override void RandomizeAttributes(int minLevel, int maxLevel)
    {
        base.RandomizeAttributes(minLevel, maxLevel);

        var rand = Random.Range(0f, 100f);

        Lvl = Random.Range(minLevel, maxLevel);

        if (rand <= 0.03f)
        {
            Title = "Overpowered";
            Speed = Random.Range(12f, 20f);
            AttackDamage = Random.Range(20f, 30f);
            AttacksPerSecond = Random.Range(2f, 3f);
            Knockback = Random.Range(4.0f, 10.0f);
        }
        else if (rand <= 0.06f)
        {
            Title = "Powerful";
            Speed = Random.Range(10f, 15f);
            AttackDamage = Random.Range(15f, 25f);
            AttacksPerSecond = Random.Range(1.5f, 2.5f);
            Knockback = Random.Range(2.0f, 5.0f);
        }
        else if (rand <= 0.09f)
        {
            Title = "Average";
            Speed = Random.Range(8f, 12f);
            AttackDamage = Random.Range(10f, 15f);
            AttacksPerSecond = Random.Range(1.0f, 1.5f);
            Knockback = Random.Range(1.0f, 3.0f);
        }
        else if (rand <= 0.12f)
        {
            Title = "Weak";
            Speed = Random.Range(5f, 8f);
            AttackDamage = Random.Range(5f, 10f);
            AttacksPerSecond = Random.Range(0.5f, 1.0f);
            Knockback = Random.Range(0.5f, 2.0f);
        }
        else if (rand <= 0.15f)
        {
            Title = "Unpowered";
            Speed = Random.Range(3f, 5f);
            AttackDamage = Random.Range(5f, 10f);
            AttacksPerSecond = Random.Range(0.0f, 0.5f);
            Knockback = Random.Range(0.5f, 1.0f);
        }
        else
        {
            Title = "";
        }

        Speed += (Lvl * 0.5f);
        AttackDamage += (Lvl * 0.5f);
        AttacksPerSecond += (Lvl * 0.1f);
    }
}
